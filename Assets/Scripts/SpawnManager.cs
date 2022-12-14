using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] PooledObject enemyPooledObject;

    private Vector3 bottomLeftBorder_1;
    private Vector3 topRightBorder_1;
    private float randomXaxis_1;
    private float randomYaxis_1;
    private Vector3 randomSpawnPosition_1;

    private Vector3 bottomLeftBorder_2;
    private Vector3 topRightBorder_2;
    private float randomXaxis_2;
    private float randomYaxis_2;
    private Vector3 randomSpawnPosition_2;

    private Vector3 bottomLeftBorder_3;
    private Vector3 topRightBorder_3;
    private float randomXaxis_3;
    private float randomYaxis_3;
    private Vector3 randomSpawnPosition_3;

    private Vector3 bottomLeftBorder_4;
    private Vector3 topRightBorder_4;
    private float randomXaxis_4;
    private float randomYaxis_4;
    private Vector3 randomSpawnPosition_4;

    //Controls how many waves are necessary to increment the spawned enemies at once for each spawn position
    public float timeBetweenEachWave = 8f;

    public static int numberOfEnemiesToSpawn_dividedByFour = 1;

    private void Awake()
    {
        SetSpawnBoundaries();
    }

    private void OnEnable()
    {
        ResetSpawnParameters();
        StartCoroutine(Spawn());
    }

    private void OnDisable()
    {
        StopCoroutine(Spawn());
    }


    private void ResetSpawnParameters()
    {
        numberOfEnemiesToSpawn_dividedByFour = 1;
    }

    IEnumerator Spawn()
    {
        if (Pool.initializationComplete)
        {
            while (true)
            {
                SpawnAtRandomPositions();
                numberOfEnemiesToSpawn_dividedByFour++;
                yield return new WaitForSeconds(timeBetweenEachWave);
            }
        }
    }


    private void SpawnAtRandomPositions()
    {
        for (int i = 0; i < numberOfEnemiesToSpawn_dividedByFour; i++)
        {
            randomXaxis_1 = UnityEngine.Random.Range(bottomLeftBorder_1.x, topRightBorder_1.x);
            randomYaxis_1 = UnityEngine.Random.Range(bottomLeftBorder_1.y, topRightBorder_1.y);
            randomSpawnPosition_1 = new Vector2(randomXaxis_1, randomYaxis_1);

            randomXaxis_2 = UnityEngine.Random.Range(bottomLeftBorder_2.x, topRightBorder_2.x);
            randomYaxis_2 = UnityEngine.Random.Range(bottomLeftBorder_2.y, topRightBorder_2.y);
            randomSpawnPosition_2 = new Vector2(randomXaxis_2, randomYaxis_2);

            randomXaxis_3 = UnityEngine.Random.Range(bottomLeftBorder_3.x, topRightBorder_3.x);
            randomYaxis_3 = UnityEngine.Random.Range(bottomLeftBorder_3.y, topRightBorder_3.y);
            randomSpawnPosition_3 = new Vector2(randomXaxis_3, randomYaxis_3);

            randomXaxis_4 = UnityEngine.Random.Range(bottomLeftBorder_4.x, topRightBorder_4.x);
            randomYaxis_4 = UnityEngine.Random.Range(bottomLeftBorder_4.y, topRightBorder_4.y);
            randomSpawnPosition_4 = new Vector2(randomXaxis_4, randomYaxis_4);

            PooledObject enemy_1 = Pool.Instance.Spawn(enemyPooledObject, randomSpawnPosition_1, Quaternion.identity);
            PooledObject enemy_2 = Pool.Instance.Spawn(enemyPooledObject, randomSpawnPosition_2, Quaternion.identity);
            PooledObject enemy_3 = Pool.Instance.Spawn(enemyPooledObject, randomSpawnPosition_3, Quaternion.identity);
            PooledObject enemy_4 = Pool.Instance.Spawn(enemyPooledObject, randomSpawnPosition_4, Quaternion.identity);

            //Power-up enemy relocation
            float randomXaxis_5 = UnityEngine.Random.Range(-80f, 80f);
            float randomYaxis_5 = UnityEngine.Random.Range(-37.5f, 37.5f);

            if (enemy_1.GetComponent<EnemyBehavior>().isPowerup)
            {
                enemy_1.transform.position = new Vector2(randomXaxis_5, randomYaxis_5);
            }
            else if (enemy_2.GetComponent<EnemyBehavior>().isPowerup)
            {
                enemy_2.transform.position = new Vector2(randomXaxis_5, randomYaxis_5);
            }
            else if (enemy_3.GetComponent<EnemyBehavior>().isPowerup)
            {
                enemy_3.transform.position = new Vector2(randomXaxis_5, randomYaxis_5);
            }
            else if (enemy_4.GetComponent<EnemyBehavior>().isPowerup)
            {
                enemy_4.transform.position = new Vector2(randomXaxis_5, randomYaxis_5);
            }
        }
    }

    private void SetSpawnBoundaries()
    {
        //Bottom Spawn Area
        bottomLeftBorder_1 = new Vector3(-140, -90, 0);
        topRightBorder_1 = new Vector3(140, -60, 0);

        //Left Spawn Area
        bottomLeftBorder_2 = new Vector3(-140, -60, 0);
        topRightBorder_2 = new Vector3(-110, 60, 0);

        //Top Spawn Area
        bottomLeftBorder_3 = new Vector3(-140, 60, 0);
        topRightBorder_3 = new Vector3(140, 90, 0);

        //Right Spawn Area
        bottomLeftBorder_4 = new Vector3(110, -60, 0);
        topRightBorder_4 = new Vector3(140, 60, 0);
    }

    public static void DecreaseAmountOfEnemiesToBeSpawnedByOne()
    {
        if (numberOfEnemiesToSpawn_dividedByFour > 5)
        {
            numberOfEnemiesToSpawn_dividedByFour--;
        }
    }
}
