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

    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] float runSpeed = 10.0f;
    [SerializeField] float floatingSpeed = 2f;
    [SerializeField] float floatingAmount = 1f;
    [SerializeField] Transform playerBodySprite;
    [SerializeField] Animator animator;
    [SerializeField] Animator slashAnimator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hitSFX;
    [SerializeField] AudioClip deadSFX;
    [SerializeField] AudioClip reviveSFX;

    public static int currentHP = 0;
    
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private CircleCollider2D slashingArea;

    private float horizontal;
    private float vertical;

    private bool restartGame = false;

    private bool locked = false;
    private bool isDeathSequenceTriggered = false;

    private bool hitSequenceComplete = false;
    private bool hitSequenceOngoing = false;

    //-----------Used to disable during death & respawn-------------//
    [SerializeField] GameObject POOL;
    [SerializeField] GameObject MANAGER_Spawn;
    [SerializeField] GameObject Sword;

    [SerializeField] SpawnManager spawnManager;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        GainHP();
        slashingArea = GetComponentInChildren<CircleCollider2D>();
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
        audioSource.PlayOneShot(reviveSFX, 0.5f);
        

        //Reset each element to its initial state.
        MANAGER_Spawn.SetActive(true);
        POOL.SetActive(true);
        Sword.SetActive(true);
        ScoreManager.ResetScore();
        GainHP();

        Time.timeScale = 1;
        isDeathSequenceTriggered = false;
        locked = false;
        boxCollider.enabled = true;
        hitSequenceOngoing = false;
        hitSequenceComplete = true;
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
                playerBodySprite.rotation = Quaternion.Euler(0, 0, 10);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                animator.Play("PlayerBack");
                playerBodySprite.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                animator.Play("PlayerFront");
                playerBodySprite.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                animator.Play("PlayerRight");
                playerBodySprite.rotation = Quaternion.Euler(0, 0, -10);
            }
            else
            {
                playerBodySprite.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else
        {
            playerBodySprite.rotation = Quaternion.Euler(0, 0, 0);
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
        if (currentHP < 2)
        {
            currentHP++;
            HealthLanternManager.AddHP();
        }
    }

    public void LoseHP()
    {
        if (currentHP > 0 && !hitSequenceOngoing)
        {
            hitSequenceOngoing = true;

            //Decrease HP by 1
            currentHP--;
            HealthLanternManager.LoseHP();

            hitSequenceComplete = false;

            //Check whether the player died after HP loss
            if (currentHP != 0)
            {
                boxCollider.enabled = false;
                StartCoroutine(PlayerHitSequence());
            }

            StartCoroutine(WaitUntilHitSequenceEnds());
        }
    }


    IEnumerator WaitUntilHitSequenceEnds()
    {
        yield return new WaitUntil(() => hitSequenceComplete);
        yield return new WaitForSeconds(0.04f);
        boxCollider.enabled = true;
        slashingArea.enabled = false;
        locked = false;
        hitSequenceOngoing = false;
    }

    IEnumerator PlayerHitSequence()
    {
        locked = true;
        audioSource.PlayOneShot(hitSFX, 1f);
        slashingArea.enabled = true;

        Time.timeScale = 0f;
        locked = true;
        animator.Play("PlayerHit");
        slashAnimator.Play("SlashAround");
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1;
        yield return new WaitForSecondsRealtime(0.1f);

        animator.Play("PlayerFront");
        hitSequenceComplete = true;
    }
}
