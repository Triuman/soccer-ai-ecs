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

        if(userInput.MouseDown){
            if(rectMesh == null){
                rectMesh = new Mesh();
                drawingRect = true;
                rectStartPos = userInput.MousePos;
                return;
            }
            if(math.Equals(rectStartPos, userInput.MousePos))
                return;

            
        }

    }
}