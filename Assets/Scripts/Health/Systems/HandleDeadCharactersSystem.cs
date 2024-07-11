using PotatoFinch.TmgDotsJam.Enemy;
using PotatoFinch.TmgDotsJam.Shop;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.Health {
	[UpdateInGroup(typeof(CharacterHealthSystemGroup))]
	public partial struct HandleDeadCharactersSystem : ISystem {
		private EntityQuery _deadCharactersQuery;
		private EntityQuery _spawnPointQuery;
		
		public void OnCreate(ref SystemState state) {
			_deadCharactersQuery = state.GetEntityQuery(typeof(CharacterDeadTag));
			_spawnPointQuery = state.GetEntityQuery(typeof(EnemySpawnPointId), typeof(EnemySpawnAmount));
			
			state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
			state.RequireForUpdate<OwnedGold>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			if (_deadCharactersQuery.CalculateEntityCount() <= 0) {
				return;
			}
			
			EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
			
			using var spawnPointEntities = _spawnPointQuery.ToEntityArray(Allocator.Temp);
			using var enemySpawnPointIds = _spawnPointQuery.ToComponentDataArray<EnemySpawnPointId>(Allocator.Temp);

			foreach (RefRO<EnemySpawnPointId> enemySpawnPointId in SystemAPI.Query<RefRO<EnemySpawnPointId>>().WithAll<CharacterDeadTag>()) {
				for (int i = 0; i < enemySpawnPointIds.Length; i++) {
					if (enemySpawnPointIds[i].Value != enemySpawnPointId.ValueRO.Value) {
						continue;
					}

					SystemAPI.GetComponentRW<EnemySpawnAmount>(spawnPointEntities[i]).ValueRW.CurrentValue -= 1;
				}
			}
			
			var ownedGoldSingleton = SystemAPI.GetSingletonRW<OwnedGold>();

			foreach (RefRO<GoldValue> goldValue in SystemAPI.Query<RefRO<GoldValue>>().WithAll<EnemyTag, CharacterDeadTag>()) {
				ownedGoldSingleton.ValueRW.Value += goldValue.ValueRO.Value;
			}

			ecb.DestroyEntity(_deadCharactersQuery, EntityQueryCaptureMode.AtRecord);
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {

		}
	}
}