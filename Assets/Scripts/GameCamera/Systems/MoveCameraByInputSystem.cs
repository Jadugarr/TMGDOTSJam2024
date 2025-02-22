﻿using PotatoFinch.TmgDotsJam.GameControls;
using PotatoFinch.TmgDotsJam.GameTime;
using PotatoFinch.TmgDotsJam.Movement;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace PotatoFinch.TmgDotsJam.GameCamera {
	[DisableAutoCreation]
	[UpdateInGroup(typeof(GameCameraSystemGroup))]
	public partial struct MoveCameraByInputSystem : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<CameraFollowTargetTag>();
			state.RequireForUpdate<CurrentGameInput>();
			state.RequireForUpdate<GameCameraComponent>();
			state.RequireForUpdate<GameTimeComponent>();
		}

		public void OnUpdate(ref SystemState state) {
			var currentInput = SystemAPI.GetSingleton<CurrentGameInput>();
			var gameTimeComponent = SystemAPI.GetSingleton<GameTimeComponent>();

			if (math.all(currentInput.CurrentMovementInputVector == float2.zero)) {
				return;
			}

			var cameraComponent = SystemAPI.ManagedAPI.GetSingleton<GameCameraComponent>();
			var cameraFollowTargetEntity = SystemAPI.GetSingletonEntity<CameraFollowTargetTag>();
			var movementSpeed = SystemAPI.GetComponentRO<MovementSpeed>(cameraFollowTargetEntity);
			var localTransform = SystemAPI.GetComponentRW<LocalTransform>(cameraFollowTargetEntity);

			var movementVector = new float3(currentInput.CurrentMovementInputVector.x, 0f, currentInput.CurrentMovementInputVector.y);
			movementVector = cameraComponent.Value.transform.rotation * movementVector;
			movementVector.y = 0f;
			localTransform.ValueRW.Position += movementVector * movementSpeed.ValueRO.Value * gameTimeComponent.DeltaTime;
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) { }
	}
}