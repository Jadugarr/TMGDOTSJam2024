﻿using PotatoFinch.TmgDotsJam.Movement;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace PotatoFinch.TmgDotsJam.Combat {
	[UpdateInGroup(typeof(CombatSystemGroup))]
	public partial struct SetProjectileVelocityToTargetEnemySystem : ISystem {
		[BurstCompile]
		public void OnCreate(ref SystemState state) {
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			state.Dependency = new SetProjectileVelocityJob { DeltaTime = SystemAPI.Time.DeltaTime, LocalTransformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true) }.ScheduleParallel(state.Dependency);
		}

		[BurstCompile]
		public void OnDestroy(ref SystemState state) {
		}

		[BurstCompile]
		private partial struct SetProjectileVelocityJob : IJobEntity {
			[ReadOnly] public ComponentLookup<LocalTransform> LocalTransformLookup;

			public float DeltaTime;

			public void Execute(RefRW<Velocity> velocity, RefRO<LocalTransform> localTransform, RefRO<MovementSpeed> movementSpeed, RefRO<TargetEnemy> targetEnemy) {
				float3 dir = math.normalize(LocalTransformLookup[targetEnemy.ValueRO.Value].Position - localTransform.ValueRO.Position);

				velocity.ValueRW.Value = dir * movementSpeed.ValueRO.Value * DeltaTime;
			}
		}
	}
}