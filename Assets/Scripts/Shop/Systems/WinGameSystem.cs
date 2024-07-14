using PotatoFinch.TmgDotsJam.GameControls;
using PotatoFinch.TmgDotsJam.GameState;
using PotatoFinch.TmgDotsJam.GameTime;
using PotatoFinch.TmgDotsJam.UI;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace PotatoFinch.TmgDotsJam.Shop {
	[UpdateInGroup(typeof(ShopSystemGroup))]
	public partial struct WinGameSystem : ISystem, ISystemStartStop {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<WinGameTag>();
		}

		public void OnStartRunning(ref SystemState state) {
			GameObject.FindGameObjectWithTag("ShopDialog").GetComponent<ShopDialogHandler>().ActivateShopDialog(false);
			GameObject.FindGameObjectWithTag("WinDialog").GetComponent<WonGameDialogHandler>().ActivateDialog(true);
			state.EntityManager.CreateEntity(typeof(GamePausedTag));
			state.EntityManager.CreateEntity(typeof(IgnoreInputTag));
		}

		public void OnStopRunning(ref SystemState state) {
		}

		public void OnUpdate(ref SystemState state) {
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {

		}
	}
}