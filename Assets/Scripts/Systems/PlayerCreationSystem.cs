using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Jobs;

public class PlayerCreationSystem : SystemBase
{
    protected override void OnCreate()
    {
        base.OnCreate();
        rnd = new Random(811515);
    }

    Random rnd;

    private static int remainingPlayerCount = 10;
    private double lastCreateTime = 0;

    protected override void OnUpdate()
    {
        if (Time.ElapsedTime - lastCreateTime < 0.1f)
        {
            return;
        }

        lastCreateTime = Time.ElapsedTime;

        Entity? PlayerPrefab = null;
        Entity? BallPrefab = null;

        Entities.ForEach((ref PrefabContainer prefabContainer) =>
        {
            PlayerPrefab = prefabContainer.PlayerPrefab;
            BallPrefab = prefabContainer.BallPrefab;
        }).Run();

        if (PlayerPrefab != null)
        {
            var playerEntity = EntityManager.Instantiate((Entity)PlayerPrefab);
            EntityManager.AddComponentData(playerEntity, new PhysicsVelocity()
            {
                Linear = new Unity.Mathematics.float3(rnd.NextFloat(-10f, 10f), rnd.NextFloat(-10f, 10f), 0)
            });
            EntityManager.AddComponentData(playerEntity, new SetIntertia()
            {
            });

            
            var ballEntity = EntityManager.Instantiate((Entity)BallPrefab);
            EntityManager.AddComponentData(ballEntity, new PhysicsVelocity()
            {
                Linear = new Unity.Mathematics.float3(rnd.NextFloat(-10f, 10f), rnd.NextFloat(-10f, 10f), 0)
            });
            EntityManager.AddComponentData(ballEntity, new SetIntertia()
            {
            });
        }
    }
}


public class PhysicsBodyConstraintSystem : SystemBase
{
    protected override void OnCreate()
    {
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    EndSimulationEntityCommandBufferSystem m_EntityCommandBufferSystem;
    protected override void OnUpdate()
    {

        var CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer();

        Entities.ForEach((ref Entity entity, ref PhysicsMass physicsMass, ref SetIntertia setIntertia) =>
        {
            physicsMass.InverseInertia = new float3(0, 0, 0);
            CommandBuffer.RemoveComponent(entity, typeof(SetIntertia));

        }).WithoutBurst().Run();
    }
}