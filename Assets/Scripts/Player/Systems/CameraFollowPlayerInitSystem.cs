using PotatoFinch.TmgDotsJam.GameCamera;
using Unity.Burst;
using Unity.Entities;

namespace PotatoFinch.TmgDotsJam {
	public partial struct CameraFollowPlayerInitSystem : ISystem, ISystemStartStop {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<CameraFollowTargetTag>();
			state.RequireForUpdate<PlayerTag>();
		}

		public void OnStartRunning(ref SystemState state) {
			var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
			var cameraFollowEntity = SystemAPI.GetSingletonEntity<CameraFollowTargetTag>();

			SystemAPI.SetComponent(cameraFollowEntity, new EntityToFocus { Value = playerEntity });
		}

		public void OnStopRunning(ref SystemState state) {
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
		}
	}
}