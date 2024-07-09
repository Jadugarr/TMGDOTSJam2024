using PotatoFinch.TmgDotsJam.Enemy;
using PotatoFinch.TmgDotsJam.GameControls;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.Health {
	[UpdateInGroup(typeof(CharacterHealthSystemGroup))]
	public partial struct KillRandomEnemyTestSystem : ISystem {
		private EntityQuery _enemyHealthQuery;
		
		public void OnCreate(ref SystemState state) {
			_enemyHealthQuery = state.GetEntityQuery(typeof(EnemyTag), typeof(CharacterHealth));
			
			state.RequireForUpdate<CurrentGameInput>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			var currentGameInput = SystemAPI.GetSingleton<CurrentGameInput>();

			if (!currentGameInput.KillRandomEnemy || _enemyHealthQuery.CalculateEntityCount() <= 0) {
				return;
			}

			using var enemyEntityArray = _enemyHealthQuery.ToEntityArray(Allocator.Temp);

			SystemAPI.GetComponentRW<CharacterHealth>(enemyEntityArray[0]).ValueRW.CurrentHealth = 0f;
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {

		}
	}
}