using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using Unity.Mathematics;

public class InputControlSystem : SystemBase
{
    protected override void OnCreate()
    {
        base.OnCreate();
    }

    protected override void OnUpdate()
    {
        InputComponent userInput = GetSingleton<InputComponent>();


        /// Get the physics world.
        // You don't need to do this if you're going to use ComponentExtensions.ApplyLinearImpulse, but I have kept the code in to show you how to do it.
        PhysicsWorld physicsWorld = World.DefaultGameObjectInjectionWorld.GetExistingSystem<Unity.Physics.Systems.BuildPhysicsWorld>().PhysicsWorld;

        float3 playerPos = float3.zero;

        Entities
            .ForEach((
            Entity _entity,
            ref PhysicsVelocity _physicsVelocity,
            ref PhysicsMass _physicsMass,
            ref Translation translation, in ControlledByIUserInput controlledByIUserInput) =>
            {
                if (userInput.MouseDown)
                {
                    translation.Value.x = userInput.MousePos.x;
                    translation.Value.y = userInput.MousePos.y;
                }


                _physicsVelocity.Linear.z = 0;
                /// Apply a linear impulse to the entity.
                PhysicsComponentExtensions.ApplyLinearImpulse(ref _physicsVelocity, _physicsMass, new float3(userInput.MoveInput * 0.03f, 0));
                playerPos = translation.Value;
            })
            .WithoutBurst()
            .Run();

        if (userInput.ShootInput)
        {
            Entities
                .ForEach((
                Entity _entity,
                ref PhysicsVelocity _physicsVelocity,
                ref PhysicsMass _physicsMass,
                in Translation translation, in Tag_Ball tag_ball) =>
                {
                    _physicsVelocity.Linear.z = 0;
                    if (math.distance(new float2(playerPos.x, playerPos.y), new float2(translation.Value.x, translation.Value.y)) < 0.6f)
                    {
                        var shootDir = translation.Value - playerPos;
                        /// Apply a linear impulse to the entity.
                        PhysicsComponentExtensions.ApplyLinearImpulse(ref _physicsVelocity, _physicsMass, shootDir * 1.5f);
                    }
                })
                .WithoutBurst()
                .Run();
        }
    }
}