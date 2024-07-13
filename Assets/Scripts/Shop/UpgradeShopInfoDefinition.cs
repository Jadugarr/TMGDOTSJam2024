using UnityEngine;

namespace PotatoFinch.TmgDotsJam.Shop {
	[CreateAssetMenu(fileName = "UpgradeShopInfoDefinition", menuName = "ScriptableObjects/UpgradeShopInfoDefinition", order = 0)]
	public class UpgradeShopInfoDefinition : ScriptableObject {
		[SerializeField] private UpgradeShopInfoEditorSetup _upgradeShopInfo;

		public UpgradeShopInfoEditorSetup UpgradeShopInfo => _upgradeShopInfo;
	}
}