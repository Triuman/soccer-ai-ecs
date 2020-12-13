using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

public class DrawShapeSystem : SystemBase
{

    bool drawingRect = false;
    float2 rectStartPos;
    Mesh rectMesh;

    protected override void OnUpdate()
    {
        InputComponent userInput = new InputComponent();

        Entities
            .ForEach((in InputComponent input) =>
            {
                userInput = input;

            })
            .WithoutBurst()
            .Run();

        if (userInput.MouseDown)
        {
            if (rectMesh == null)
            {
                drawingRect = true;
                rectStartPos = userInput.MousePos;
                Entity RectPrefab = default;
                Entities
                    .ForEach((in AssetContainer assetContainer) =>
                    {
                        RectPrefab = assetContainer.RectPrefab;

                    })
                    .WithoutBurst()
                    .Run();

                var rectEntity = EntityManager.Instantiate(RectPrefab);
                var prefabRenderMesh = EntityManager.GetSharedComponentData<RenderMesh>(rectEntity);
                var renderMesh = new RenderMesh();
                rectMesh = renderMesh.mesh = new Mesh();
                renderMesh.material = prefabRenderMesh.material;
                EntityManager.SetSharedComponentData<RenderMesh>(rectEntity, renderMesh);
                return;
            }
            // if (math.Equals(rectStartPos, userInput.MousePos))
            //     return;

            Vector3[] vertices = new Vector3[4];

            vertices[0] = new Vector3(rectStartPos.x, rectStartPos.y, -2);
            vertices[1] = new Vector3(userInput.MousePos.x, rectStartPos.y, -2);
            vertices[2] = new Vector3(userInput.MousePos.x, userInput.MousePos.y, -2);
            vertices[3] = new Vector3(rectStartPos.x, userInput.MousePos.y, -2);

            rectMesh.vertices = vertices;
            rectMesh.triangles = new int[] {
                    0,1,2,2,3,0
                };

        }
        else if (drawingRect)
        {
            rectMesh = null;
            drawingRect = false;
        }

    }
}