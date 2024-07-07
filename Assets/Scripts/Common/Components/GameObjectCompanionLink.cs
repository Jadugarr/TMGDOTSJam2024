using System;
using Unity.Entities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PotatoFinch.TmgDotsJam.Common {
	public class GameObjectCompanionLink : IComponentData, IEquatable<GameObjectCompanionLink>, IDisposable, ICloneable
	{
		public GameObject Companion;
		public bool Equals(GameObjectCompanionLink other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(Companion, other.Companion);
		}

		public override int GetHashCode()
		{
			return ReferenceEquals(Companion, null) ? 0 : Companion.GetHashCode();
		}

		public void Dispose()
		{
#if UNITY_EDITOR
			if (Application.isPlaying)
				Object.Destroy(Companion);
			else
				Object.DestroyImmediate(Companion);
#else
            Object.Destroy(Companion);
#endif
		}

		public object Clone()
		{
			var cloned = new GameObjectCompanionLink { Companion = Object.Instantiate(Companion) };
			return cloned;
		}
	}
}