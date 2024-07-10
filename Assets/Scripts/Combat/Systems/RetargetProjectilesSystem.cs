using PotatoFinch.TmgDotsJam.Enemy;
using PotatoFinch.TmgDotsJam.Health;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace PotatoFinch.TmgDotsJam.Combat {
	[UpdateInGroup(typeof(CombatSystemGroup))]
	public partial struct RetargetProjectilesSystem : ISystem {
		private EntityQuery _enemyQuery;

		public void OnCreate(ref SystemState state) {
			_enemyQuery = state.GetEntityQuery(typeof(EnemyTag), typeof(LocalTransform));

			state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			var enemyEntityArray = _enemyQuery.ToEntityArray(Allocator.TempJob);
			var enemyPositionArray = _enemyQuery.ToComponentDataArray<LocalTransform>(Allocator.TempJob);

			state.Dependency = new RetargetProjectileJob {
				CharacterDeadLookup = SystemAPI.GetComponentLookup<CharacterDeadTag>(true),
				EnemyArray = enemyEntityArray,
				EnemyPositionArray = enemyPositionArray,
				ParallelEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
			}.ScheduleParallel(state.Dependency);

			enemyEntityArray.Dispose(state.Dependency);
			enemyPositionArray.Dispose(state.Dependency);
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
		}

		private partial struct RetargetProjectileJob : IJobEntity {
			[ReadOnly] public NativeArray<Entity> EnemyArray;
			[ReadOnly] public NativeArray<LocalTransform> EnemyPositionArray;
			[ReadOnly] public ComponentLookup<CharacterDeadTag> CharacterDeadLookup;

			public EntityCommandBuffer.ParallelWriter ParallelEcb;

			public void Execute(Entity projectileEntity, [EntityIndexInQuery] int index, RefRW<TargetEnemy> targetEnemy, RefRO<LocalTransform> projectilePosition) {
				if (targetEnemy.ValueRO.Value != Entity.Null) {
					if (!CharacterDeadLookup.TryGetComponent(targetEnemy.ValueRO.Value, out _)) {
						return;
					}
				}

				float shortestEnemyDistance = float.MaxValue;
				Entity enemyEntity = Entity.Null;

				for (var i = 0; i < EnemyPositionArray.Length; i++) {
					var enemyPosition = EnemyPositionArray[i];
					var distance = math.distance(enemyPosition.Position, projectilePosition.ValueRO.Position);
					var currentEnemyEntityToCheck = EnemyArray[i];
					if (distance >= shortestEnemyDistance || CharacterDeadLookup.TryGetComponent(currentEnemyEntityToCheck, out _)) {
						continue;
					}

					shortestEnemyDistance = distance;
					enemyEntity = currentEnemyEntityToCheck;
				}

				if (enemyEntity == Entity.Null) {
					ParallelEcb.DestroyEntity(index, projectileEntity);
					return;
				}

				targetEnemy.ValueRW.Value = enemyEntity;
			}
		}
	}
}