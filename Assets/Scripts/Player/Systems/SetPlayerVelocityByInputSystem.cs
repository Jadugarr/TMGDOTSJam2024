using PotatoFinch.TmgDotsJam.GameCamera;
using PotatoFinch.TmgDotsJam.GameControls;
using PotatoFinch.TmgDotsJam.Movement;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace PotatoFinch.TmgDotsJam {
	[UpdateInGroup(typeof(PlayerSystemGroup))]
	public partial struct SetPlayerVelocityByInputSystem : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<PlayerTag>();
			state.RequireForUpdate<CurrentGameInput>();
			state.RequireForUpdate<GameCameraComponent>();
		}

		public void OnUpdate(ref SystemState state) {
			var currentGameInput = SystemAPI.GetSingleton<CurrentGameInput>();
			var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
			var playerVelocity = SystemAPI.GetComponentRW<Velocity>(playerEntity);
			var movementSpeed = SystemAPI.GetComponentRO<MovementSpeed>(playerEntity);
			var gameCameraComponent = SystemAPI.ManagedAPI.GetSingleton<GameCameraComponent>();

			if (math.all(currentGameInput.CurrentMovementInputVector == float2.zero)) {
				playerVelocity.ValueRW.Value = 0f;
				return;
			}

			var movementVector = new float3(currentGameInput.CurrentMovementInputVector.x, 0f, currentGameInput.CurrentMovementInputVector.y);
			movementVector = gameCameraComponent.Value.transform.rotation * movementVector;
			movementVector.y = 0f;
			playerVelocity.ValueRW.Value = movementVector * movementSpeed.ValueRO.Value * SystemAPI.Time.DeltaTime;
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
		}
	}
}