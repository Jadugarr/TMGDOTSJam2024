using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.Shop {
	public struct BoughtUpgrade : IBufferElementData {
		public UpgradeType UpgradeType;
		public int CurrentLevel;
	}
}