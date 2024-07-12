using System;
using Unity.Collections;

namespace PotatoFinch.TmgDotsJam.Shop {
	[Serializable]
	public struct UpgradeShopInfo {
		public UpgradeType UpgradeType;
		public FixedString64Bytes Name;
		public FixedString64Bytes Description;
		public int Cost;
		public int MaxLevel;
	}
}