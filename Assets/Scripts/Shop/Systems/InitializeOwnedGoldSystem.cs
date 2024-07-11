using Unity.Burst;
using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.Shop {
	[UpdateInGroup(typeof(ShopSystemGroup))]
	public partial struct InitializeOwnedGoldSystem : ISystem {
		
		public void OnCreate(ref SystemState state) {
			state.EntityManager.CreateEntity(typeof(OwnedGold));
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) { }

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {

		}
	}
}