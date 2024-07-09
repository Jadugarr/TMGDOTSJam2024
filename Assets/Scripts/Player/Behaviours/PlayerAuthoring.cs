﻿using PotatoFinch.TmgDotsJam.Health;
using PotatoFinch.TmgDotsJam.Movement;
using Unity.Entities;
using UnityEngine;
using MovementSpeed = PotatoFinch.TmgDotsJam.Movement.MovementSpeed;

namespace PotatoFinch.TmgDotsJam {
	public class PlayerAuthoring : MonoBehaviour {
		[SerializeField] private float _movementSpeed;
		[SerializeField] private float _maxHealth;

		private class PlayerAuthoringBaker : Baker<PlayerAuthoring> {
			public override void Bake(PlayerAuthoring authoring) {
				var playerEntity = GetEntity(authoring, TransformUsageFlags.Dynamic);
				AddComponent<PlayerTag>(playerEntity);
				AddComponent<Velocity>(playerEntity);
				AddComponent(playerEntity, new CharacterHealth { MaxHealth = authoring._maxHealth, CurrentHealth = authoring._maxHealth});
				AddComponent(playerEntity, new MovementSpeed { Value = authoring._movementSpeed });
			}
		}
	}
}