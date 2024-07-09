using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PotatoFinch.TmgDotsJam.GameControls {
	public partial class HandleGameInputSystem : SystemBase {
		private GameInputActions _gameInputActions;

		private Vector2 _currentMovementInputVector;
		private bool _killRandomEnemy;

		protected override void OnCreate() {
			_gameInputActions = new GameInputActions();
			_gameInputActions.Enable();
			
			_gameInputActions.Gameplay.PlayerMovement.performed += OnPlayerMovementPerformed;
			_gameInputActions.Gameplay.PlayerMovement.canceled += OnPlayerMovementCanceled;
			
			_gameInputActions.Gameplay.KillRandomEnemy.performed += OnKillRandomEnemyPerformed;

			EntityManager.CreateSingleton<CurrentGameInput>();
		}

		private void OnPlayerMovementCanceled(InputAction.CallbackContext _) {
			_currentMovementInputVector = Vector2.zero;
		}

		private void OnPlayerMovementPerformed(InputAction.CallbackContext callbackContext) {
			var currentMovementInputVector = callbackContext.ReadValue<Vector2>();
			_currentMovementInputVector = currentMovementInputVector;
		}

		protected override void OnUpdate() {
			SystemAPI.SetSingleton(new CurrentGameInput {
				CurrentMovementInputVector = _currentMovementInputVector,
				KillRandomEnemy = _killRandomEnemy,
			});

			_killRandomEnemy = false;
		}

		private void OnKillRandomEnemyPerformed(InputAction.CallbackContext _) {
			_killRandomEnemy = true;
		}
	}
}