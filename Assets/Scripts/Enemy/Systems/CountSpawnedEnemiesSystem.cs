using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.Enemy {
	[UpdateInGroup(typeof(EnemySystemGroup), OrderFirst = true)]
	public partial struct CountSpawnedEnemiesSystem : ISystem {
		private EntityQuery _enemyQuery;
		
		public void OnCreate(ref SystemState state) {
			_enemyQuery = state.GetEntityQuery(typeof(EnemyTag), typeof(EnemySpawnPointId));
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			using var enemySpawnPointIds = _enemyQuery.ToComponentDataArray<EnemySpawnPointId>(Allocator.Temp);

			foreach ((RefRO<EnemySpawnPointId> spawnPointId, RefRW<EnemySpawnAmount> spawnAmount) in SystemAPI.Query<RefRO<EnemySpawnPointId>, RefRW<EnemySpawnAmount>>()) {
				int enemyAmount = 0;
				
				foreach (EnemySpawnPointId spawnedEnemySpawnPointId in enemySpawnPointIds) {
					if (spawnedEnemySpawnPointId.Value == spawnPointId.ValueRO.Value) {
						enemyAmount++;
					}
				}

				spawnAmount.ValueRW.CurrentValue = enemyAmount;
			}
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {

		}
	}
}