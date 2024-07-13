using Unity.Entities;
using UnityEngine;

namespace PotatoFinch.TmgDotsJam.Shop {
	public class ShopAuthoring : MonoBehaviour {
		[SerializeField] private float _activeShopRange;

		private class ShopAuthoringBaker : Baker<ShopAuthoring> {
			public override void Bake(ShopAuthoring authoring) {
				var shopEntity = GetEntity(authoring, TransformUsageFlags.WorldSpace);
				AddComponent<ShopTag>(shopEntity);
				AddComponent<ShopActiveTag>(shopEntity);
				AddComponent(shopEntity, new ActiveShopRange { Value = authoring._activeShopRange });
			}
		}
	}
}