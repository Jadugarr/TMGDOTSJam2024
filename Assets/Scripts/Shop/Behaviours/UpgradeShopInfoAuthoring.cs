using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace PotatoFinch.TmgDotsJam.Shop {
	public class UpgradeShopInfoAuthoring : MonoBehaviour {
		[SerializeField] private UpgradeShopInfoEditorSetup[] _upgradeShopInfos;

		private class UpgradeShopInfoAuthoringBaker : Baker<UpgradeShopInfoAuthoring> {
			public override void Bake(UpgradeShopInfoAuthoring authoring) {
				var blobBuilder = new BlobBuilder(Allocator.Temp);

				ref var upgradeShopInfos = ref blobBuilder.ConstructRoot<UpgradeShopInfos>();

				var shopInfoArray = blobBuilder.Allocate(ref upgradeShopInfos.Value, authoring._upgradeShopInfos.Length);

				for (int i = 0; i < authoring._upgradeShopInfos.Length; i++) {
					var upgradeShopInfoEditorSetup = authoring._upgradeShopInfos[i];

					shopInfoArray[i] = new UpgradeShopInfo {
						Description = upgradeShopInfoEditorSetup.Description,
						Name = upgradeShopInfoEditorSetup.Name,
						Cost = upgradeShopInfoEditorSetup.Cost,
						MaxLevel = upgradeShopInfoEditorSetup.MaxLevel,
						UpgradeType = upgradeShopInfoEditorSetup.UpgradeType
					};
				}

				var blobAssetReference = blobBuilder.CreateBlobAssetReference<UpgradeShopInfos>(Allocator.Persistent);
				AddBlobAsset(ref blobAssetReference, out var hash);
				var entity = GetEntity(TransformUsageFlags.None);
				AddComponent(entity, new UpgradeShipInfoBlobAssetReference { Value = blobAssetReference });
			}
		}
	}
}