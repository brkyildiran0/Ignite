using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float runSpeed = 10.0f;

    Rigidbody2D body;

    float horizontal;
    float vertical;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        GetInput();
        HandleAnimation();
    }

    void FixedUpdate()
    {
        HandleVelocity();
    }

    private void HandleAnimation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            animator.Play("PlayerLeft");
        }
        else if (Input.GetKey(KeyCode.W))
        {
            animator.Play("PlayerBack");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            animator.Play("PlayerFront");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            animator.Play("PlayerRight");
        }
    }

    private void HandleVelocity()
    {
        body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }

    private void GetInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }


}
