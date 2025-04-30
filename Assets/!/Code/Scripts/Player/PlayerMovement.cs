using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.5f;
    public float acceleration = 10f;
    public float deceleration = 20f;
    public float quickStartDuration = 0.1f;
    public float quickStartAcceleration = 100f;

    private Rigidbody2D rb;
    private InputAction sprintAction;
    private Vector2 currentInput;
    private Vector2 targetVelocity;
    private Vector2 velocity;

    private float quickStartTimer = 0f;
    private bool wasMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        var moveAction = InputSystem.actions.FindAction("Move");

        if (moveAction != null)
        {
            moveAction.performed += ctx => currentInput = ctx.ReadValue<Vector2>();
            moveAction.canceled += ctx => currentInput = Vector2.zero;
        }

        sprintAction = InputSystem.actions.FindAction("Sprint");
    }

    void FixedUpdate()
    {
        float currentSpeed = moveSpeed;

        if (sprintAction != null && sprintAction.IsPressed())
        {
            currentSpeed *= sprintMultiplier;
        }

        targetVelocity = currentInput * currentSpeed;

        bool isTryingToMove = currentInput.magnitude > 0.01f;

        if (isTryingToMove && !wasMoving)
        {
            quickStartTimer = quickStartDuration;
        }

        wasMoving = isTryingToMove;

        float accelRate;

        if (quickStartTimer > 0)
        {
            accelRate = quickStartAcceleration;
            quickStartTimer -= Time.fixedDeltaTime;
        }
        else
        {
            accelRate = isTryingToMove ? acceleration : deceleration;
        }

        velocity = Vector2.MoveTowards(
            rb.linearVelocity,
            targetVelocity,
            accelRate * Time.fixedDeltaTime
        );
        rb.linearVelocity = velocity;
    }
}
