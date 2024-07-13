using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.Shop {
	public struct BuyUpgrade : IComponentData {
		public UpgradeType Value;
	}
}