using UnityEngine;

namespace PotatoFinch.TmgDotsJam.UI {
	public class ShopDialogHandler : MonoBehaviour {
		[SerializeField] private GameObject _shopDialogObject;

		public void ActivateShopDialog(bool isActive) {
			_shopDialogObject.SetActive(isActive);
		}
	}
}