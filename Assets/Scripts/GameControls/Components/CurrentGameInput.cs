using Unity.Entities;
using UnityEngine;

namespace PotatoFinch.TmgDotsJam.GameControls {
	public struct CurrentGameInput : IComponentData {
		public Vector2 CurrentMovementInputVector;
	}
}