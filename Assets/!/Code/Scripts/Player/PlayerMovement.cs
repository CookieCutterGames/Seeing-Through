using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.5f;

    private Rigidbody2D rb;
    private Vector2 targetVelocity;

    private bool IsMoving;

    public Action<bool> IsMovingValueChanged;

    private Queue<Vector2> positionHistory = new();
    private Vector2 lastFacingDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

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
}
