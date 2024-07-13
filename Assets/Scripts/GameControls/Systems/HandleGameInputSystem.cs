using PotatoFinch.TmgDotsJam.Shop;
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

			_gameInputActions.Gameplay.BuyUpgradeTest.performed += OnBuyUpgradeTestPerformed;
			_gameInputActions.Gameplay.BuyUpgradeTest2.performed += OnBuyUpgradeTest2Performed;
			_gameInputActions.Gameplay.BuyUpgradeTest3.performed += OnBuyUpgradeTest3Performed;
			_gameInputActions.Gameplay.BuyUpgradeTest4.performed += OnBuyUpgradeTest4Performed;
			_gameInputActions.Gameplay.BuyUpgradeTest5.performed += OnBuyUpgradeTest5Performed;
			_gameInputActions.Gameplay.BuyUpgradeTest6.performed += OnBuyUpgradeTest6Performed;

			EntityManager.CreateSingleton<CurrentGameInput>();
		}

		private void OnBuyUpgradeTestPerformed(InputAction.CallbackContext _) {
			var entity = EntityManager.CreateEntity(typeof(BuyUpgrade));
			EntityManager.SetComponentData(entity, new BuyUpgrade { Value = UpgradeType.MovementSpeed });
		}

		private void OnBuyUpgradeTest2Performed(InputAction.CallbackContext _) {
			var entity = EntityManager.CreateEntity(typeof(BuyUpgrade));
			EntityManager.SetComponentData(entity, new BuyUpgrade { Value = UpgradeType.TimerReset });
		}

		private void OnBuyUpgradeTest3Performed(InputAction.CallbackContext _) {
			var entity = EntityManager.CreateEntity(typeof(BuyUpgrade));
			EntityManager.SetComponentData(entity, new BuyUpgrade { Value = UpgradeType.EnemySpawnCooldown });
		}

		private void OnBuyUpgradeTest4Performed(InputAction.CallbackContext _) {
			var entity = EntityManager.CreateEntity(typeof(BuyUpgrade));
			EntityManager.SetComponentData(entity, new BuyUpgrade { Value = UpgradeType.Damage });
		}

		private void OnBuyUpgradeTest5Performed(InputAction.CallbackContext _) {
			var entity = EntityManager.CreateEntity(typeof(BuyUpgrade));
			EntityManager.SetComponentData(entity, new BuyUpgrade { Value = UpgradeType.AttackSpeed });
		}

		private void OnBuyUpgradeTest6Performed(InputAction.CallbackContext _) {
			var entity = EntityManager.CreateEntity(typeof(BuyUpgrade));
			EntityManager.SetComponentData(entity, new BuyUpgrade { Value = UpgradeType.EnemyRespawn });
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