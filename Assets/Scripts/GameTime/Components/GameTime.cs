using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.GameTime {
	public struct GameTime : IComponentData {
		public float TotalTime;
		public float RemainingTime;
		public float DeltaTime;
	}
}