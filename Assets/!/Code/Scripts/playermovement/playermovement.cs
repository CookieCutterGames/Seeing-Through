using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.5f;
    public LayerMask groundLayer;
    public Transform groundCheck;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float moveX = 0f;
        float moveY = 0f;
        float currentSpeed = moveSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= sprintMultiplier;
        }

        if (Input.GetKey(KeyCode.W)) moveY = 1;
        if (Input.GetKey(KeyCode.S)) moveY = -1;
        if (Input.GetKey(KeyCode.A)) moveX = -1;
        if (Input.GetKey(KeyCode.D)) moveX = 1;

        rb.linearVelocity = new Vector2(moveX * currentSpeed, moveY * currentSpeed);
    }
}