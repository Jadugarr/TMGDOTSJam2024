using System;
using PotatoFinch.TmgDotsJam.Common;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

namespace PotatoFinch.TmgDotsJam.Enemy {
	[UpdateInGroup(typeof(EnemySystemGroup))]
	public partial struct SpawnAllEnemiesSystem : ISystem {
		private Random _random;
		private EntityQuery _enemyPositionQuery;
		private EntityQuery _enemySpawnPointQuery;
		private EntityQuery _spawnAllEnemiesQuery;

		public void OnCreate(ref SystemState state) {
			_random = new Random(1 + (uint)DateTime.UtcNow.Millisecond);
			_enemyPositionQuery = state.GetEntityQuery(typeof(EnemyTag), typeof(LocalTransform));
			_spawnAllEnemiesQuery = state.GetEntityQuery(typeof(SpawnAllEnemiesTag));

			state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
			state.RequireForUpdate<PrefabContainer>();
			state.RequireForUpdate<SpawnAllEnemiesTag>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			var prefabContainer = SystemAPI.GetSingleton<PrefabContainer>();
			var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
			using var enemyPositions = _enemyPositionQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);

			foreach ((RefRO<EnemySpawnPointId> pointId, RefRO<EnemySpawnPointOrigin> pointOrigin, RefRO<EnemySpawnPointRange> pointRange, RefRW<EnemySpawnAmount> spawnAmount)
			         in SystemAPI.Query<RefRO<EnemySpawnPointId>, RefRO<EnemySpawnPointOrigin>, RefRO<EnemySpawnPointRange>, RefRW<EnemySpawnAmount>>()) {
				int amountToSpawn = spawnAmount.ValueRO.MaxValue - spawnAmount.ValueRO.CurrentValue;

				if (amountToSpawn <= 0) {
					continue;
				}

				using NativeArray<Entity> spawnedEntities = new NativeArray<Entity>(amountToSpawn, Allocator.Temp);
				using NativeArray<float3> spawnedEnemyPositions = new NativeArray<float3>(amountToSpawn, Allocator.Temp);

				ecb.Instantiate(prefabContainer.SmallEnemyPrefab, spawnedEntities);

				for (var index = 0; index < spawnedEntities.Length; index++) {
					var spawnedEntity = spawnedEntities[index];
					float3 spawnPosition;
					float3 spawnDirection = new float3(1f, 0f, 0f);

					bool isValidPosition = false;
					int currentTries = 0;

					var nativeArray = spawnedEnemyPositions;

					do {
						currentTries++;
						spawnDirection = math.mul(quaternion.RotateY(_random.NextFloat(359f)), spawnDirection);
						float spawnDistance = _random.NextFloat(pointRange.ValueRO.Value);
						spawnPosition = pointOrigin.ValueRO.Value + spawnDirection * spawnDistance;

						bool foundError = false;
						foreach (LocalTransform enemyPosition in enemyPositions) {
							if (math.distance(enemyPosition.Position, spawnPosition) <= 2f) {
								foundError = true;
								break;
							}
						}

						if (!foundError) {
							foreach (float3 spawnedEnemyPosition in nativeArray) {
								if (math.distance(spawnedEnemyPosition, spawnPosition) <= 2f) {
									foundError = true;
									break;
								}
							}

							if (!foundError) {
								isValidPosition = true;
							}
						}
					} while (!isValidPosition && currentTries < 10);

					nativeArray[index] = spawnPosition;
					ecb.SetComponent(spawnedEntity, new LocalTransform { Position = spawnPosition, Rotation = quaternion.identity, Scale = 1f });
					ecb.SetComponent(spawnedEntity, pointId.ValueRO);
				}
			}

			ecb.DestroyEntity(_spawnAllEnemiesQuery, EntityQueryCaptureMode.AtRecord);
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
		}
	}
}