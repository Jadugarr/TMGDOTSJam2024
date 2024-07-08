using PotatoFinch.TmgDotsJam.Common;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace PotatoFinch.TmgDotsJam.Enemy {
	[UpdateInGroup(typeof(EnemySystemGroup))]
	public partial struct SpawnEnemySystem : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
			state.RequireForUpdate<PrefabContainer>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			var prefabContainer = SystemAPI.GetSingleton<PrefabContainer>();
			var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

			foreach ((RefRO<EnemySpawnPointId> pointId, RefRO<EnemySpawnPointOrigin> pointOrigin, RefRO<EnemySpawnPointRange> pointRange, RefRW<EnemySpawnAmount> spawnAmount)
			         in SystemAPI.Query<RefRO<EnemySpawnPointId>, RefRO<EnemySpawnPointOrigin>, RefRO<EnemySpawnPointRange>, RefRW<EnemySpawnAmount>>()) {
				int amountToSpawn = spawnAmount.ValueRO.MaxValue - spawnAmount.ValueRO.CurrentValue;

				if (amountToSpawn <= 0) {
					continue;
				}
				
				using NativeArray<Entity> spawnedEntities = new NativeArray<Entity>(spawnAmount.ValueRO.MaxValue, Allocator.Temp);
				ecb.Instantiate(prefabContainer.SmallEnemyPrefab, spawnedEntities);

				foreach (Entity spawnedEntity in spawnedEntities) {
					ecb.SetComponent(spawnedEntity, new LocalTransform { Position = pointOrigin.ValueRO.Value, Rotation = quaternion.identity, Scale = 1f });
				}

				spawnAmount.ValueRW.CurrentValue += amountToSpawn;
			}
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
		}
	}
}