using PotatoFinch.TmgDotsJam.GameTime;
using Unity.Burst;
using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.GameState {
	[UpdateInGroup(typeof(GameStateSystemGroup))]
	public partial struct ResetGameWhenOutOfTimeSystem : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<GameTimeComponent>();
		}

		public void OnUpdate(ref SystemState state) {
			var gameTimeComponent = SystemAPI.GetSingleton<GameTimeComponent>();

			if (gameTimeComponent.RemainingTime > 0f) {
				return;
			}

			state.EntityManager.CreateEntity(typeof(ResetGameTag));
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {

		}
	}
}