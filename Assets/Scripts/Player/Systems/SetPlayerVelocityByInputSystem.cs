using PotatoFinch.TmgDotsJam.GameCamera;
using PotatoFinch.TmgDotsJam.GameControls;
using PotatoFinch.TmgDotsJam.GameTime;
using PotatoFinch.TmgDotsJam.Movement;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace PotatoFinch.TmgDotsJam {
	[UpdateInGroup(typeof(PlayerSystemGroup))]
	public partial struct SetPlayerVelocityByInputSystem : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<PlayerTag>();
			state.RequireForUpdate<CurrentGameInput>();
			state.RequireForUpdate<GameCameraComponent>();
			state.RequireForUpdate<GameTimeComponent>();
		}

		public void OnUpdate(ref SystemState state) {
			var currentGameInput = SystemAPI.GetSingleton<CurrentGameInput>();
			var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
			var playerVelocity = SystemAPI.GetComponentRW<Velocity>(playerEntity);
			var movementSpeed = SystemAPI.GetComponentRO<MovementSpeed>(playerEntity);
			var gameCameraComponent = SystemAPI.ManagedAPI.GetSingleton<GameCameraComponent>();
			var gameTimeComponent = SystemAPI.GetSingleton<GameTimeComponent>();

			if (math.all(currentGameInput.CurrentMovementInputVector == float2.zero)) {
				playerVelocity.ValueRW.Value = 0f;
				return;
			}

			var movementVector = new float3(currentGameInput.CurrentMovementInputVector.x, 0f, currentGameInput.CurrentMovementInputVector.y);
			movementVector = Quaternion.Euler(0f, gameCameraComponent.Value.transform.eulerAngles.y, 0f) * movementVector;
			movementVector.y = 0f;
			movementVector = math.normalize(movementVector);
			playerVelocity.ValueRW.Value = movementVector * movementSpeed.ValueRO.Value * gameTimeComponent.DeltaTime;
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
		}
	}
}