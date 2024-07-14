using PotatoFinch.TmgDotsJam.Combat;
using PotatoFinch.TmgDotsJam.GameState;
using PotatoFinch.TmgDotsJam.Health;
using PotatoFinch.TmgDotsJam.Movement;
using Unity.Entities;
using UnityEngine;

namespace PotatoFinch.TmgDotsJam.Enemy {
	public class EnemyAuthoring : MonoBehaviour {
		[SerializeField] private float _movementSpeed;
		[SerializeField] private float _maxHealth;
		[SerializeField] private float _aggroRange;
		[SerializeField] private int _goldValue;

		private class EnemyAuthoringBaker : Baker<EnemyAuthoring> {
			public override void Bake(EnemyAuthoring authoring) {
				var enemyEntity = GetEntity(authoring, TransformUsageFlags.Dynamic);
				AddComponent<EnemyTag>(enemyEntity);
				AddComponent<Velocity>(enemyEntity);
				AddComponent<EnemySpawnPointId>(enemyEntity);
				AddComponent<DestroyOnLoopResetTag>(enemyEntity);
				AddComponent<ColorLerpMaterialOverride>(enemyEntity);
				AddComponent(enemyEntity, new CharacterHealth { MaxHealth = authoring._maxHealth, CurrentHealth = authoring._maxHealth});
				AddComponent(enemyEntity, new MovementSpeed { Value = authoring._movementSpeed });
				AddComponent(enemyEntity, new AggroRange { Value = authoring._aggroRange });
				AddComponent(enemyEntity, new GoldValue{Value = authoring._goldValue});
			}
		}
	}
}