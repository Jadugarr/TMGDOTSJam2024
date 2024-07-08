using UnityEngine;

namespace PotatoFinch.TmgDotsJam.Enemy {
	public class EnemySpawnPointBehaviour : MonoBehaviour {
		[SerializeField] private float _range;

		public float Range => _range;

		private void OnDrawGizmos() {
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, _range);
		}
	}
}