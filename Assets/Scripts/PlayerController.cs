using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float runSpeed = 10.0f;
    [SerializeField] float floatingSpeed = 2f;
    [SerializeField] float floatingAmount = 1f;
    [SerializeField] Transform playerBodySprite;
    [SerializeField] Animator animator;

    public static int currentHP = 0;
    
    private Rigidbody2D body;

    private float horizontal;
    private float vertical;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        GainHP();
    }

    void Update()
    {
        GetInput();
        HandleAnimation();
        HPCheck();
    }

    void FixedUpdate()
    {
        HandleVelocity();
    }

    private void HandleAnimation()
    {
        float floatingLimiter = Mathf.PingPong(Time.time * floatingSpeed, floatingAmount);
        playerBodySprite.localPosition = new Vector3(0, floatingLimiter, 0);

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

    private void HPCheck()
    {
        if (currentHP == 0)
        {
            //Death, press any button to trigger "revive" sequence
            //Time.timeScale = 0.0001f;
        }
    }

    public static void GainHP()
    {
        if (currentHP < 3)
        {
            currentHP++;
            HealthLanternManager.AddHP();
        }
    }

    public static void LoseHP()
    {
        //Play getting hit animation

        //Disable input for 0.3f;

        //Kill nearby enemies 


        if (currentHP > 0)
        {
            currentHP--;
            HealthLanternManager.LoseHP();
        }
    }
}
