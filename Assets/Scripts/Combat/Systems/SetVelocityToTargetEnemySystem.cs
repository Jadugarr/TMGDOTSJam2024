using PotatoFinch.TmgDotsJam.Movement;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace PotatoFinch.TmgDotsJam.Combat {
	[UpdateInGroup(typeof(CombatSystemGroup))]
	public partial struct SetVelocityToTargetEnemySystem : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			state.Dependency = new SetVelocityJob { DeltaTime = SystemAPI.Time.DeltaTime, LocalTransformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true) }.ScheduleParallel(state.Dependency);
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
		}

		[BurstCompile]
		private partial struct SetVelocityJob : IJobEntity {
			[ReadOnly] public ComponentLookup<LocalTransform> LocalTransformLookup;

			public float DeltaTime;

			public void Execute(RefRW<Velocity> velocity, RefRO<LocalTransform> localTransform, RefRO<MovementSpeed> movementSpeed, RefRO<TargetEnemy> targetEnemy) {
				if (!LocalTransformLookup.TryGetComponent(targetEnemy.ValueRO.Value, out LocalTransform enemyLocalTransform)) {
					return;
				}
				
				float3 dir = math.normalize(enemyLocalTransform.Position - localTransform.ValueRO.Position);

				velocity.ValueRW.Value = dir * movementSpeed.ValueRO.Value * DeltaTime;
			}
		}
	}
}