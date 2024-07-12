using PotatoFinch.TmgDotsJam.GameState;
using PotatoFinch.TmgDotsJam.Movement;
using Unity.Entities;
using UnityEngine;

namespace PotatoFinch.TmgDotsJam.Combat.Behaviours {
	public class ProjectileAuthoring : MonoBehaviour {
		[SerializeField] private float _damageValue;
		[SerializeField] private float _movementSpeed;

		private class ProjectileAuthoringBaker : Baker<ProjectileAuthoring> {
			public override void Bake(ProjectileAuthoring authoring) {
				var projectileEntity = GetEntity(authoring, TransformUsageFlags.Dynamic);
				AddComponent(projectileEntity, new DamageValue { Value = authoring._damageValue });
				AddComponent(projectileEntity, new MovementSpeed { Value = authoring._movementSpeed });
				AddComponent<Velocity>(projectileEntity);
				AddComponent<TargetEnemy>(projectileEntity);
				AddComponent<ProjectileTag>(projectileEntity);
				AddComponent<DestroyOnLoopResetTag>(projectileEntity);
			}
		}
	}
}