using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.Enemy {
	public struct EnemySpawnCooldown : IComponentData {
		public float Cooldown;
		public float CurrentCooldown;
	}
}