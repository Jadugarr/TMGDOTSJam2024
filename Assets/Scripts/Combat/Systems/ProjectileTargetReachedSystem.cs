using PotatoFinch.TmgDotsJam.Health;
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

			var damageEnemiesJob = new DamageHitEnemiesJob {
				ProjectileArray = projectileArray,
				CharacterHealthLookup = SystemAPI.GetComponentLookup<CharacterHealth>(),
				TargetEnemyLookup = SystemAPI.GetComponentLookup<TargetEnemy>(true),
				DamageValueLookup = SystemAPI.GetComponentLookup<DamageValue>(true),
			}.Schedule(checkJobHandle);

			state.Dependency =
				new DestroyProjectilesJob {
						ProjectileArray = projectileArray,
						ParallelEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
					}
					.Schedule(projectileArray.Length, 64, damageEnemiesJob);

			projectileArray.Dispose(state.Dependency);
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
		}

		[BurstCompile]
		private partial struct CheckProjectileReachedTargetJob : IJobEntity {
			[ReadOnly] public ComponentLookup<LocalTransform> LocalTransformLookup;

			[NativeDisableParallelForRestriction] public NativeArray<Entity> ProjectileArray;

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

		[BurstCompile]
		private struct DamageHitEnemiesJob : IJob {
			[ReadOnly] public NativeArray<Entity> ProjectileArray;

			[ReadOnly] public ComponentLookup<TargetEnemy> TargetEnemyLookup;
			[ReadOnly] public ComponentLookup<DamageValue> DamageValueLookup;
			public ComponentLookup<CharacterHealth> CharacterHealthLookup;


			public void Execute() {
				foreach (var projectileEntity in ProjectileArray) {
					if (!TargetEnemyLookup.TryGetComponent(projectileEntity, out TargetEnemy targetEnemy)) {
						continue;
					}

					if (!CharacterHealthLookup.TryGetComponent(targetEnemy.Value, out CharacterHealth enemyHealth)) {
						continue;
					}

					if (!DamageValueLookup.TryGetComponent(projectileEntity, out DamageValue damageValue)) {
						continue;
					}

					CharacterHealthLookup[targetEnemy.Value] = new CharacterHealth { MaxHealth = enemyHealth.MaxHealth, CurrentHealth = enemyHealth.CurrentHealth - damageValue.Value };
				}
			}
		}
	}
}