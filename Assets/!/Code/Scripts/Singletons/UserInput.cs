using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[RequireComponent(typeof(PlayerInput))]
public class UserInput : MonoBehaviour
{
    public static UserInput Instance { get; private set; }
    private PlayerInput _playerInput;

    #region Mechanics

    public Vector2 MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool Attack1Pressed { get; private set; }
    public bool Attack2Pressed { get; private set; }
    public bool SprintPressed { get; private set; }
    public bool MenuOpenCloseInput { get; private set; }
    public bool InteractionPressed { get; private set; }

    #endregion

    private InputAction _moveAction;
    private InputAction _jumpAction;
    public InputAction _attack1Action;
    public InputAction _attack2Action;
    private InputAction _sprintAction;
    public InputAction _interactAction;
    public InputAction _menuOpenCloseAction;

    public void DisableMovement()
    {
        _moveAction.Disable();
    }

    public void EnableMovement()
    {
        _moveAction.Enable();
    }

    public void DisableAbilites()
    {
        _attack1Action.Disable();
        _attack2Action.Disable();
    }

    public void EnableAbilites()
    {
        _attack1Action.Enable();
        _attack2Action.Enable();
    }

    void SetupInputActions()
    {
        _moveAction = _playerInput.actions["Move"];
        _jumpAction = _playerInput.actions["Jump"];
        _interactAction = _playerInput.actions["Interact"];
        _attack1Action = _playerInput.actions["Attack"];
        _attack2Action = _playerInput.actions["Attack2"];
        _sprintAction = _playerInput.actions["Sprint"];
        _menuOpenCloseAction = _playerInput.actions["TurnPauseMenu"];
    }

    void UpdateInputs()
    {
        MoveInput = _moveAction.ReadValue<Vector2>();
        JumpPressed = _jumpAction.WasPressedThisFrame();
        InteractionPressed = _interactAction.WasPressedThisFrame();
        Attack1Pressed = _attack1Action.WasPressedThisFrame();
        Attack2Pressed = _attack2Action.WasPressedThisFrame();
        SprintPressed = _sprintAction.IsPressed();
        MenuOpenCloseInput = _menuOpenCloseAction.WasPressedThisFrame();
    }

    public InputAction GetActionByName(string actionName)
    {
        return _playerInput.actions[actionName];
    }

    void Update()
    {
        UpdateInputs();
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        _playerInput = GetComponent<PlayerInput>();
        SetupInputActions();
        DisableMovement();
    }
}
