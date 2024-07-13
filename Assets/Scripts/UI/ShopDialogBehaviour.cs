using System.Globalization;
using PotatoFinch.TmgDotsJam.GameTime;
using PotatoFinch.TmgDotsJam.Shop;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace PotatoFinch.TmgDotsJam.UI {
	public class ShopDialogBehaviour : MonoBehaviour {
		[SerializeField, Header("Buttons")] private Button _closeButton;
		[SerializeField] private UpgradeButton[] _upgradeButtons;

		[SerializeField, Header("Textfields")] private TMP_Text _descriptionText;
		[SerializeField] private TMP_Text _costText;

		[SerializeField, Header("Containers")] private GameObject _costContainer;

		private EntityQuery _boughtUpgradesQuery;
		private EntityQuery _gamePausedQuery;
		private EntityQuery _ownedGoldQuery;
		private EntityManager _entityManager;

		private void Awake() {
			_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			_gamePausedQuery = _entityManager.CreateEntityQuery(typeof(GamePausedTag));
			_ownedGoldQuery = _entityManager.CreateEntityQuery(typeof(OwnedGold));
			_boughtUpgradesQuery = _entityManager.CreateEntityQuery(typeof(BoughtUpgrade));

			foreach (UpgradeButton upgradeButton in _upgradeButtons) {
				upgradeButton.HoverStart += OnHoverStart;
				upgradeButton.HoverEnd += OnHoverEnd;
				upgradeButton.ButtonClicked += OnButtonClicked;
			}

			_closeButton.onClick.AddListener(OnCloseButtonClicked);

			_descriptionText.text = string.Empty;
			_costContainer.SetActive(false);
		}

		private void OnDestroy() {
			_closeButton.onClick.RemoveListener(OnCloseButtonClicked);
		}

		private void OnCloseButtonClicked() {
			gameObject.SetActive(false);
			if (_gamePausedQuery.CalculateEntityCount() > 0) {
				_entityManager.DestroyEntity(_gamePausedQuery);
			}
		}

		private void OnButtonClicked(UpgradeShopInfoDefinition upgradeShopInfoDefinition) {
			int level = GetNextLevelOfUpgrade(upgradeShopInfoDefinition);
			var cost = upgradeShopInfoDefinition.UpgradeShopInfo.Cost * level;

			if (!_ownedGoldQuery.TryGetSingleton(out OwnedGold ownedGold)) {
				Debug.LogError("No OwnedGold component found!");
				return;
			}

			if (cost > ownedGold.Value) {
				return;
			}

			var buyUpgradeEntity = _entityManager.CreateEntity(typeof(BuyUpgrade));
			_entityManager.SetComponentData(buyUpgradeEntity, new BuyUpgrade { Value = upgradeShopInfoDefinition.UpgradeShopInfo.UpgradeType });
			UpdateCost(upgradeShopInfoDefinition, level + 1);
		}

		private int GetNextLevelOfUpgrade(UpgradeShopInfoDefinition upgradeShopInfoDefinition) {
			if (_boughtUpgradesQuery.TryGetSingletonBuffer(out DynamicBuffer<BoughtUpgrade> boughtUpgrades, true) && boughtUpgrades.TryGetBoughtUpgrade(upgradeShopInfoDefinition.UpgradeShopInfo.UpgradeType, out int boughtUpgradeIndex)) {
				return boughtUpgrades[boughtUpgradeIndex].CurrentLevel + 1;
			}

			return 1;
		}

		private void OnHoverStart(UpgradeShopInfoDefinition upgradeShopInfoDefinition) {
			_descriptionText.text = upgradeShopInfoDefinition.UpgradeShopInfo.Description;
			_costContainer.SetActive(true);

			UpdateCost(upgradeShopInfoDefinition, GetNextLevelOfUpgrade(upgradeShopInfoDefinition));
		}

		private void UpdateCost(UpgradeShopInfoDefinition shopInfoDefinition, int desiredLevel) {
			if (desiredLevel > shopInfoDefinition.UpgradeShopInfo.MaxLevel) {
				_costText.text = "MAX";
				return;
			}

			var cost = shopInfoDefinition.UpgradeShopInfo.Cost * desiredLevel;
			_costText.text = cost.ToString(CultureInfo.InvariantCulture);
		}

		private void OnHoverEnd() {
			_costContainer.SetActive(false);
			_descriptionText.text = string.Empty;
		}
	}
}