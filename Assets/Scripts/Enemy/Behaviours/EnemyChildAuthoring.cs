using PotatoFinch.TmgDotsJam.GameState;
using Unity.Entities;
using UnityEngine;

namespace PotatoFinch.TmgDotsJam.Enemy {
	public class EnemyChildAuthoring : MonoBehaviour {
		private class EnemyChildAuthoringBaker : Baker<EnemyChildAuthoring> {
			public override void Bake(EnemyChildAuthoring authoring) {
				var childEntity = GetEntity(authoring, TransformUsageFlags.Dynamic);
				AddComponent<DestroyOnLoopResetTag>(childEntity);
			}
		}
	}
}