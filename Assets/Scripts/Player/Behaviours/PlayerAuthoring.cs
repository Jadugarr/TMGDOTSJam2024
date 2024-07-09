using PotatoFinch.TmgDotsJam.Combat;
using PotatoFinch.TmgDotsJam.Health;
using PotatoFinch.TmgDotsJam.Movement;
using Unity.Entities;
using UnityEngine;

namespace PotatoFinch.TmgDotsJam {
	public class PlayerAuthoring : MonoBehaviour {
		[SerializeField] private float _movementSpeed;
		[SerializeField] private float _maxHealth;
		[SerializeField] private float _attackRange;

		private class PlayerAuthoringBaker : Baker<PlayerAuthoring> {
			public override void Bake(PlayerAuthoring authoring) {
				var playerEntity = GetEntity(authoring, TransformUsageFlags.Dynamic);
				AddComponent<PlayerTag>(playerEntity);
				AddComponent<Velocity>(playerEntity);
				AddComponent(playerEntity, new CharacterHealth { MaxHealth = authoring._maxHealth, CurrentHealth = authoring._maxHealth });
				AddComponent(playerEntity, new MovementSpeed { Value = authoring._movementSpeed });
				AddComponent(playerEntity, new AttackRange { Value = authoring._attackRange });
				AddBuffer<AvailableAttack>(playerEntity);
				AppendToBuffer(playerEntity, new AvailableAttack { AttackType = AttackType.Bullet, Cooldown = 2f, Range = 5f });
			}
		}
	}
}