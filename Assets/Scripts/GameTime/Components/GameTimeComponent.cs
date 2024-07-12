using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.GameTime {
	public struct GameTimeComponent : IComponentData {
		public float TotalTime;
		public float RemainingTime;
		public float DeltaTime;
	}
}