using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PotatoFinch.TmgDotsJam.GameControls {
	public partial class HandleGameInputSystem : SystemBase {
		private GameInputActions _gameInputActions;

		private Vector2 _currentMovementInputVector;
		private bool _killRandomEnemy;
		private bool _togglePause;

		protected override void OnCreate() {
			_gameInputActions = new GameInputActions();
			_gameInputActions.Enable();
			
			_gameInputActions.Gameplay.PlayerMovement.performed += OnPlayerMovementPerformed;
			_gameInputActions.Gameplay.PlayerMovement.canceled += OnPlayerMovementCanceled;
			
			_gameInputActions.Gameplay.KillRandomEnemy.performed += OnKillRandomEnemyPerformed;
			
			_gameInputActions.Gameplay.PauseGame.performed += OnPauseGamePerformed;

			EntityManager.CreateSingleton<CurrentGameInput>();
		}

		private void OnPlayerMovementCanceled(InputAction.CallbackContext _) {
			_currentMovementInputVector = Vector2.zero;
		}

		private void OnPlayerMovementPerformed(InputAction.CallbackContext callbackContext) {
			var currentMovementInputVector = callbackContext.ReadValue<Vector2>();
			_currentMovementInputVector = currentMovementInputVector;
		}
		private void OnKillRandomEnemyPerformed(InputAction.CallbackContext _) {
			_killRandomEnemy = true;
		}

		private void OnPauseGamePerformed(InputAction.CallbackContext _) {
			_togglePause = true;
		}

		protected override void OnUpdate() {
			SystemAPI.SetSingleton(new CurrentGameInput {
				CurrentMovementInputVector = _currentMovementInputVector,
				KillRandomEnemy = _killRandomEnemy,
				TogglePause = _togglePause,
			});

			_killRandomEnemy = false;
			_togglePause = false;
		}
	}
}