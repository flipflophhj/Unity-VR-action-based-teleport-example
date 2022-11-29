using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SpawnerAuthoring : MonoBehaviour
{
   public GameObject prefab;
}

public class SpawnerBaker : Baker<SpawnerAuthoring>
{
   public override void Bake(SpawnerAuthoring authoring)
   {
       AddComponent(new Spawner { entity = GetEntity(authoring.prefab) });
   }
}