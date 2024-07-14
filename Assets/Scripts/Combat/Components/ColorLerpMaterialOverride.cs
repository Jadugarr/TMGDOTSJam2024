using Unity.Entities;
using Unity.Rendering;

namespace PotatoFinch.TmgDotsJam.Combat {
	[MaterialProperty("_ColorLerp")]
	public struct ColorLerpMaterialOverride : IComponentData {
		public float Value;
	}
}