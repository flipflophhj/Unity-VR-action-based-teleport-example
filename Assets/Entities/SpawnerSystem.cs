using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class SpawnerSystem : SystemBase
{
    int count;

    protected override void OnCreate()
    {
        count = 0;
        base.OnCreate();
    }

    protected override void OnUpdate()
    {
        var spawner = SystemAPI.GetSingleton<Spawner>();
        var singleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var buffer = singleton.CreateCommandBuffer(World.Unmanaged);

        while (count++ < 10000) {
            buffer.Instantiate(spawner.entity);
        }
    }
}
