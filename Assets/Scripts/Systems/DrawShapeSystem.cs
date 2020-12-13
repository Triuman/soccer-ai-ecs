using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

public class DrawShapeSystem : SystemBase
{

    EndSimulationEntityCommandBufferSystem m_EndSimulationEcbSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        // Find the ECB system once and store it for later usage
        m_EndSimulationEcbSystem = World
            .GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

        EntityManager.CreateEntity(new ComponentType[] { typeof(SelectedShapeComponent) });
        SetSingleton<SelectedShapeComponent>(new SelectedShapeComponent() { Value = SelectedShape.Circle });
    }

    protected override void OnStartRunning()
    {
        base.OnStartRunning();

    }



    // if (shapeMesh == null)
    // {
    //     drawingShape = true;
    //     shapeStartPos = userInput.MousePos;
    //     Entity RectPrefab = default;
    //     Entities
    //         .ForEach((in AssetContainer assetContainer) =>
    //         {
    //             RectPrefab = assetContainer.RectPrefab;

    //         })
    //         .WithoutBurst()
    //         .Run();

    //     var rectEntity = EntityManager.Instantiate(RectPrefab);
    //     var prefabRenderMesh = EntityManager.GetSharedComponentData<RenderMesh>(rectEntity);
    //     var renderMesh = new RenderMesh();
    //     shapeMesh = renderMesh.mesh = new Mesh();
    //     renderMesh.material = prefabRenderMesh.material;
    //     EntityManager.SetSharedComponentData<RenderMesh>(rectEntity, renderMesh);
    //     return;
    // }


    bool isDrawing = false;

    protected override void OnUpdate()
    {

        // Get User Input
        InputComponent userInput = GetSingleton<InputComponent>();

        // TEST CODE
        if (isDrawing == false && userInput.MouseDown)
        {
            var assetContainer = GetSingleton<AssetContainer>();
            var rectEntity = EntityManager.Instantiate(assetContainer.RectPrefab);
            var renderMesh = EntityManager.GetSharedComponentData<RenderMesh>(rectEntity);
            EntityManager.AddSharedComponentData<RenderMesh>(rectEntity, new RenderMesh(){
                material = renderMesh.material,
                layer = renderMesh.layer,
                mesh = new Mesh(),
                receiveShadows = false,
                needMotionVectorPass = false
            });
            // EntityManager.AddComponent<DrawingShapeComponent>(rectEntity);
            EntityManager.AddComponentData<DrawingShapeComponent>(rectEntity, new DrawingShapeComponent() { startPos = userInput.MousePos });
            isDrawing = true;
        }
        // TEST CODE END


        // Get Selected Shape
        SelectedShapeComponent selectedShape = GetSingleton<SelectedShapeComponent>();

        // Acquire an ECB and convert it to a concurrent one to be able
        // to use it from a parallel job.
        var ecb = m_EndSimulationEcbSystem.CreateCommandBuffer();

        Entities.ForEach((Entity entity, in RenderMesh renderMesh, in DrawingShapeComponent drawingShapeComponent, in Tag_Rectangle tag_Rectangle, in Tag_Shape tag_Shape) =>
        {
            if (userInput.MouseDown)
            {
                var shapeMesh = renderMesh.mesh;
                var shapeStartPos = drawingShapeComponent.startPos;

                Vector3[] vertices = new Vector3[4];
                vertices[0] = new Vector3(shapeStartPos.x, shapeStartPos.y, -2);
                vertices[1] = new Vector3(userInput.MousePos.x, shapeStartPos.y, -2);
                vertices[2] = new Vector3(userInput.MousePos.x, userInput.MousePos.y, -2);
                vertices[3] = new Vector3(shapeStartPos.x, userInput.MousePos.y, -2);
                shapeMesh.vertices = vertices;

                Vector2[] uv = new Vector2[4];
                uv[0] = new Vector2(shapeStartPos.x, shapeStartPos.y);
                uv[1] = new Vector2(userInput.MousePos.x, shapeStartPos.y);
                uv[2] = new Vector2(userInput.MousePos.x, userInput.MousePos.y);
                uv[3] = new Vector2(shapeStartPos.x, userInput.MousePos.y);
                shapeMesh.uv = uv;

                shapeMesh.triangles = new int[] {
                        0,1,2,2,3,0
                    };
            }
            else
            {
                ecb.RemoveComponent<DrawingShapeComponent>(entity);
                isDrawing = false;
            }
        }).WithoutBurst().Run();

        // Make sure that the ECB system knows about our job
        m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);

        // if (shapeMesh == null)
        // {
        //     drawingShape = true;
        //     shapeStartPos = userInput.MousePos;
        //     Entity RectPrefab = default;
        //     Entities
        //         .ForEach((in AssetContainer assetContainer) =>
        //         {
        //             RectPrefab = assetContainer.RectPrefab;

        //         })
        //         .WithoutBurst()
        //         .Run();

        //     var rectEntity = EntityManager.Instantiate(RectPrefab);
        //     var prefabRenderMesh = EntityManager.GetSharedComponentData<RenderMesh>(rectEntity);
        //     var renderMesh = new RenderMesh();
        //     shapeMesh = renderMesh.mesh = new Mesh();
        //     renderMesh.material = prefabRenderMesh.material;
        //     EntityManager.SetSharedComponentData<RenderMesh>(rectEntity, renderMesh);
        //     return;
        // }
        // // if (math.Equals(rectStartPos, userInput.MousePos))
        // //     return;

        // Vector3[] vertices = new Vector3[4];
        // vertices[0] = new Vector3(shapeStartPos.x, shapeStartPos.y, -2);
        // vertices[1] = new Vector3(userInput.MousePos.x, shapeStartPos.y, -2);
        // vertices[2] = new Vector3(userInput.MousePos.x, userInput.MousePos.y, -2);
        // vertices[3] = new Vector3(shapeStartPos.x, userInput.MousePos.y, -2);
        // shapeMesh.vertices = vertices;

        // Vector2[] uv = new Vector2[4];
        // uv[0] = new Vector2(shapeStartPos.x, shapeStartPos.y);
        // uv[1] = new Vector2(userInput.MousePos.x, shapeStartPos.y);
        // uv[2] = new Vector2(userInput.MousePos.x, userInput.MousePos.y);
        // uv[3] = new Vector2(shapeStartPos.x, userInput.MousePos.y);
        // shapeMesh.uv = uv;

        // shapeMesh.triangles = new int[] {
        //         0,1,2,2,3,0
        //     };

        // else if (drawingShape)
        // {
        //     shapeMesh = null;
        //     drawingShape = false;
        // }

    }
}