using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace PotatoFinch.TmgDotsJam.GameCamera {
	[UpdateInGroup(typeof(GameCameraSystemGroup))]
	public partial struct FocusCameraOnEntitySystem : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<EntityToFocus>();
			state.RequireForUpdate<CameraFollowTargetTag>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			var cameraFollowTargetEntity = SystemAPI.GetSingletonEntity<CameraFollowTargetTag>();
			var entityToFocus = SystemAPI.GetSingleton<EntityToFocus>().Value;

			if (entityToFocus == Entity.Null) {
				return;
			}

			var focusLocalTransform = SystemAPI.GetComponentRO<LocalTransform>(entityToFocus);
			SystemAPI.GetComponentRW<LocalTransform>(cameraFollowTargetEntity).ValueRW = focusLocalTransform.ValueRO;
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {

		}
	}
}