using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace PotatoFinch.TmgDotsJam.Shop {
	[UpdateInGroup(typeof(ShopSystemGroup))]
	public partial struct HandleShopPromptActivation : ISystem {
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<ShopTag>();
			state.RequireForUpdate<ActiveShopRange>();
			state.RequireForUpdate<PlayerTag>();
			state.RequireForUpdate<ShopPromptComponent>();
		}

		public void OnUpdate(ref SystemState state) {
			var playerPosition = SystemAPI.GetComponentRO<LocalTransform>(SystemAPI.GetSingletonEntity<PlayerTag>());
			var shopEntity = SystemAPI.GetSingletonEntity<ShopTag>();
			var shopPosition = SystemAPI.GetComponentRO<LocalToWorld>(shopEntity);
			var activeShopRange = SystemAPI.GetSingleton<ActiveShopRange>();

			var isActive = math.distance(playerPosition.ValueRO.Position, shopPosition.ValueRO.Position) < activeShopRange.Value;
			SystemAPI.ManagedAPI.GetSingleton<ShopPromptComponent>().Value.SetActive(isActive);
			SystemAPI.SetComponentEnabled<ShopActiveTag>(shopEntity, isActive);
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
		}
	}
}