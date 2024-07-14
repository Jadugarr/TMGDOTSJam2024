using PotatoFinch.TmgDotsJam.Enemy;
using PotatoFinch.TmgDotsJam.Health;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace PotatoFinch.TmgDotsJam.Combat {
	[UpdateInGroup(typeof(CombatSystemGroup))]
	public partial struct DamagePlayerSystem : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<PlayerTag>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			var playerPosition = SystemAPI.GetComponentRO<LocalTransform>(SystemAPI.GetSingletonEntity<PlayerTag>()).ValueRO.Position;

			state.Dependency = new DamagePlayerJob {
				PlayerPosition = playerPosition,
				CharacterHealthLookup = SystemAPI.GetComponentLookup<CharacterHealth>(),
				DamageCooldownLookup = SystemAPI.GetComponentLookup<DamageCooldown>(),
				ColorLerpMaterialOverrideLookup = SystemAPI.GetComponentLookup<ColorLerpMaterialOverride>(),
			}.Schedule(state.Dependency);
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {

		}

		[BurstCompile]
		[WithAll(typeof(EnemyTag))]
		private partial struct DamagePlayerJob : IJobEntity {
			public ComponentLookup<DamageCooldown> DamageCooldownLookup;
			public ComponentLookup<CharacterHealth> CharacterHealthLookup;
			public ComponentLookup<ColorLerpMaterialOverride> ColorLerpMaterialOverrideLookup;
			
			public float3 PlayerPosition;
			
			public void Execute(RefRO<LocalTransform> currentPosition, RefRO<TargetEnemy> targetEnemy, RefRO<DamageValue> damageValue) {
				if (math.distance(currentPosition.ValueRO.Position, PlayerPosition) > 0.9f) {
					return;
				}

				if (!DamageCooldownLookup.TryGetComponent(targetEnemy.ValueRO.Value, out DamageCooldown damageCooldown)) {
					return;
				}

				if (damageCooldown.Value > 0f) {
					return;
				}

				var characterHealth = CharacterHealthLookup[targetEnemy.ValueRO.Value];
				characterHealth.CurrentHealth -= damageValue.ValueRO.Value;
				CharacterHealthLookup[targetEnemy.ValueRO.Value] = characterHealth;

				DamageCooldownLookup[targetEnemy.ValueRO.Value] = new DamageCooldown { Value = 0.5f };
				ColorLerpMaterialOverrideLookup[targetEnemy.ValueRO.Value] = new ColorLerpMaterialOverride { Value = 1f };
			}
		}
	}
}