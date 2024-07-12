using PotatoFinch.TmgDotsJam.GameTime;
using Unity.Burst;
using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.GameState {
	[UpdateInGroup(typeof(GameStateSystemGroup))]
	public partial struct ResetGameStateSystem : ISystem {
		private EntityQuery _destroyOnResetQuery;
		
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
			_destroyOnResetQuery = state.GetEntityQuery(typeof(DestroyOnLoopResetTag));
			
			state.RequireForUpdate<GameTimeComponent>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			var gameTimeComponent = SystemAPI.GetSingleton<GameTimeComponent>();

			if (gameTimeComponent.RemainingTime > 0f) {
				return;
			}

			var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
			ecb.DestroyEntity(_destroyOnResetQuery, EntityQueryCaptureMode.AtPlayback);
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {

		}
	}
}