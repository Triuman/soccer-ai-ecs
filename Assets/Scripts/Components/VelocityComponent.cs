using Unity.Entities;
using Unity.Mathematics;

public struct VelocityComponent: IComponentData {
    public float3 Value;
}



// player touched the ball +10 fitness
//      player id
//      ball id
//      fitness
//      event: touch

// Touch event
// register Player's onCollisonEnter
// 


// Player Components
/*
    RenderMesh
    LocalToWorld
    Translation

    Velocity
    Collisions entity buffer, type: collision, trigger

    Id
    Tag_Player
    Tag_Home / Tag_Away
    Score
    Brain
    
    MoveTo: Apply to Translation and remove
    SetColor: apply and remove
*/

// Ball Components
/*
    RenderMesh
    LocalToWorld
    Translation
    
    Id
    Tag_Ball
    Velocity
    Collisions entity buffer, type: collision, trigger

    MoveTo: Apply to Translation and remove
    SetColor: apply and remove

*/