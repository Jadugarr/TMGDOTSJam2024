using Unity.Entities;
using Unity.Mathematics;

namespace PotatoFinch.TmgDotsJam.Movement {
	public struct Velocity : IComponentData {
		public float3 Value;
	}
}