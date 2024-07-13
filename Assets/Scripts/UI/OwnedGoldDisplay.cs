using System.Globalization;
using PotatoFinch.TmgDotsJam.Shop;
using TMPro;
using Unity.Entities;
using UnityEngine;

namespace PotatoFinch.TmgDotsJam.UI {
	public class OwnedGoldDisplay : MonoBehaviour {
		[SerializeField] private TMP_Text _ownedGoldDisplay;

		private EntityQuery _ownedGoldQuery;

		private void Awake() {
			_ownedGoldQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(OwnedGold));
		}

		private void Update() {
			if (!_ownedGoldQuery.TryGetSingleton(out OwnedGold ownedGoldComponent)) {
				return;
			}

			_ownedGoldDisplay.text = $"Owned Gold: {ownedGoldComponent.Value.ToString(CultureInfo.InvariantCulture)}";
		}
	}
}