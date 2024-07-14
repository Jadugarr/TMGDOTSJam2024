using PotatoFinch.TmgDotsJam.GameState;
using Unity.Burst;
using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.GameTime {
	[UpdateInGroup(typeof(GameTimeSystemGroup))]
	public partial struct ManageGameTimeSystem : ISystem {
		private EntityQuery _gamePausedQuery;
		private EntityArchetype _resetGameArchetype;
		
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
			var gameTimeEntity = state.EntityManager.CreateEntity(typeof(GameTimeComponent));
			SystemAPI.SetComponent(gameTimeEntity, new GameTimeComponent { TotalTime = 30f, RemainingTime = 30f });

			_gamePausedQuery = state.GetEntityQuery(typeof(GamePausedTag));
			_resetGameArchetype = state.EntityManager.CreateArchetype(typeof(ResetGameTag));

			state.RequireForUpdate<GameTimeComponent>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			var gameTimeComponent = SystemAPI.GetSingletonRW<GameTimeComponent>();

			if (gameTimeComponent.ValueRO.RemainingTime <= 0f) {
				SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).CreateEntity(_resetGameArchetype);
				return;
			}

			if (_gamePausedQuery.CalculateEntityCount() > 0) {
				gameTimeComponent.ValueRW.DeltaTime = 0f;
				return;
			}

			gameTimeComponent.ValueRW.RemainingTime -= SystemAPI.Time.DeltaTime;
			gameTimeComponent.ValueRW.DeltaTime = SystemAPI.Time.DeltaTime;
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
		}
	}
}