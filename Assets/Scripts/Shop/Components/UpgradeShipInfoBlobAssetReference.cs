using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.Shop {
	public struct UpgradeShipInfoBlobAssetReference : IComponentData {
		public BlobAssetReference<UpgradeShopInfos> Value;

		public bool TryGetUpgradeShopInfoByUpgradeType(UpgradeType upgradeType, out UpgradeShopInfo upgradeShopInfo) {
			ref var upgradeShopInfos = ref Value.Value.Value;
			
			for (var i = 0; i < upgradeShopInfos.Length; i++) {
				var currentUpgradeShopInfo = upgradeShopInfos[i];

				if (currentUpgradeShopInfo.UpgradeType != upgradeType) continue;
				
				upgradeShopInfo = currentUpgradeShopInfo;
				return true;
			}

			upgradeShopInfo = default;
			return false;
		}
	}
}