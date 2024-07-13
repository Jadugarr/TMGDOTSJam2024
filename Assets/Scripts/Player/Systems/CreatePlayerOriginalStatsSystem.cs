using PotatoFinch.TmgDotsJam.Health;
using PotatoFinch.TmgDotsJam.Movement;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace PotatoFinch.TmgDotsJam {
	[UpdateInGroup(typeof(PlayerSystemGroup))]
	public partial struct CreatePlayerOriginalStatsSystem : ISystem, ISystemStartStop {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<PlayerTag>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
		}

		public void OnStartRunning(ref SystemState state) {
			var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
			var localTransform = SystemAPI.GetComponentRO<LocalTransform>(playerEntity);
			var characterHealth = SystemAPI.GetComponentRO<CharacterHealth>(playerEntity);
			var movementSpeed = SystemAPI.GetComponentRO<MovementSpeed>(playerEntity);

			var playerOriginalStatsEntity = state.EntityManager.CreateEntity(typeof(OriginalPlayerStats));
			
			state.EntityManager.SetComponentData(playerOriginalStatsEntity, new OriginalPlayerStats {
				OriginalPosition = localTransform.ValueRO.Position,
				MovementSpeed = movementSpeed.ValueRO.Value,
				MaxHealth = characterHealth.ValueRO.MaxHealth
			});
		}

		public void OnStopRunning(ref SystemState state) {
		}
	}
}