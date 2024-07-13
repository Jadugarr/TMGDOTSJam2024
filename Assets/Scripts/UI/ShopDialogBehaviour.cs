using System.Globalization;
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
		private EntityManager _entityManager;

		private void Awake() {
			_entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			_boughtUpgradesQuery = _entityManager.CreateEntityQuery(typeof(BoughtUpgrade));

			foreach (UpgradeButton upgradeButton in _upgradeButtons) {
				upgradeButton.HoverStart += OnHoverStart;
				upgradeButton.HoverEnd += OnHoverEnd;
				upgradeButton.ButtonClicked += OnButtonClicked;
			}

			_descriptionText.text = string.Empty;
			_costContainer.SetActive(false);
		}

		private void OnButtonClicked(UpgradeShopInfoDefinition upgradeShopInfoDefinition) {
			var buyUpgradeEntity = _entityManager.CreateEntity(typeof(BuyUpgrade));
			_entityManager.SetComponentData(buyUpgradeEntity, new BuyUpgrade { Value = upgradeShopInfoDefinition.UpgradeShopInfo.UpgradeType });
		}

		private void OnHoverStart(UpgradeShopInfoDefinition upgradeShopInfoDefinition) {
			_descriptionText.text = upgradeShopInfoDefinition.UpgradeShopInfo.Description;
			_costContainer.SetActive(true);

			if (_boughtUpgradesQuery.TryGetSingletonBuffer(out DynamicBuffer<BoughtUpgrade> boughtUpgrades, true) && boughtUpgrades.TryGetBoughtUpgrade(upgradeShopInfoDefinition.UpgradeShopInfo.UpgradeType, out int boughtUpgradeIndex)) {
				var cost = upgradeShopInfoDefinition.UpgradeShopInfo.Cost * (boughtUpgrades[boughtUpgradeIndex].CurrentLevel + 1);
				_costText.text = cost.ToString(CultureInfo.InvariantCulture);
			}
			else {
				_costText.text = upgradeShopInfoDefinition.UpgradeShopInfo.Cost.ToString(CultureInfo.InvariantCulture);
			}
		}

		private void OnHoverEnd() {
			_costContainer.SetActive(false);
			_descriptionText.text = string.Empty;
		}
	}
}