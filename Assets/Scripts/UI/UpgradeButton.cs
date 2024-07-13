using System;
using PotatoFinch.TmgDotsJam.Shop;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PotatoFinch.TmgDotsJam.UI {
	public class UpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
		[SerializeField] private Button _button;
		[SerializeField] private UpgradeShopInfoDefinition _upgradeShopInfoDefinition;
		[SerializeField] private TMP_Text _upgradeNameText;

		public event Action<UpgradeShopInfoDefinition> HoverStart;
		public event Action HoverEnd;
		public event Action<UpgradeShopInfoDefinition> ButtonClicked;

		private void Awake() {
			_button.onClick.AddListener(OnButtonClicked);

			_upgradeNameText.text = _upgradeShopInfoDefinition.UpgradeShopInfo.Name.ToString();
		}

		private void OnDestroy() {
			_button.onClick.RemoveListener(OnButtonClicked);
		}

		private void OnButtonClicked() {
			ButtonClicked?.Invoke(_upgradeShopInfoDefinition);
		}

		public void OnPointerEnter(PointerEventData eventData) {
			HoverStart?.Invoke(_upgradeShopInfoDefinition);
		}

		public void OnPointerExit(PointerEventData eventData) {
			HoverEnd?.Invoke();
		}
	}
}