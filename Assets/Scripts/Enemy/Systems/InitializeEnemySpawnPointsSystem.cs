﻿using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace PotatoFinch.TmgDotsJam.Enemy {
	[UpdateInGroup(typeof(EnemySystemGroup))]
	public partial struct InitializeEnemySpawnPointsSystem : ISystem, ISystemStartStop {
		private EntityArchetype _spawnPointArchetype;

		public void OnCreate(ref SystemState state) {
			_spawnPointArchetype = state.EntityManager.CreateArchetype(typeof(EnemySpawnAmount), typeof(EnemySpawnPointId), typeof(EnemySpawnPointRange), typeof(EnemySpawnPointOrigin), typeof(EnemySpawnCooldown));
		}

		public void OnStartRunning(ref SystemState state) {
			var enemySpawnPointGameObjects = Object.FindObjectsByType<EnemySpawnPointBehaviour>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

			for (var i = 0; i < enemySpawnPointGameObjects.Length; i++) {
				var enemySpawnPointGameObject = enemySpawnPointGameObjects[i];

				var spawnPointEntity = state.EntityManager.CreateEntity(_spawnPointArchetype);
				SystemAPI.SetComponent(spawnPointEntity, new EnemySpawnAmount { MaxValue = 10 });
				SystemAPI.SetComponent(spawnPointEntity, new EnemySpawnPointId { Value = i });
				SystemAPI.SetComponent(spawnPointEntity, new EnemySpawnPointRange { Value = enemySpawnPointGameObject.Range });
				SystemAPI.SetComponent(spawnPointEntity, new EnemySpawnPointOrigin { Value = enemySpawnPointGameObject.gameObject.transform.position });
				SystemAPI.SetComponent(spawnPointEntity, new EnemySpawnCooldown { Cooldown = 10f, CurrentCooldown = 10f });
			}

			state.EntityManager.CreateEntity(typeof(SpawnAllEnemiesTag));
		}

		public void OnStopRunning(ref SystemState state) {
		}

		public void OnUpdate(ref SystemState state) {
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
		}
	}
}