using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace PotatoFinch.TmgDotsJam.Movement {
	[UpdateInGroup(typeof(MovementSystemGroup))]
	public partial struct RotateTowardsVelocitySystem : ISystem {
		private float3 _vectorUp;

		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			_vectorUp = new float3(0f, 1f, 0f);
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			state.Dependency = new RotateTowardsVelocityJob { VectorUp = _vectorUp }.ScheduleParallel(state.Dependency);
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
		}

		[BurstCompile]
		private partial struct RotateTowardsVelocityJob : IJobEntity {
			public float3 VectorUp;

			public void Execute(RefRO<Velocity> velocity, RefRW<LocalTransform> localTransform) {
				if (math.all(velocity.ValueRO.Value == float3.zero)) {
					return;
				}
				
				localTransform.ValueRW.Rotation = quaternion.LookRotation(math.normalize(velocity.ValueRO.Value), VectorUp);
			}
		}
	}
}