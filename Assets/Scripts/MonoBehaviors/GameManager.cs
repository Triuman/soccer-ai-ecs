using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Physics;
using Unity.Rendering;

public class GameManager : MonoBehaviour
{
    public Mesh ShipMesh;
    public UnityEngine.Material ShipMaterial;


    public Camera mainCamera;
    public static Camera MainCamera;


    void Awake()
    {
        MainCamera = mainCamera;
    }

    // Start is called before the first frame update
    void Start()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var shipArch = entityManager.CreateArchetype(new ComponentType[] {
            typeof(Translation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld),
            typeof(PhysicMaterial),
            typeof(PhysicsVelocity),
            typeof(PhysicsMass),
            typeof(PhysicsCollider),
        });

        for (int i = 0; i < 0; i++)
        {
            var ship = entityManager.CreateEntity(shipArch);
            entityManager.AddComponentData(ship, new PhysicsVelocity()
            {
                Linear = new Unity.Mathematics.float3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0)
            });

            entityManager.AddSharedComponentData(ship, new RenderMesh()
            {
                mesh = ShipMesh,
                material = ShipMaterial
            });
            entityManager.AddComponentData(ship, new RenderBounds()
            {
                Value = new Unity.Mathematics.AABB()
            });

            // var collider = new CylinderGeometry();
            // // collider.Center = 0f;
            // collider.Radius = 0.2f;
            // collider.BevelRadius = 0.2f;
            // entityManager.AddComponentData(ship, new PhysicsCollider()
            // {
            //     Value = Unity.Physics.CylinderCollider.Create(collider)
            // });
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}
