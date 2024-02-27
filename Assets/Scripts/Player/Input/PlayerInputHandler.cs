using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private Camera cam;

    public Vector2 RawMovementInput { get; private set; }
    public Vector2 RawDashDirectionInput { get; private set; }

    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }

    public bool JumpInput { get; private set; }

    public bool JumpInputStop { get; private set; }

    public bool GrabInput { get; private set; }

    public bool DashInput { get; private set; }

    public bool DashInputStop { get; private set; }

    public bool[] AttackInputs { get; private set; }

    public bool RollInput { get; private set; }

    [SerializeField]
    private float inputHoldTime = 0.2f;
    private float jumpInputStartTime;
    private float dashInputStartTime;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        int countOfAttacks = Enum.GetValues(typeof(CombatInputs)).Length;
        AttackInputs = new bool[countOfAttacks];
        cam = Camera.main;
    }
    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
    }
    public void OnPrimaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInputs[(int)CombatInputs.primary] = true;
        }
        if (context.canceled)
        {
            AttackInputs[(int)CombatInputs.primary] = false;
        }
    }
    public void OnSecondaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInputs[(int)CombatInputs.secondary] = true;
        }
        if (context.canceled)
        {
            AttackInputs[(int)CombatInputs.secondary] = false;
        }
    }
    public void OnMoveInupt(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();

        NormInputX = Mathf.RoundToInt(RawMovementInput.x);
        NormInputY = Mathf.RoundToInt(RawMovementInput.y);
    }
    public void OnRollInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            RollInput = true;
        }
        if (context.canceled)
        {
            RollInput = false;
        }
    }
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jumpInputStartTime = Time.time;
            JumpInput = true;
            JumpInputStop = false;
        }
        if (context.canceled)
        {
            JumpInputStop = true;
        }

    }
    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GrabInput = true;
        }
        if (context.canceled)
        {
            GrabInput = false;
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DashInput = true;
            DashInputStop = false;
            dashInputStartTime = Time.time;
        }
        if (context.canceled)
        {
            DashInputStop = true;
        }

    }
    public void OnDashDirectionInput(InputAction.CallbackContext context)
    {
        RawDashDirectionInput = context.ReadValue<Vector2>();

        if (playerInput.currentControlScheme == "Keyboard")
        {
            RawDashDirectionInput = cam.ScreenToWorldPoint((Vector3)RawDashDirectionInput) - transform.position;
        }
        else if (playerInput.currentControlScheme == "Gamepad")
        {

        }
    }

    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }

    private void CheckDashInputHoldTime()
    {
        if (Time.time >= dashInputStartTime + inputHoldTime)
        {
            DashInput = false;
        }
    }
    public void UseJumpInput() => JumpInput = false;

    public void UseDashInput() => DashInput = false;

    public void UseRollInput() => RollInput = false;

    public void UseAttackInput()
    {
        AttackInputs[(int)CombatInputs.primary] = false;
        AttackInputs[(int)CombatInputs.secondary] = false;
    }
}

public enum CombatInputs
{
    primary,
    secondary
}