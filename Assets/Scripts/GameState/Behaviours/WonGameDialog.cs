using Unity.Entities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PotatoFinch.TmgDotsJam.GameState {
	public class WonGameDialog : MonoBehaviour {
		[SerializeField] private Button _restartButton;
		[SerializeField] private Button _quitButton;

		private void Awake() {
			_restartButton.onClick.AddListener(OnRestartClicked);
			_quitButton.onClick.AddListener(OnQuitClicked);
		}

		private void OnQuitClicked() {
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}

		private void OnRestartClicked() {
			World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity(typeof(ResetGameTag));
			gameObject.SetActive(false);
		}
	}
}