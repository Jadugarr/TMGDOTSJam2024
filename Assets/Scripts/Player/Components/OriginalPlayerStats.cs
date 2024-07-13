using Unity.Entities;
using Unity.Mathematics;

namespace PotatoFinch.TmgDotsJam {
	public struct OriginalPlayerStats : IComponentData {
		public float3 OriginalPosition;
		public float MovementSpeed;
		public float MaxHealth;
	}
}