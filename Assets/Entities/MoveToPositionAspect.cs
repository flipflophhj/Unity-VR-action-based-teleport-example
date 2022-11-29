using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct MoveToPositionAspect : IAspect 
{
   private readonly Entity entity;
   private readonly TransformAspect transformAspect;
   private readonly RefRO<Speed> speed;
   private readonly RefRW<TargetPosition> targetPosition;

   public void Move(float deltaTime) 
   {
      if (math.lengthsq(targetPosition.ValueRO.value - transformAspect.Position) > 0) {
         // move towards target
         float3 dir = math.normalize(targetPosition.ValueRO.value - transformAspect.Position);
         transformAspect.Position += dir * deltaTime * speed.ValueRO.value;
      }
   }
   public void HandleArrived(RefRW<RandomSingleton> randomRef) 
   {
      float threshold = 0.05f * 0.05f;
      if (math.distancesq(transformAspect.Position, targetPosition.ValueRO.value) < threshold) {
         var random = randomRef.ValueRO.random;
         targetPosition.ValueRW.value = new float3(random.NextFloat(-5f, 5f), 0, random.NextFloat(2f, 10f));
         randomRef.ValueRW.random = random;
      }
   }
}
