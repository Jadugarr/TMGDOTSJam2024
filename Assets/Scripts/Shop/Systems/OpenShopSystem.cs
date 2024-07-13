using PotatoFinch.TmgDotsJam.GameControls;
using PotatoFinch.TmgDotsJam.GameTime;
using PotatoFinch.TmgDotsJam.UI;
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

			GameObject.FindGameObjectWithTag("ShopDialog").GetComponent<ShopDialogHandler>().ActivateShopDialog(true);
			state.EntityManager.CreateEntity(typeof(GamePausedTag));
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {

		}
	}
}