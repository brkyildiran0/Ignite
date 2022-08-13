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

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRandomizer = Random.Range(0, 3);
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
