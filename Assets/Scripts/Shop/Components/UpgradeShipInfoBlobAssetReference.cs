using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.Shop {
	public struct UpgradeShipInfoBlobAssetReference : IComponentData {
		public BlobAssetReference<UpgradeShopInfos> Value;
	}
}