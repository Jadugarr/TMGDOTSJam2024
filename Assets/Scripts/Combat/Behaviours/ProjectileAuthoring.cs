using Unity.Entities;
using UnityEngine;

namespace PotatoFinch.TmgDotsJam.Combat.Behaviours {
	public class ProjectileAuthoring : MonoBehaviour {
		[SerializeField] private float _damageValue;

		private class ProjectileAuthoringBaker : Baker<ProjectileAuthoring> {
			public override void Bake(ProjectileAuthoring authoring) {
				var projectileEntity = GetEntity(authoring, TransformUsageFlags.Dynamic);
				AddComponent(projectileEntity, new DamageValue { Value = authoring._damageValue });
			}
		}
	}
}