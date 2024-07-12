using PotatoFinch.TmgDotsJam.Enemy;
using PotatoFinch.TmgDotsJam.GameTime;
using PotatoFinch.TmgDotsJam.Shop;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace PotatoFinch.TmgDotsJam.GameState {
	[UpdateInGroup(typeof(GameStateSystemGroup))]
	public partial struct ResetGameStateSystem : ISystem {
		private EntityQuery _destroyOnResetQuery;
		private EntityArchetype _forceRespawnEnemiesArchetype;

		public void OnCreate(ref SystemState state) {
			_destroyOnResetQuery = state.GetEntityQuery(typeof(DestroyOnLoopResetTag));
			_forceRespawnEnemiesArchetype = state.EntityManager.CreateArchetype(typeof(SpawnAllEnemiesTag));
			
			state.RequireForUpdate<PlayerOriginPosition>();
			state.RequireForUpdate<PlayerTag>();
			state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
			state.RequireForUpdate<GameTimeComponent>();
			state.RequireForUpdate<OwnedGold>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			var gameTimeComponent = SystemAPI.GetSingleton<GameTimeComponent>();

			if (gameTimeComponent.RemainingTime > 0f) {
				return;
			}

			// Destroy all necessary objects
			var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
			ecb.DestroyEntity(_destroyOnResetQuery, EntityQueryCaptureMode.AtPlayback);

			// Reset player position
			var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
			var playerOriginPosition = SystemAPI.GetSingleton<PlayerOriginPosition>();

			SystemAPI.GetComponentRW<LocalTransform>(playerEntity).ValueRW.Position = playerOriginPosition.Value;
			
			// Reset owned gold
			SystemAPI.GetSingletonRW<OwnedGold>().ValueRW.Value = 0;
			
			// Force respawn enemies
			ecb.CreateEntity(_forceRespawnEnemiesArchetype);
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {

		}
	}
}