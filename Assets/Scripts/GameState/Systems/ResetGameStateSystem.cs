using PotatoFinch.TmgDotsJam.Combat;
using PotatoFinch.TmgDotsJam.Enemy;
using PotatoFinch.TmgDotsJam.GameControls;
using PotatoFinch.TmgDotsJam.GameTime;
using PotatoFinch.TmgDotsJam.Health;
using PotatoFinch.TmgDotsJam.Movement;
using PotatoFinch.TmgDotsJam.Shop;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace PotatoFinch.TmgDotsJam.GameState {
	[UpdateInGroup(typeof(GameStateSystemGroup))]
	public partial struct ResetGameStateSystem : ISystem, ISystemStartStop {
		private EntityQuery _destroyOnResetQuery;
		private EntityQuery _resetGameQuery;
		private EntityQuery _ignoreInputQuery;
		private EntityQuery _wonGameQuery;
		private EntityQuery _pauseQuery;
		private EntityArchetype _forceRespawnEnemiesArchetype;

		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<OriginalEnemySpawnPointStats>();
			_destroyOnResetQuery = state.GetEntityQuery(typeof(DestroyOnLoopResetTag));
			_resetGameQuery = state.GetEntityQuery(typeof(ResetGameTag));
			_ignoreInputQuery = state.GetEntityQuery(typeof(IgnoreInputTag));
			_wonGameQuery = state.GetEntityQuery(typeof(WinGameTag));
			_pauseQuery = state.GetEntityQuery(typeof(GamePausedTag));
			_forceRespawnEnemiesArchetype = state.EntityManager.CreateArchetype(typeof(SpawnAllEnemiesTag));
			
			state.RequireForUpdate<OriginalPlayerStats>();
			state.RequireForUpdate<PlayerTag>();
			state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
			state.RequireForUpdate<GameTimeComponent>();
			state.RequireForUpdate<OwnedGold>();
			state.RequireForUpdate<ResetGameTag>();
		}

		public void OnStartRunning(ref SystemState state) {
			// Destroy all necessary objects
			var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
			ecb.DestroyEntity(_destroyOnResetQuery, EntityQueryCaptureMode.AtPlayback);

			// Reset player stats
			var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
			var playerOriginalStats = SystemAPI.GetSingleton<OriginalPlayerStats>();

			SystemAPI.GetComponentRW<LocalTransform>(playerEntity).ValueRW.Position = playerOriginalStats.OriginalPosition;
			SystemAPI.GetComponentRW<CharacterHealth>(playerEntity).ValueRW.MaxHealth = playerOriginalStats.MaxHealth;
			SystemAPI.GetComponentRW<CharacterHealth>(playerEntity).ValueRW.CurrentHealth = playerOriginalStats.MaxHealth;
			SystemAPI.GetComponentRW<MovementSpeed>(playerEntity).ValueRW.Value = playerOriginalStats.MovementSpeed;
			
			// Reset owned gold
			SystemAPI.GetSingletonRW<OwnedGold>().ValueRW.Value = 0;
			
			// Force respawn enemies
			ecb.CreateEntity(_forceRespawnEnemiesArchetype);
			
			// Reset bought upgrades
			SystemAPI.GetSingletonBuffer<BoughtUpgrade>().Clear();
			
			// Reset enemy spawn cooldowns
			var originalEnemySpawnPointStats = SystemAPI.GetSingleton<OriginalEnemySpawnPointStats>();

			foreach (RefRW<EnemySpawnCooldown> enemySpawnCooldown in SystemAPI.Query<RefRW<EnemySpawnCooldown>>()) {
				enemySpawnCooldown.ValueRW.CurrentCooldown = originalEnemySpawnPointStats.SpawnCooldown;
				enemySpawnCooldown.ValueRW.Cooldown = originalEnemySpawnPointStats.SpawnCooldown;
			}
			
			// Reset damage values
			foreach (RefRW<DamageValue> damageValue in SystemAPI.Query<RefRW<DamageValue>>().WithOptions(EntityQueryOptions.IncludePrefab)) {
				damageValue.ValueRW.Value = damageValue.ValueRO.OriginalValue;
			}
			
			// Reset attack speed
			var availableAttacks = SystemAPI.GetSingletonBuffer<AvailableAttack>();

			for (int i = 0; i < availableAttacks.Length; i++) {
				AvailableAttack availableAttack = availableAttacks[i];
				availableAttack.Cooldown = availableAttack.OriginalCooldown;
				availableAttack.CurrentCooldown = availableAttack.OriginalCooldown;
				availableAttacks[i] = availableAttack;
			}
			
			// Remove input lock
			ecb.DestroyEntity(_ignoreInputQuery, EntityQueryCaptureMode.AtRecord);
			
			// Remove won tag
			ecb.DestroyEntity(_wonGameQuery, EntityQueryCaptureMode.AtRecord);
			
			// Unpause
			ecb.DestroyEntity(_pauseQuery, EntityQueryCaptureMode.AtRecord);
			
			// Reset time
			var gameTimeComponent = SystemAPI.GetSingletonRW<GameTimeComponent>();
			gameTimeComponent.ValueRW.RemainingTime = gameTimeComponent.ValueRO.TotalTime;
			
			ecb.DestroyEntity(_resetGameQuery, EntityQueryCaptureMode.AtRecord);
		}

		public void OnStopRunning(ref SystemState state) {
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {

		}
	}
}