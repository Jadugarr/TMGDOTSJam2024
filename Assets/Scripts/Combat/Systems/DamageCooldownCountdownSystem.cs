using PotatoFinch.TmgDotsJam.GameTime;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace PotatoFinch.TmgDotsJam.Combat {
	[UpdateInGroup(typeof(CombatSystemGroup))]
	public partial struct DamageCooldownCountdownSystem : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<GameTimeComponent>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			var gameTimeComponent = SystemAPI.GetSingleton<GameTimeComponent>();

			foreach (RefRW<DamageCooldown> damageCooldown in SystemAPI.Query<RefRW<DamageCooldown>>()) {
				if (damageCooldown.ValueRO.Value <= 0f) {
					continue;
				}

				damageCooldown.ValueRW.Value = math.max(0f, damageCooldown.ValueRO.Value - gameTimeComponent.DeltaTime);
			}
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
		}
	}
}