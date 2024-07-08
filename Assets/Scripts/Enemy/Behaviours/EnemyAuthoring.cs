using PotatoFinch.TmgDotsJam.Movement;
using Unity.Entities;
using UnityEngine;

namespace PotatoFinch.TmgDotsJam.Enemy {
	public class EnemyAuthoring : MonoBehaviour {
		[SerializeField] private float _movementSpeed;

		private class EnemyAuthoringBaker : Baker<EnemyAuthoring> {
			public override void Bake(EnemyAuthoring authoring) {
				var enemyEntity = GetEntity(authoring, TransformUsageFlags.Dynamic);
				AddComponent<EnemyTag>(enemyEntity);
				AddComponent<Velocity>(enemyEntity);
				AddComponent<EnemySpawnPointId>(enemyEntity);
				AddComponent(enemyEntity, new MovementSpeed { Value = authoring._movementSpeed });
			}
		}
	}
}