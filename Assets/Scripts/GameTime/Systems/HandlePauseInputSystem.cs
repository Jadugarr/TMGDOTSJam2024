using PotatoFinch.TmgDotsJam.GameControls;
using Unity.Burst;
using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.GameTime {
	[UpdateInGroup(typeof(GameTimeSystemGroup))]
	public partial struct HandlePauseInputSystem : ISystem {
		private EntityQuery _gamePausedQuery;
		
		public void OnCreate(ref SystemState state) {
			_gamePausedQuery = state.GetEntityQuery(typeof(GamePausedTag));
				
			state.RequireForUpdate<CurrentGameInput>();
		}

		public void OnUpdate(ref SystemState state) {
			var currentGameInput = SystemAPI.GetSingleton<CurrentGameInput>();

			if (!currentGameInput.TogglePause) {
				return;
			}

			if (_gamePausedQuery.CalculateEntityCount() > 0) {
				state.EntityManager.DestroyEntity(_gamePausedQuery);
				return;
			}

			state.EntityManager.CreateEntity(typeof(GamePausedTag));
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {

		}
	}
}