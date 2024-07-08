using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace PotatoFinch.TmgDotsJam.Movement {
	[UpdateInGroup(typeof(MovementSystemGroup))]
	public partial struct VelocityMovementSystem : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			state.Dependency = new MoveByVelocityJob().ScheduleParallel(state.Dependency);
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {

		}

		[BurstCompile]
		private partial struct MoveByVelocityJob : IJobEntity {
			public void Execute(RefRW<LocalTransform> localTransform, RefRO<Velocity> velocity) {
				localTransform.ValueRW.Position += velocity.ValueRO.Value;
			}
		} 
	}
}