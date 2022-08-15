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
    [SerializeField] Animator slashAnimator;

    public static int currentHP = 0;
    
    private Rigidbody2D body;
    private CircleCollider2D slashingArea;

    private float horizontal;
    private float vertical;

    private bool locked = false;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        GainHP();
        slashingArea = GetComponentInChildren<CircleCollider2D>();
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
        float floatingLimiter = Mathf.PingPong(Time.time * floatingSpeed, floatingAmount);
        playerBodySprite.localPosition = new Vector3(0, floatingLimiter, 0);

        if (!locked)
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

    public void GainHP()
    {
        if (currentHP < 3)
        {
            currentHP++;
            HealthLanternManager.AddHP();
        }
    }

    public void LoseHP()
    {
        if (currentHP > 0)
        {
            currentHP--;
            HealthLanternManager.LoseHP();
            StartCoroutine(PlayerHitSequence());
        }
    }

    IEnumerator PlayerHitSequence()
    {
        Time.timeScale = 0.2f;
        locked = true;
        animator.Play("PlayerHit");
        slashingArea.enabled = true;
        slashAnimator.Play("SlashAround");
        yield return new WaitForSecondsRealtime(1f);
        slashingArea.enabled = false;
        Time.timeScale = 1;
        locked = false;    
    }
}
