using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace PotatoFinch.TmgDotsJam.Combat {
	[UpdateInGroup(typeof(CombatSystemGroup))]
	public partial struct PropagateColorLerpToChildrenSystem : ISystem {
		private ComponentLookup<ColorLerpMaterialOverride> _colorLerpMaterialOverrideLookup;

		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
			_colorLerpMaterialOverrideLookup = state.GetComponentLookup<ColorLerpMaterialOverride>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			_colorLerpMaterialOverrideLookup.Update(ref state);
			var parallelEcb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

			state.Dependency = new PropagateComponentJob {
				ComponentLookup = _colorLerpMaterialOverrideLookup,
				ParallelEcb = parallelEcb
			}.ScheduleParallel(state.Dependency);
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
		}

		[BurstCompile]
		[WithChangeFilter(typeof(Child))]
		[WithChangeFilter(typeof(ColorLerpMaterialOverride))]
		private partial struct PropagateComponentJob : IJobEntity {
			[ReadOnly]
			public ComponentLookup<ColorLerpMaterialOverride> ComponentLookup;
			
			public EntityCommandBuffer.ParallelWriter ParallelEcb;

			public void Execute([EntityIndexInQuery] int index, RefRO<ColorLerpMaterialOverride> isPlaceableMaterialOverride, in DynamicBuffer<Child> childBuffer) {
				foreach (Child child in childBuffer) {
					if (ComponentLookup.HasComponent(child.Value)) {
						ParallelEcb.SetComponent(index, child.Value, isPlaceableMaterialOverride.ValueRO);
					}
					else {
						ParallelEcb.AddComponent(index, child.Value, isPlaceableMaterialOverride.ValueRO);
					}
				}
			}
		}
	}
}