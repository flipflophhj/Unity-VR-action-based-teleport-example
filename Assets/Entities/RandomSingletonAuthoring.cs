using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class RandomSingletonAuthoring : MonoBehaviour
{
}

public class RandomSingletonBaker : Baker<RandomSingletonAuthoring>
{
   public override void Bake(RandomSingletonAuthoring authoring)
   {
       AddComponent(new RandomSingleton { random = new Unity.Mathematics.Random(1) });
   }
}