using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.Enemy {
	public struct EnemySpawnPointId : ICleanupComponentData {
		public int Value;
	}
}