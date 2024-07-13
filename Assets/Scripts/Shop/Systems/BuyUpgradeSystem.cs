using PotatoFinch.TmgDotsJam.Enemy;
using PotatoFinch.TmgDotsJam.GameTime;
using PotatoFinch.TmgDotsJam.Movement;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace PotatoFinch.TmgDotsJam.Shop {
	public partial struct BuyUpgradeSystem : ISystem {
		private EntityQuery _buyUpgradeQuery;

		public void OnCreate(ref SystemState state) {
			_buyUpgradeQuery = state.GetEntityQuery(typeof(BuyUpgrade));

			state.RequireForUpdate<OwnedGold>();
			state.RequireForUpdate<OriginalPlayerStats>();
			state.RequireForUpdate<PlayerTag>();
			state.RequireForUpdate<BuyUpgrade>();
			state.RequireForUpdate<BoughtUpgrade>();
			state.RequireForUpdate<UpgradeShipInfoBlobAssetReference>();
			state.RequireForUpdate<GameTimeComponent>();
			state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			var boughtUpgradesEntity = SystemAPI.GetSingletonEntity<BoughtUpgrade>();
			var boughtUpgrades = SystemAPI.GetSingletonBuffer<BoughtUpgrade>();
			var upgradeShipInfoBlobAssetReference = SystemAPI.GetSingleton<UpgradeShipInfoBlobAssetReference>();
			var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
			var originalPlayerStats = SystemAPI.GetSingleton<OriginalPlayerStats>();
			var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
			var ownedGold = SystemAPI.GetSingletonRW<OwnedGold>();
			var gameTimeComponent = SystemAPI.GetSingletonRW<GameTimeComponent>();

			foreach (RefRO<BuyUpgrade> buyUpgrade in SystemAPI.Query<RefRO<BuyUpgrade>>()) {
				if (!upgradeShipInfoBlobAssetReference.TryGetUpgradeShopInfoByUpgradeType(buyUpgrade.ValueRO.Value, out UpgradeShopInfo upgradeShopInfo)) {
					Debug.LogError($"No upgrade of type {buyUpgrade.ValueRO.Value} has been defined in the blob asset!");
					continue;
				}

				BoughtUpgrade boughtUpgrade = default;
				if (boughtUpgrades.TryGetBoughtUpgrade(buyUpgrade.ValueRO.Value, out int boughtUpgradeIndex)) {
					boughtUpgrade = boughtUpgrades[boughtUpgradeIndex];

					if (boughtUpgrade.CurrentLevel >= upgradeShopInfo.MaxLevel) {
						continue;
					}
				}

				int levelToBuy = boughtUpgrade.CurrentLevel + 1;
				var cost = upgradeShopInfo.GetCostByLevel(levelToBuy);
				if (ownedGold.ValueRO.Value < cost) {
					continue;
				}

				ownedGold.ValueRW.Value -= cost;

				if (boughtUpgradeIndex > -1) {
					boughtUpgrade.CurrentLevel++;
					boughtUpgrades[boughtUpgradeIndex] = boughtUpgrade;
				}
				else {
					boughtUpgrade = new BoughtUpgrade { UpgradeType = buyUpgrade.ValueRO.Value, CurrentLevel = 1 };
					ecb.AppendToBuffer(boughtUpgradesEntity, boughtUpgrade);
				}

				// Handle upgrade type
				switch (buyUpgrade.ValueRO.Value) {
					case UpgradeType.MovementSpeed:
						SystemAPI.SetComponent(playerEntity, new MovementSpeed { Value = originalPlayerStats.MovementSpeed * (1f + 0.1f * boughtUpgrade.CurrentLevel) });
						break;
					case UpgradeType.TimerReset:
						gameTimeComponent.ValueRW.RemainingTime = gameTimeComponent.ValueRO.TotalTime;
						break;
					case UpgradeType.EnemySpawnCooldown:
						foreach (RefRW<EnemySpawnCooldown> enemySpawnCooldown in SystemAPI.Query<RefRW<EnemySpawnCooldown>>()) {
							enemySpawnCooldown.ValueRW.Cooldown -= 1f;
						}
						break;
					default:
						Debug.LogError($"Undefined handling for upgrade type {buyUpgrade.ValueRO.Value}!");
						break;
				}
			}

			ecb.DestroyEntity(_buyUpgradeQuery, EntityQueryCaptureMode.AtRecord);
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
		}
	}
}