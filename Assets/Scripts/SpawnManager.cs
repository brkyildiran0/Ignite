using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private Vector3 bottomLeftBorder;
    private Vector3 topRightBorder;
    private float randomXaxis;
    private float randomYaxis;
    private Vector3 randomSpawnPosition;


    public float spawnRate = 5f;
    public int hordePopulation = 3;
    public int hordeGrowObserver = 0;

    public PooledObject projectile;

    private void Start()
    {
        SetSpawnBoundaries();
        StartCoroutine(SpawnUnarmedEnemy());
    }

     private void SetSpawnBoundaries()
     {
         bottomLeftBorder = new Vector3(-90, -50, 0);
         topRightBorder = new Vector3(90, 50, 0);
     }

    IEnumerator SpawnUnarmedEnemy()
    {
        while (true)
        {
            for (int i = 0; i < hordePopulation; i++)
            {
                randomXaxis = UnityEngine.Random.Range(bottomLeftBorder.x, topRightBorder.x);
                randomYaxis = UnityEngine.Random.Range(bottomLeftBorder.y, topRightBorder.y);
                randomSpawnPosition = new Vector3(randomXaxis, randomYaxis, 0f);
                PooledObject instance = Pool.Instance.Spawn(projectile, randomSpawnPosition, Quaternion.identity);
            }
            if (hordeGrowObserver == 2)
            {
                hordePopulation++;
                hordeGrowObserver = 0;
            }
            hordeGrowObserver++;
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
