using PotatoFinch.TmgDotsJam.GameControls;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace PotatoFinch.TmgDotsJam.Shop {
	public partial struct OpenShopSystem : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<CurrentGameInput>();
		}

		public void OnUpdate(ref SystemState state) {
			if (!SystemAPI.GetSingleton<CurrentGameInput>().OpenShop) {
				return;
			}
			
			GameObject.FindWithTag("ShopDialog").SetActive(true);
			
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {

		}
	}
}