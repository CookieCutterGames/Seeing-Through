using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.5f;

    [SerializeField]
    private Rigidbody2D rb;
    private Vector2 targetVelocity;

    private bool IsMoving;
    Animator anim;
    private Vector2 lastMoveDirection;
    private bool facingLeft = false;

    public Action<bool> IsMovingValueChanged;

    private Queue<Vector2> positionHistory = new();
    private Vector2 lastFacingDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate() { }

    void Update()
    {
        float currentSpeed = moveSpeed;

        if (UserInput.Instance.SprintPressed)
        {
            currentSpeed *= sprintMultiplier;
        }

        targetVelocity = UserInput.Instance.MoveInput * currentSpeed;
        IsMoving = targetVelocity != Vector2.zero;
        IsMovingValueChanged.Invoke(IsMoving);
        rb.linearVelocity = targetVelocity;
        UpdateRecentPositions();
        ProcessInputs();
        Animate();
        if (
            UserInput.Instance.MoveInput.x < 0 && !facingLeft
            || UserInput.Instance.MoveInput.x > 0 && facingLeft
        )
            Flip();
    }

    public void UpdateRecentPositions()
    {
        positionHistory.Enqueue(transform.position);
        if (positionHistory.Count > 10)
            positionHistory.Dequeue();

        if (rb.linearVelocity != Vector2.zero)
        {
            lastFacingDirection = rb.linearVelocity.normalized;
        }
    }

    public List<Vector2> GetRecentPositions(int count)
    {
        List<Vector2> result = new List<Vector2>();
        foreach (Vector2 pos in positionHistory)
        {
            if (result.Count >= count)
                break;
            result.Add(pos);
        }
        return result;
    }

    public Vector2 GetFacingDirection()
    {
        return lastFacingDirection;
    }

    public void ProcessInputs()
    {
        var inputs = UserInput.Instance.MoveInput;
        if ((inputs.x == 0 && inputs.y == 0) && (inputs.x != 0 || inputs.y != 0))
        {
            lastMoveDirection = inputs;
        }
    }

    void Animate()
    {
        anim.SetFloat("MoveX", UserInput.Instance.MoveInput.x);
        anim.SetFloat("MoveY", UserInput.Instance.MoveInput.y);
        anim.SetFloat("MoveMagnitude", UserInput.Instance.MoveInput.magnitude);
        anim.SetFloat("LastMoveX", lastMoveDirection.x);
        anim.SetFloat("LastMoveY", lastMoveDirection.y);
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;

        scale.x *= -1;
        transform.localScale = scale;
        facingLeft = !facingLeft;
    }
}
