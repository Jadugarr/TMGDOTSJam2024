using System;
using PotatoFinch.TmgDotsJam.Common;
using PotatoFinch.TmgDotsJam.GameTime;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

namespace PotatoFinch.TmgDotsJam.Enemy {
	[UpdateInGroup(typeof(EnemySystemGroup))]
	public partial struct SpawnEnemySystem : ISystem {
		private Random _random;
		private EntityQuery _enemyPositionQuery;
		private EntityQuery _enemySpawnPointQuery;

		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<PlayerTag>();
			_random = new Random(1 + (uint)DateTime.UtcNow.Millisecond);
			_enemyPositionQuery = state.GetEntityQuery(typeof(EnemyTag), typeof(LocalTransform));

			state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
			state.RequireForUpdate<PrefabContainer>();
			state.RequireForUpdate<GameTimeComponent>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			var prefabContainer = SystemAPI.GetSingleton<PrefabContainer>();
			var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
			using var enemyPositions = _enemyPositionQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);
			var playerPosition = SystemAPI.GetComponentRO<LocalTransform>(SystemAPI.GetSingletonEntity<PlayerTag>());
			var gameTimeComponent = SystemAPI.GetSingleton<GameTimeComponent>();

			foreach ((RefRW<EnemySpawnCooldown> spawnCooldown, RefRO<EnemySpawnAmount> spawnAmount) in SystemAPI.Query<RefRW<EnemySpawnCooldown>, RefRO<EnemySpawnAmount>>()) {
				if (spawnAmount.ValueRO.CurrentValue >= spawnAmount.ValueRO.MaxValue) {
					spawnCooldown.ValueRW.CurrentCooldown = spawnCooldown.ValueRO.Cooldown;
					continue;
				}

				spawnCooldown.ValueRW.CurrentCooldown -= gameTimeComponent.DeltaTime;
			}

			foreach ((RefRO<EnemySpawnPointId> pointId, RefRO<EnemySpawnPointOrigin> pointOrigin, RefRO<EnemySpawnPointRange> pointRange, RefRW<EnemySpawnAmount> spawnAmount, RefRW<EnemySpawnCooldown> spawnCooldown)
			         in SystemAPI.Query<RefRO<EnemySpawnPointId>, RefRO<EnemySpawnPointOrigin>, RefRO<EnemySpawnPointRange>, RefRW<EnemySpawnAmount>, RefRW<EnemySpawnCooldown>>()) {
				if (spawnCooldown.ValueRO.CurrentCooldown > 0f) {
					continue;
				}

				var spawnedEntity = ecb.Instantiate(prefabContainer.SmallEnemyPrefab);

				float3 spawnPosition;
				float3 spawnDirection = new float3(1f, 0f, 0f);

				bool isValidPosition = false;
				int currentTries = 0;

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
						
						if (math.distance(playerPosition.ValueRO.Position, spawnPosition) <= 5f) {
							foundError = true;
							break;
						}
					}

					if (!foundError) {
						isValidPosition = true;
					}
				} while (!isValidPosition && currentTries < 10);

				ecb.SetComponent(spawnedEntity, new LocalTransform { Position = spawnPosition, Rotation = quaternion.identity, Scale = 1f });
				ecb.SetComponent(spawnedEntity, pointId.ValueRO);

				spawnAmount.ValueRW.CurrentValue += 1;
				spawnCooldown.ValueRW.CurrentCooldown = spawnCooldown.ValueRO.Cooldown;
			}
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
		}
	}
}