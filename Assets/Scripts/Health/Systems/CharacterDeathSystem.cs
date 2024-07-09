using Unity.Burst;
using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.Health {
	[UpdateInGroup(typeof(CharacterHealthSystemGroup))]
	public partial struct CharacterDeathSystem : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			state.Dependency = new MarkEnemiesAsDeadJob { Ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged) }.Schedule(state.Dependency);
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
		}

		[BurstCompile]
		[WithNone(typeof(CharacterDeadTag))]
		[WithChangeFilter(typeof(CharacterHealth))]
		private partial struct MarkEnemiesAsDeadJob : IJobEntity {
			public EntityCommandBuffer Ecb;

			public void Execute(Entity characterEntity, RefRO<CharacterHealth> characterHealth) {
				if (characterHealth.ValueRO.CurrentHealth > 0f) {
					return;
				}

				Ecb.AddComponent<CharacterDeadTag>(characterEntity);
			}
		}
	}
}