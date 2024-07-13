using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.Combat {
	public struct DamageValue : IComponentData {
		public float OriginalValue;
		public float Value;
	}
}