using Unity.Entities;
using Unity.Mathematics;

namespace PotatoFinch.TmgDotsJam.GameControls {
	public struct CurrentGameInput : IComponentData {
		public float2 CurrentMovementInputVector;
		public bool TogglePause;
		public bool OpenShop;
	}
}