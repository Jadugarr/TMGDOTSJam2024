using PotatoFinch.TmgDotsJam.Common;
using PotatoFinch.TmgDotsJam.Enemy;
using PotatoFinch.TmgDotsJam.Health;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace PotatoFinch.TmgDotsJam.Combat {
	[UpdateInGroup(typeof(CombatSystemGroup))]
	public partial struct AttackEnemyInRangeSystem : ISystem, ISystemStartStop {
		private NativeHashMap<int, Entity> _attackTypeToPrefabEntityMap;

		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
			state.RequireForUpdate<PrefabContainer>();
			state.RequireForUpdate<PlayerTag>();
			state.RequireForUpdate<AvailableAttack>();
		}

		public void OnStartRunning(ref SystemState state) {
			var prefabContainer = SystemAPI.GetSingleton<PrefabContainer>();
			_attackTypeToPrefabEntityMap = new NativeHashMap<int, Entity>(1, Allocator.Persistent);

			_attackTypeToPrefabEntityMap.Add((int)AttackType.Bullet, prefabContainer.BulletPrefab);
		}

		public void OnStopRunning(ref SystemState state) {
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			using NativeList<AvailableAttack> availableAttacks = new NativeList<AvailableAttack>(Allocator.Temp);

			var availableAttackBuffer = SystemAPI.GetSingletonBuffer<AvailableAttack>();
			for (var i = 0; i < availableAttackBuffer.Length; i++) {
				var availableAttack = availableAttackBuffer[i];
				if (availableAttack.CurrentCooldown > 0f) {
					continue;
				}

				availableAttacks.Add(availableAttack);
			}

			if (availableAttacks.Length <= 0) {
				return;
			}

			float shortestEnemyDistance = float.MaxValue;
			float3 playerPosition = SystemAPI.GetComponentRO<LocalTransform>(SystemAPI.GetSingletonEntity<PlayerTag>()).ValueRO.Position;
			float3 enemyPosition = float3.zero;
			Entity enemyEntity = Entity.Null;

			foreach ((RefRO<LocalTransform> localTransform, Entity entity) in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<EnemyTag>().WithNone<CharacterDeadTag>().WithEntityAccess()) {
				var distance = math.distance(localTransform.ValueRO.Position, playerPosition);
				if (distance >= shortestEnemyDistance) {
					continue;
				}

				shortestEnemyDistance = distance;
				enemyPosition = localTransform.ValueRO.Position;
				enemyEntity = entity;
			}

			if (math.all(enemyPosition == float3.zero) || enemyEntity == Entity.Null) {
				return;
			}

			EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

			foreach (AvailableAttack availableAttack in availableAttacks) {
				if (availableAttack.Range < shortestEnemyDistance) {
					continue;
				}

				if (!_attackTypeToPrefabEntityMap.TryGetValue((int)availableAttack.AttackType, out Entity projectilePrefab)) {
					Debug.LogError($"Forgot to set up a prefab for {availableAttack.AttackType}!");
					return;
				}

				var bulletEntity = ecb.Instantiate(projectilePrefab);
				ecb.SetComponent(bulletEntity, new LocalTransform { Position = playerPosition, Rotation = quaternion.identity, Scale = 1f });
				ecb.SetComponent(bulletEntity, new TargetEnemy { Value = enemyEntity });
				
				for (var i = 0; i < availableAttackBuffer.Length; i++) {
					var bufferElement = availableAttackBuffer[i];

					if (bufferElement.AttackType != availableAttack.AttackType) {
						continue;
					}

					bufferElement.CurrentCooldown = bufferElement.Cooldown;
					availableAttackBuffer[i] = bufferElement;
				}
			}
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
		}
	}
}