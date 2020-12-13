using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Physics;
using Unity.Mathematics;

public class MoveSystem : SystemBase
{

    private struct asd : ITriggerEventsJobBase
    {
        public void Execute(TriggerEvent triggerEvent)
        {
            throw new System.NotImplementedException();
        }
    }

    protected override void OnUpdate()
    {
        Entities
            .ForEach((ref Translation translation,
                      in VelocityComponent velocity) =>
            {
                translation.Value += velocity.Value;
            })
            .WithBurst()
            .ScheduleParallel();
    }
}