using Unity.Entities;
using Unity.Mathematics;

namespace PotatoFinch.TmgDotsJam {
	public struct PlayerOriginPosition : IComponentData {
		public float3 Value;
	}
}