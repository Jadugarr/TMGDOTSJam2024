using PotatoFinch.TmgDotsJam.GameState;
using PotatoFinch.TmgDotsJam.Health;
using Unity.Burst;
using Unity.Entities;

namespace PotatoFinch.TmgDotsJam.Combat {
	[UpdateInGroup(typeof(CombatSystemGroup))]
	public partial struct PlayerDeathSystem : ISystem, ISystemStartStop {
		private EntityQuery _deadPlayerQuery;
		
		public void OnCreate(ref SystemState state) {
			_deadPlayerQuery = state.GetEntityQuery(typeof(PlayerTag), typeof(CharacterDeadTag));
			
			state.RequireForUpdate(_deadPlayerQuery);
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {

		}

		public void OnStartRunning(ref SystemState state) {
			state.EntityManager.CreateEntity(typeof(ResetGameTag));
		}

		public void OnStopRunning(ref SystemState state) {
		}
	}
}