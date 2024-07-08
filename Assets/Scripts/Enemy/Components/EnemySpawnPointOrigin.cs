using Unity.Entities;
using Unity.Mathematics;

namespace PotatoFinch.TmgDotsJam.Enemy {
	public struct EnemySpawnPointOrigin : IComponentData {
		public float3 Value;
	}
}