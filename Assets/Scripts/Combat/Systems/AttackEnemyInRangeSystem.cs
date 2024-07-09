using PotatoFinch.TmgDotsJam.Common;
using PotatoFinch.TmgDotsJam.Enemy;
using PotatoFinch.TmgDotsJam.Health;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace PotatoFinch.TmgDotsJam.Combat {
	[UpdateInGroup(typeof(CombatSystemGroup))]
	public partial struct AttackEnemyInRangeSystem : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
			state.RequireForUpdate<PrefabContainer>();
			state.RequireForUpdate<PlayerTag>();
			state.RequireForUpdate<AvailableAttack>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			// Check if any of player's available attacks are off cooldown
			// Find the closest enemy to the player
			// Check for each available attack if that enemy is in range
			// Send each attack to the enemy
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

			foreach (RefRO<LocalTransform> localTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<EnemyTag>().WithNone<CharacterDeadTag>()) {
				var distance = math.distance(localTransform.ValueRO.Position, playerPosition);
				if (distance >= shortestEnemyDistance) {
					continue;
				}

				shortestEnemyDistance = distance;
				enemyPosition = localTransform.ValueRO.Position;
			}

			if (math.all(enemyPosition == float3.zero)) {
				return;
			}
			
			enemyPosition.y = 1.5f;
			var prefabContainer = SystemAPI.GetSingleton<PrefabContainer>();
			EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

			foreach (AvailableAttack availableAttack in availableAttacks) {
				if (availableAttack.Range < shortestEnemyDistance) {
					continue;
				}
				
				var bulletEntity = ecb.Instantiate(prefabContainer.BulletPrefab);
				ecb.SetComponent(bulletEntity, new LocalTransform { Position = enemyPosition, Rotation = quaternion.identity, Scale = 1f });
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