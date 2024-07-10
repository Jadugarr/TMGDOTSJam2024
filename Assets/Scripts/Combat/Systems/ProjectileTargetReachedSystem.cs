using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace PotatoFinch.TmgDotsJam.Combat {
	[UpdateInGroup(typeof(CombatSystemGroup))]
	public partial struct ProjectileTargetReachedSystem : ISystem {
		private EntityQuery _projectileQuery;

		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
			_projectileQuery = state.GetEntityQuery(typeof(ProjectileTag));

			state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			var projectileArray = new NativeArray<Entity>(_projectileQuery.CalculateEntityCount(), Allocator.TempJob);

			JobHandle checkJobHandle = new CheckProjectileReachedTargetJob {
				LocalTransformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true),
				ProjectileArray = projectileArray,
			}.ScheduleParallel(state.Dependency);

			state.Dependency =
				new DestroyProjectilesJob {
						ProjectileArray = projectileArray,
						ParallelEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
					}
					.Schedule(projectileArray.Length, 64, checkJobHandle);

			projectileArray.Dispose(state.Dependency);
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
		}

		[BurstCompile]
		private partial struct CheckProjectileReachedTargetJob : IJobEntity {
			[ReadOnly] public ComponentLookup<LocalTransform> LocalTransformLookup;

			[NativeDisableParallelForRestriction]
			public NativeArray<Entity> ProjectileArray;

			public void Execute(Entity projectileEntity, [EntityIndexInQuery] int index, RefRO<TargetEnemy> targetEnemy, RefRO<LocalTransform> projectilePosition) {
				if (!LocalTransformLookup.TryGetComponent(targetEnemy.ValueRO.Value, out LocalTransform enemyPosition)) {
					return;
				}

				if (math.distance(projectilePosition.ValueRO.Position, enemyPosition.Position) > 0.5f) {
					return;
				}

				ProjectileArray[index] = projectileEntity;
			}
		}

		[BurstCompile]
		private struct DestroyProjectilesJob : IJobParallelFor {
			[ReadOnly, NativeDisableParallelForRestriction]
			public NativeArray<Entity> ProjectileArray;

			public EntityCommandBuffer.ParallelWriter ParallelEcb;

			public void Execute(int index) {
				if (index > ProjectileArray.Length - 1 || ProjectileArray[index] == Entity.Null) {
					return;
				}

				ParallelEcb.DestroyEntity(index, ProjectileArray[index]);
			}
		}
	}
}