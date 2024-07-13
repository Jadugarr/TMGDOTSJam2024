using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.Shop {
	public static class BoughtUpgradeBufferUtils {
		public static bool TryGetBoughtUpgrade(this DynamicBuffer<BoughtUpgrade> boughtUpgrades, UpgradeType upgradeType, out int boughtUpgradeIndex) {
			for (var i = 0; i < boughtUpgrades.Length; i++) {
				var upgrade = boughtUpgrades[i];
				if (upgrade.UpgradeType != upgradeType) continue;

				boughtUpgradeIndex = i;
				return true;
			}

			boughtUpgradeIndex = -1;
			return false;
		}
	}
}