using PotatoFinch.TmgDotsJam.Common;
using PotatoFinch.TmgDotsJam.Movement;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace PotatoFinch.TmgDotsJam.GameCamera {
	[UpdateInGroup(typeof(GameCameraSystemGroup))]
	public partial struct LinkFollowTargetToEntitySystem : ISystem, ISystemStartStop {
		private EntityArchetype _cameraFollowEntityArchetype;
		
		public void OnCreate(ref SystemState state) {
			_cameraFollowEntityArchetype = state.EntityManager.CreateArchetype(typeof(CameraFollowTargetTag), typeof(LocalTransform), typeof(LocalToWorld), typeof(MovementSpeed), typeof(GameObjectCompanionLink));
			
			state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
		}

		public void OnStartRunning(ref SystemState state) {
			EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
			var followTargetEntity = ecb.CreateEntity(_cameraFollowEntityArchetype);
			ecb.SetComponent(followTargetEntity, new CameraFollowTargetTag());
			ecb.SetComponent(followTargetEntity, new LocalTransform());
			ecb.SetComponent(followTargetEntity, new LocalToWorld());
			ecb.SetComponent(followTargetEntity, new MovementSpeed { Value = 10f });
			var cameraFollowTargetObject = GameObject.FindGameObjectWithTag("CameraFollowTarget");
			ecb.SetComponent(followTargetEntity, new GameObjectCompanionLink { Companion = cameraFollowTargetObject });
		}

		[BurstCompile]
		public void OnStopRunning(ref SystemState state) { }

		[BurstCompile]
		public void OnUpdate(ref SystemState state) { }

		[BurstCompile]
		public void OnDestroy(ref SystemState state) { }
	}
}