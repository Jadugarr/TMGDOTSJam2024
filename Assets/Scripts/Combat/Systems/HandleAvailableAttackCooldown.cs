using PotatoFinch.TmgDotsJam.GameTime;
using Unity.Burst;
using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.Combat {
	[UpdateInGroup(typeof(CombatSystemGroup))]
	public partial struct HandleAvailableAttackCooldown : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<AvailableAttack>();
			state.RequireForUpdate<GameTimeComponent>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			var deltaTime = SystemAPI.GetSingleton<GameTimeComponent>().DeltaTime;
			var availableAttacks = SystemAPI.GetSingletonBuffer<AvailableAttack>();

			for (var i = 0; i < availableAttacks.Length; i++) {
				var availableAttack = availableAttacks[i];

				if (availableAttack.CurrentCooldown <= 0f) {
					continue;
				}
				
				availableAttack.CurrentCooldown -= deltaTime;
				availableAttacks[i] = availableAttack;
			}
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {

		}
	}
}