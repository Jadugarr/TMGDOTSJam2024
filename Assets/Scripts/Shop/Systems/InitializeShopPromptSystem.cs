using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace PotatoFinch.TmgDotsJam.Shop {
	public partial struct InitializeShopPromptSystem : ISystem, ISystemStartStop {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
		}

		public void OnStartRunning(ref SystemState state) {
			var shopPromptEntity = state.EntityManager.CreateEntity(typeof(ShopPromptComponent));

			var shopPrompt = GameObject.FindGameObjectWithTag("OpenShopPrompt");
			state.EntityManager.AddComponentData(shopPromptEntity, new ShopPromptComponent { Value = shopPrompt });
		}

		public void OnStopRunning(ref SystemState state) {
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
		}
	}
}