using UnityEngine;

namespace PotatoFinch.TmgDotsJam.GameState {
	public class WonGameDialogHandler : MonoBehaviour {
		[SerializeField] private GameObject _wonGameDialogObject;

		public void ActivateDialog(bool isActive) {
			_wonGameDialogObject.SetActive(isActive);
		}
	}
}