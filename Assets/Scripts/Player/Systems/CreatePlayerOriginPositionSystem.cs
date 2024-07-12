using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace PotatoFinch.TmgDotsJam {
	[UpdateInGroup(typeof(PlayerSystemGroup))]
	public partial struct CreatePlayerOriginPositionSystem : ISystem, ISystemStartStop {
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
			var localTransform = SystemAPI.GetComponent<LocalTransform>(SystemAPI.GetSingletonEntity<PlayerTag>());

			var playerOriginEntity = state.EntityManager.CreateEntity(typeof(PlayerOriginPosition));
			state.EntityManager.SetComponentData(playerOriginEntity, new PlayerOriginPosition { Value = localTransform.Position });
		}

		public void OnStopRunning(ref SystemState state) { }
	}
}