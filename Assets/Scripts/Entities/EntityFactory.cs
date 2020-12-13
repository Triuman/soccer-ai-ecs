using System;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using Unity.Rendering;

public class EntityFactory
{
    private static EntityFactory instance;
    public static EntityFactory Instance
    {
        get
        {
            if (instance == null)
                instance = new EntityFactory();
            return instance;
        }
    }

    private EntityManager entityManager;

    public EntityFactory()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        if (EntityFactory.instance != null)
            throw new Exception("EntityFactory is initialized more than once.");
    }

    private EntityArchetype PlayerArchetype;
    private EntityArchetype BallArchetype;
    private void CreateArchetypes()
    {
        PlayerArchetype = entityManager.CreateArchetype(PlayerComponents);
        BallArchetype = entityManager.CreateArchetype(BallComponents);
    }


    public Entity CreatePlayerEntity()
    {
        return CreateEntity(PlayerArchetype);
    }
    public Entity CreateBallEntity()
    {
        return CreateEntity(PlayerArchetype);
    }

    private Entity CreateEntity(EntityArchetype archetype)
    {
        return entityManager.CreateEntity(archetype);
    }


    private readonly ComponentType[] PlayerComponents = new ComponentType[] {
            typeof(Tag_Player),

            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld),
            typeof(Translation),

            typeof(PhysicsVelocity),
            typeof(PhysicsMass),
            typeof(PhysicsCollider)
        };

    private readonly ComponentType[] BallComponents = new ComponentType[] {
            typeof(Tag_Ball),

            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld),
            typeof(Translation),

            typeof(PhysicsVelocity),
            typeof(PhysicsMass),
            typeof(PhysicsCollider)
        };

}