using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct MovingSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ramdomSingleton = SystemAPI.GetSingletonRW<RandomSingleton>();
        var deltaTime = SystemAPI.Time.DeltaTime;

        var moveJobHandle = new MoveJob {
            deltaTime = deltaTime
        }.ScheduleParallel(state.Dependency);

        moveJobHandle.Complete();

        new HandleArrivedJob {
            randomRef = ramdomSingleton
        }.Run();

        // foreach (var aspect in SystemAPI.Query<MoveToPositionAspect>()) {
        //     aspect.HandleArrived(ramdomSingleton);
        // }
    }
}

[BurstCompile]
public partial struct MoveJob : IJobEntity 
{
    public float deltaTime;
    [BurstCompile]
    public void Execute(MoveToPositionAspect aspect) {
        aspect.Move(deltaTime);
    }
}

[BurstCompile]
public partial struct HandleArrivedJob : IJobEntity 
{
    [NativeDisableUnsafePtrRestriction]
    public RefRW<RandomSingleton> randomRef;
    [BurstCompile]
    public void Execute(MoveToPositionAspect aspect) {
        aspect.HandleArrived(randomRef);
    }
}
