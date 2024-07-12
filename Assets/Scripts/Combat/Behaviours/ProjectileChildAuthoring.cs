using PotatoFinch.TmgDotsJam.GameState;
using Unity.Entities;
using UnityEngine;

namespace PotatoFinch.TmgDotsJam.Combat.Behaviours {
	public class ProjectileChildAuthoring : MonoBehaviour {
		private class ProjectileChildAuthoringBaker : Baker<ProjectileChildAuthoring> {
			public override void Bake(ProjectileChildAuthoring authoring) {
				var childEntity = GetEntity(authoring, TransformUsageFlags.Dynamic);
				AddComponent<DestroyOnLoopResetTag>(childEntity);
			}
		}
	}
}