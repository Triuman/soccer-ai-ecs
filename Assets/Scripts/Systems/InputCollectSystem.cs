using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputCollectSystem : SystemBase
{
    private PlayerInputActions playerActions;
    protected override void OnCreate()
    {
        base.OnCreate();

        playerActions = new PlayerInputActions();
        playerActions.Player.Move.performed += Move_performed;
        playerActions.Player.Move.Enable();
        playerActions.Player.Shoot.performed += Shoot_performed;
        playerActions.Player.Shoot.Enable();
        playerActions.Player.ShootCancel.performed += ShootCancel_performed;
        playerActions.Player.ShootCancel.Enable();
        playerActions.User.MouseDown.performed += MouseDown_performed;
        playerActions.User.MouseDown.Enable();
        playerActions.User.MouseMove.performed += MouseMove_performed;
        playerActions.User.MouseMove.Enable();

        EntityManager.CreateEntity(new ComponentType[] { typeof(InputComponent) });
        SetSingleton<InputComponent>(new InputComponent());
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        playerActions.Player.Move.performed -= Move_performed;
        playerActions.Player.Move.Disable();
        playerActions.Player.Shoot.performed -= Shoot_performed;
        playerActions.Player.Shoot.Disable();
        playerActions.Player.ShootCancel.performed -= ShootCancel_performed;
        playerActions.Player.ShootCancel.Disable();
        playerActions.User.MouseDown.performed -= MouseDown_performed;
        playerActions.User.MouseDown.Disable();
        playerActions.User.MouseMove.performed -= MouseMove_performed;
        playerActions.User.MouseMove.Disable();
    }

    private Vector2 moveInput = Vector2.zero;
    private Vector2 mousePos = Vector2.zero;
    private bool mouseDown = false;
    private bool shootInput = false;


    private void Move_performed(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }
    private void Shoot_performed(InputAction.CallbackContext ctx)
    {
        shootInput = true;
    }
    private void ShootCancel_performed(InputAction.CallbackContext ctx)
    {
        shootInput = false;
    }
    void MouseDown_performed(InputAction.CallbackContext ctx)
    {
        mouseDown = ctx.ReadValue<float>() == 1f;
    }

    void MouseMove_performed(InputAction.CallbackContext ctx)
    {
        mousePos = ctx.ReadValue<Vector2>();
        mousePos = GameManager.MainCamera.ScreenToWorldPoint(new float3(mousePos, 0));
    }

    protected override void OnUpdate()
    {
        Entities
            .ForEach((ref InputComponent input) =>
            {
                input.MoveInput = new float2(moveInput.x, moveInput.y);
                input.ShootInput = shootInput;
                input.MouseDown = mouseDown;
                input.MousePos = mousePos;
            })
            .WithoutBurst()
            .Run();
    }
}