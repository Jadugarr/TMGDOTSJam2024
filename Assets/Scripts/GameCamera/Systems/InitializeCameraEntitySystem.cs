using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace PotatoFinch.TmgDotsJam.GameCamera {
	[UpdateInGroup(typeof(GameCameraSystemGroup), OrderFirst = true)]
	public partial struct InitializeCameraEntitySystem : ISystem, ISystemStartStop {
		public void OnCreate(ref SystemState state) {
		}

		public void OnStartRunning(ref SystemState state) {
			var gameCameraEntity = state.EntityManager.CreateEntity(typeof(GameCameraComponent));
			state.EntityManager.SetComponentData(gameCameraEntity, new GameCameraComponent { Value = Camera.main });
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