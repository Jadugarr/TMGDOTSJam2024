using PotatoFinch.TmgDotsJam.GameTime;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace PotatoFinch.TmgDotsJam.Combat {
	[UpdateInGroup(typeof(CombatSystemGroup))]
	public partial struct CountdownColorLerpValueSystem : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<GameTimeComponent>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			var gameTimeComponent = SystemAPI.GetSingleton<GameTimeComponent>();

			state.Dependency = new CountdownJob { DeltaTime = gameTimeComponent.DeltaTime }.ScheduleParallel(state.Dependency);
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {

		}

		[BurstCompile]
		private partial struct CountdownJob : IJobEntity {
			public float DeltaTime;
			
			public void Execute(RefRW<ColorLerpMaterialOverride> colorLerpMaterialOverride) {
				if (colorLerpMaterialOverride.ValueRO.Value <= 0f) {
					return;
				}

				colorLerpMaterialOverride.ValueRW.Value = math.max(0f, colorLerpMaterialOverride.ValueRO.Value - DeltaTime * 5f);
			}
		}
	}
}