using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.Health {
	public struct CharacterHealth : IComponentData {
		public float MaxHealth;
        public float CurrentHealth;
	}
}