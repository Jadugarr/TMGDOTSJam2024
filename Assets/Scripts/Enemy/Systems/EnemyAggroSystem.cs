using PotatoFinch.TmgDotsJam.Combat;
using PotatoFinch.TmgDotsJam.Health;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace PotatoFinch.TmgDotsJam.Enemy {
	[UpdateInGroup(typeof(EnemySystemGroup))]
	public partial struct EnemyAggroSystem : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
			state.RequireForUpdate<PlayerTag>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
			var playerPosition = SystemAPI.GetComponentRO<LocalTransform>(playerEntity);

			var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

			foreach ((RefRO<LocalTransform> enemyPosition, RefRO<AggroRange> aggroRange, Entity enemyEntity) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<AggroRange>>().WithAll<EnemyTag>().WithNone<TargetEnemy>().WithEntityAccess()) {
				if (math.distance(playerPosition.ValueRO.Position, enemyPosition.ValueRO.Position) > aggroRange.ValueRO.Value) {
					continue;
				}

				ecb.AddComponent(enemyEntity, new TargetEnemy { Value = playerEntity });
			}

			// TODO: This will crash if player is in aggro range and hits an enemy in the same frame
			foreach ((RefRO<CharacterHealth> enemyHealth, Entity enemyEntity) in SystemAPI.Query<RefRO<CharacterHealth>>().WithChangeFilter<CharacterHealth>().WithAll<EnemyTag>().WithNone<TargetEnemy>().WithEntityAccess()) {
				if (enemyHealth.ValueRO.CurrentHealth >= enemyHealth.ValueRO.MaxHealth) {
					continue;
				}

				ecb.AddComponent(enemyEntity, new TargetEnemy { Value = playerEntity });
			}
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
		}
	}
}