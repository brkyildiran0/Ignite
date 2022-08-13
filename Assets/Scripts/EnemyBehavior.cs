using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] float enemyMovementSpeed;
    [SerializeField] Sprite enemySprite1;
    [SerializeField] Sprite enemySprite2;
    [SerializeField] Sprite enemySprite3;

    private Transform player;
    private SpriteRenderer spriteRenderer;
    private int spriteRandomizer = 0;
    private int mirrorRandomizer = 0;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRandomizer = Random.Range(0, 3);
        mirrorRandomizer = Random.Range(0, 2);
    }

    private void OnEnable()
    {
        RandomizeSprite();
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

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, enemyMovementSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Weapon")
        {
            GetComponent<PooledObject>().Finish();
        }

        if (collision.tag == "Player")
        {
            //TODO, game over
        }
    }
}
