using System;

namespace PotatoFinch.TmgDotsJam.Shop {
	[Serializable]
	public struct UpgradeShopInfoEditorSetup {
		public UpgradeType UpgradeType;
		public string Name;
		public string Description;
		public int Cost;
		public int MaxLevel;
	}
}