using System;
using System.Globalization;
using PotatoFinch.TmgDotsJam.GameTime;
using TMPro;
using Unity.Entities;
using UnityEngine;

namespace PotatoFinch.TmgDotsJam.UI {
	public class RemainingGameTimeDisplay : MonoBehaviour {
		[SerializeField] private TMP_Text _textfield;

		private EntityQuery _gameTimeQuery;

		private void Awake() {
			_gameTimeQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(GameTimeComponent));
		}

		private void Update() {
			if (!_gameTimeQuery.TryGetSingleton(out GameTimeComponent gameTimeComponent)) {
				return;
			}

			_textfield.text = gameTimeComponent.RemainingTime.ToString("F2", CultureInfo.InvariantCulture);
		}
	}
}