using Unity.Burst;
using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.Shop {
	[UpdateInGroup(typeof(ShopSystemGroup))]
	public partial struct InitializeBoughtUpgradesSystem : ISystem {
		
		public void OnCreate(ref SystemState state) {
			state.EntityManager.CreateEntity(typeof(BoughtUpgrade));
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {

		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {

		}
	}
}