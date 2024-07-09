using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.Common {
	public struct PrefabContainer : IComponentData {
		public Entity SmallEnemyPrefab;
		public Entity MediumEnemyPrefab;
		public Entity LargeEnemyPrefab;
		public Entity BulletPrefab;
	}
}