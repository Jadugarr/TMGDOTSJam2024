using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PotatoFinch.TmgDotsJam.GameControls {
	public partial class HandleGameInputSystem : SystemBase {
		private EntityQuery _ignoreInputQuery;
		
		private GameInputActions _gameInputActions;

		private Vector2 _currentMovementInputVector;
		private bool _togglePause;
		private bool _openShop;

		protected override void OnCreate() {
			_ignoreInputQuery = EntityManager.CreateEntityQuery(typeof(IgnoreInputTag));
			
			_gameInputActions = new GameInputActions();
			_gameInputActions.Enable();

			_gameInputActions.Gameplay.PlayerMovement.performed += OnPlayerMovementPerformed;
			_gameInputActions.Gameplay.PlayerMovement.canceled += OnPlayerMovementCanceled;

			_gameInputActions.Gameplay.PauseGame.performed += OnPauseGamePerformed;

			_gameInputActions.Gameplay.OpenShop.performed += OnOpenShopPerformed;
			
			EntityManager.CreateSingleton<CurrentGameInput>();
		}

		private void OnOpenShopPerformed(InputAction.CallbackContext _) {
			_openShop = true;
		}

		private void OnPlayerMovementCanceled(InputAction.CallbackContext _) {
			_currentMovementInputVector = Vector2.zero;
		}

		private void OnPlayerMovementPerformed(InputAction.CallbackContext callbackContext) {
			var currentMovementInputVector = callbackContext.ReadValue<Vector2>();
			_currentMovementInputVector = currentMovementInputVector;
		}

		private void OnPauseGamePerformed(InputAction.CallbackContext _) {
			_togglePause = true;
		}

		protected override void OnUpdate() {
			if (_ignoreInputQuery.CalculateEntityCount() > 0) {
				SystemAPI.SetSingleton(new CurrentGameInput {
					CurrentMovementInputVector = Vector2.zero,
					TogglePause = false,
					OpenShop = false,
				});
			}
			else {
				SystemAPI.SetSingleton(new CurrentGameInput {
					CurrentMovementInputVector = _currentMovementInputVector,
					TogglePause = _togglePause,
					OpenShop = _openShop,
				});
			}

			_togglePause = false;
			_openShop = false;
		}
	}
}