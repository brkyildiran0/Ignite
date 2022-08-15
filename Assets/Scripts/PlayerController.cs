using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public LeaderBoard leaderboard;
    public GameObject leaderboardUI;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    [SerializeField] float runSpeed = 10.0f;
    [SerializeField] float floatingSpeed = 2f;
    [SerializeField] float floatingAmount = 1f;
    [SerializeField] Transform playerBodySprite;
    [SerializeField] Animator animator;
    [SerializeField] Animator slashAnimator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hitSFX;
    [SerializeField] AudioClip deadSFX;

    public static int currentHP = 0;
    
    private Rigidbody2D body;
    private CircleCollider2D slashingArea;

    private float horizontal;
    private float vertical;

    private bool restartGame = false;

    private bool locked = false;
    private bool isDeathSequenceTriggered = false;

    //-----------Used to disable during death & respawn-------------//
    [SerializeField] GameObject POOL;
    [SerializeField] GameObject MANAGER_Spawn;
    [SerializeField] GameObject Sword;

    [SerializeField] SpawnManager spawnManager;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        GainHP();
        slashingArea = GetComponentInChildren<CircleCollider2D>();
        //ChangeLeaderboardVisibility(false);
        leaderboardUI.SetActive(false);
    }

    private void ChangeLeaderboardVisibility(bool visible)
    {
        if (visible)
        {
            leaderboardUI.GetComponent<CanvasRenderer>().SetAlpha(1);
            foreach (Transform child in leaderboardUI.transform)
            {
                child.GetComponent<CanvasRenderer>().SetAlpha(1);
            }
            nameText.alpha = 1;
            descriptionText.alpha = 1;
        }
        else
        {
            leaderboardUI.GetComponent<CanvasRenderer>().SetAlpha(0);
            foreach (Transform child in leaderboardUI.transform)
            {
                child.GetComponent<CanvasRenderer>().SetAlpha(0);
            }
            nameText.alpha = 0;
            descriptionText.alpha = 0;
        }
    }

    void Update()
    {
        CheckHPDepleted();
        GetInput();
        HandleAnimation();
    }

    private void CheckHPDepleted()
    {
        if (currentHP == 0 && !isDeathSequenceTriggered)
        {
            isDeathSequenceTriggered = true;
            //ChangeLeaderboardVisibility(true);
            leaderboardUI.SetActive(true);
            Time.timeScale = 0f;

            //Disable the components necessary
            POOL.SetActive(false);
            MANAGER_Spawn.SetActive(false);
            Sword.SetActive(false);
            locked = true;

            audioSource.PlayOneShot(deadSFX, 0.3f); 
            StartCoroutine(DeathSequence());
        }
    }

    public void RestartGame()
    {
        restartGame = true;
    }


    IEnumerator DeathSequence()
    {
        animator.Play("PlayerDeath");

        
        yield return new WaitUntil(() => restartGame);
        yield return leaderboard.SubmitScoreRoutine(ScoreManager.score);

        //ChangeLeaderboardVisibility(false);
        leaderboardUI.SetActive(false);

        restartGame = false;

        slashAnimator.Play("PlayerRevive");
        animator.Play("PlayerFront");
        

        //Reset each element to its initial state.
        MANAGER_Spawn.SetActive(true);
        POOL.SetActive(true);
        Sword.SetActive(true);
        ScoreManager.ResetScore();
        GainHP();

        Time.timeScale = 1;
        isDeathSequenceTriggered = false;
        locked = false;
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
            if (currentHP > 0)
            {
                audioSource.PlayOneShot(hitSFX, 1f);
                StartCoroutine(PlayerHitSequence());
                spawnManager.DecreaseHordeByOne();
            }
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
