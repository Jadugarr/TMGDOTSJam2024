using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.GameCamera {
	public struct EntityToFocus : IComponentData {
		public Entity Value;
	}
}