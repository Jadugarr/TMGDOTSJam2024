using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.Combat {
	public struct DamageCooldown : IComponentData {
		public float Value;
	}
}