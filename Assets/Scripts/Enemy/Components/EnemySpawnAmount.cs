using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.Enemy {
	public struct EnemySpawnAmount : IComponentData {
		public int MaxValue;
		public int CurrentValue;
	}
}