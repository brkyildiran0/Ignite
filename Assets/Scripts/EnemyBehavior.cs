using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] float enemyMovementSpeed;
    [SerializeField] Sprite enemySprite1;
    [SerializeField] Sprite enemySprite2;
    [SerializeField] Sprite enemySprite3;
    [SerializeField] Sprite cauldronEnemySprite;
    [SerializeField] int cauldronSpawnChancePercentage = 10;
    [SerializeField] float cauldronEnemyKnockbackForce = 10f;
    [SerializeField] float swordKnockbackForce = 10f;

    private Transform player;
    private SpriteRenderer spriteRenderer;
    private int spriteRandomizer = 0;
    private int mirrorRandomizer = 0;
    private int cauldronRandomizer = 0;
    private bool isCauldron = false;
    private bool rigidbodyExists = false;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRandomizer = Random.Range(0, 3);
        mirrorRandomizer = Random.Range(0, 2);
        cauldronRandomizer = Random.Range(0, 10);
    }

    private void OnEnable()
    {
        RandomizeSprite();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, enemyMovementSpeed * Time.deltaTime);
    }

    IEnumerator RemoveRigidbodyWithDelay()
    {
        yield return new WaitForSeconds(0.5f);

        //Change enemy to normal
        isCauldron = false;

        //Remove rigidbody
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        GetComponent<BoxCollider2D>().isTrigger = true;
        rigidbodyExists = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Weapon" && isCauldron)
        {
            //Physics
            GetComponent<Rigidbody2D>().AddForceAtPosition(GetComponent<Rigidbody2D>().velocity * cauldronEnemyKnockbackForce, collision.transform.position);
            Rigidbody2D swordRb = collision.gameObject.GetComponent<Rigidbody2D>();
            swordRb.AddForceAtPosition(-swordRb.velocity * swordKnockbackForce, collision.transform.position);

            switch (spriteRandomizer)
            {
                case 0:
                    spriteRenderer.sprite = enemySprite1;
                    break;
                case 1:
                    spriteRenderer.sprite = enemySprite2;
                    break;
                case 2:
                    spriteRenderer.sprite = enemySprite3;
                    break;
            }

            StartCoroutine(RemoveRigidbodyWithDelay());
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Weapon" && !isCauldron)
        {
            GetComponent<PooledObject>().Finish();
        }

        if (collision.tag == "Player")
        {
            //TODO, lose HP
        }
    }

    private void RandomizeSprite()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        switch (spriteRandomizer)
        {
            case 0:
                spriteRenderer.sprite = enemySprite1;
                break;
            case 1:
                spriteRenderer.sprite = enemySprite2;
                break;
            case 2:
                spriteRenderer.sprite = enemySprite3;
                break;
        }

        //Chance for the enemies to be shielded
        if (cauldronRandomizer == 0)
        {
            spriteRenderer.sprite = cauldronEnemySprite;
            isCauldron = true;

            //Add rigidbody to cauldron enemies to activate knockback effects
            if (!rigidbodyExists)
            {
                gameObject.AddComponent<Rigidbody2D>();
                GetComponent<Rigidbody2D>().freezeRotation = true;
                GetComponent<BoxCollider2D>().isTrigger = false;
                rigidbodyExists = true;
            }
        }

        switch (mirrorRandomizer)
        {
            case 0:
                spriteRenderer.flipX = true;
                break;
            case 1:
                spriteRenderer.flipX = false;
                break;
        }
    }
}
