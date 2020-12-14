using Unity.Entities;
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


    bool isDrawing = false;

    protected override void OnUpdate()
    {
        // Get User Input
        InputComponent userInput = GetSingleton<InputComponent>();

        // Get Selected Shape
        SelectedShapeComponent selectedShape = GetSingleton<SelectedShapeComponent>();

        // TEST CODE
        if (isDrawing == false && userInput.MouseDown)
        {
            var assetContainer = GetSingleton<AssetContainer>();
            var shapeEntity = EntityManager.Instantiate(selectedShape.Value == SelectedShape.Rect ? assetContainer.RectPrefab : assetContainer.CirclePrefab);
            var renderMesh = EntityManager.GetSharedComponentData<RenderMesh>(shapeEntity);
            EntityManager.AddSharedComponentData<RenderMesh>(shapeEntity, new RenderMesh()
            {
                material = renderMesh.material,
                layer = renderMesh.layer,
                mesh = new Mesh(),
                receiveShadows = false,
                needMotionVectorPass = false
            });
            EntityManager.AddComponentData<DrawingShapeComponent>(shapeEntity, new DrawingShapeComponent() { startPos = userInput.MousePos });
            isDrawing = true;
        }
        // TEST CODE END


        // Acquire an ECB and convert it to a concurrent one to be able
        // to use it from a parallel job.
        var ecb = m_EndSimulationEcbSystem.CreateCommandBuffer();


        // TODO: As the shapes have same mesh structure, instead of using different prefabs,
        //       create a shader which takes parameters for the shapes, then just change thoese parameters here.


        // Draw Circle || Rectangle
        Entities.ForEach((Entity entity, ref Translation translation, in RenderMesh renderMesh, in DrawingShapeComponent drawingShapeComponent, in Tag_Shape tag_Shape) =>
        {
            if (userInput.MouseDown)
            {
                var shapeMesh = renderMesh.mesh;
                var shapeStartPos = drawingShapeComponent.startPos;

                translation.Value = new float3(shapeStartPos, -2);

                float width = (userInput.MousePos - shapeStartPos).x;
                float heigth = (userInput.MousePos - shapeStartPos).y;

                Vector3[] vertices = new Vector3[4];
                vertices[0] = new Vector3(0, 0, -2);
                vertices[1] = new Vector3(width, 0, -2);
                vertices[2] = new Vector3(width, heigth, -2);
                vertices[3] = new Vector3(0, heigth, -2);
                shapeMesh.vertices = vertices;

                Vector2[] uv = new Vector2[4];
                uv[0] = new Vector2(0, 0);
                uv[1] = new Vector2(1, 0);
                uv[2] = new Vector2(1, 1);
                uv[3] = new Vector2(0, 1);
                shapeMesh.uv = uv;

                shapeMesh.triangles = (width < 0 && heigth < 0) || width > 0 && heigth > 0 ? new int[] {
                       2,1,0,3,2,0
                    } : new int[] {
                        0,1,2,2,3,0
                    };
            }
            else
            {
                ecb.RemoveComponent<DrawingShapeComponent>(entity);
                isDrawing = false;
                SetSingleton<SelectedShapeComponent>(new SelectedShapeComponent() { Value = selectedShape.Value == SelectedShape.Rect ? SelectedShape.Circle : SelectedShape.Rect });
            }
        })
        .WithoutBurst()
        .Run();

        // Make sure that the ECB system knows about our job
        m_EndSimulationEcbSystem.AddJobHandleForProducer(this.Dependency);


    }
}