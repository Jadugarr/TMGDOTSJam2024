using PotatoFinch.TmgDotsJam.Combat.Behaviours;
using PotatoFinch.TmgDotsJam.Enemy;
using Unity.Entities;
using UnityEngine;

namespace PotatoFinch.TmgDotsJam.Common {
	public class GamePrefabAuthoring : MonoBehaviour {
		[SerializeField] private EnemyAuthoring _smallEnemyAuthoring;
		[SerializeField] private EnemyAuthoring _mediumEnemyAuthoring;
		[SerializeField] private EnemyAuthoring _largeEnemyAuthoring;
		[SerializeField] private ProjectileAuthoring _bulletAuthoring;

		private class GamePrefabAuthoringBaker : Baker<GamePrefabAuthoring> {
			public override void Bake(GamePrefabAuthoring authoring) {
				var prefabContainerEntity = GetEntity(authoring, TransformUsageFlags.None);

				AddComponent(prefabContainerEntity, new PrefabContainer {
					SmallEnemyPrefab = GetEntity(authoring._smallEnemyAuthoring, TransformUsageFlags.Dynamic),
					MediumEnemyPrefab = GetEntity(authoring._mediumEnemyAuthoring, TransformUsageFlags.Dynamic),
					LargeEnemyPrefab = GetEntity(authoring._largeEnemyAuthoring, TransformUsageFlags.Dynamic),
					BulletPrefab = GetEntity(authoring._bulletAuthoring, TransformUsageFlags.Dynamic),
				});
			}
		}
	}
}