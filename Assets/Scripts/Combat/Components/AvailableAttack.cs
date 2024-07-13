using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.Combat {
	public struct AvailableAttack : IBufferElementData {
		public AttackType AttackType;
		public float OriginalCooldown;
		public float Cooldown;
		public float CurrentCooldown;
		public float Range;
	}
}