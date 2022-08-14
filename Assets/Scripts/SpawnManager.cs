using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] float markerAppearThreshold;

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

    public static float spawnRate = 5f;
    public int hordePopulation = 1;

    //Controls how many waves are necessary to increment the spawned enemies at once for each spawn position
    public int hordeGrowLimiter = 3;

    private int hordeGrowObserver = 0;

    public PooledObject basicEnemy;
    public PooledObject spawnMarker;

    private void Start()
    {
        SetSpawnBoundaries();
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            DetermineSpawnPositions();

            yield return new WaitForSecondsRealtime(spawnRate);

            ManageHordeGrowth();
        }
    }


    private void DetermineSpawnPositions()
    {
        for (int i = 0; i < hordePopulation; i++)
        {
            randomXaxis_1 = UnityEngine.Random.Range(bottomLeftBorder_1.x, topRightBorder_1.x);
            randomYaxis_1 = UnityEngine.Random.Range(bottomLeftBorder_1.y, topRightBorder_1.y);
            randomSpawnPosition_1 = new Vector3(randomXaxis_1, randomYaxis_1, 0f);

            randomXaxis_2 = UnityEngine.Random.Range(bottomLeftBorder_2.x, topRightBorder_2.x);
            randomYaxis_2 = UnityEngine.Random.Range(bottomLeftBorder_2.y, topRightBorder_2.y);
            randomSpawnPosition_2 = new Vector3(randomXaxis_2, randomYaxis_2, 0f);

            randomXaxis_3 = UnityEngine.Random.Range(bottomLeftBorder_3.x, topRightBorder_3.x);
            randomYaxis_3 = UnityEngine.Random.Range(bottomLeftBorder_3.y, topRightBorder_3.y);
            randomSpawnPosition_3 = new Vector3(randomXaxis_3, randomYaxis_3, 0f);

            randomXaxis_4 = UnityEngine.Random.Range(bottomLeftBorder_4.x, topRightBorder_4.x);
            randomYaxis_4 = UnityEngine.Random.Range(bottomLeftBorder_4.y, topRightBorder_4.y);
            randomSpawnPosition_4 = new Vector3(randomXaxis_4, randomYaxis_4, 0f);


            PooledObject marker_1 = Pool.Instance.Spawn(spawnMarker, randomSpawnPosition_1, Quaternion.identity);
            PooledObject marker_2 = Pool.Instance.Spawn(spawnMarker, randomSpawnPosition_2, Quaternion.identity);
            PooledObject marker_3 = Pool.Instance.Spawn(spawnMarker, randomSpawnPosition_3, Quaternion.identity);
            PooledObject marker_4 = Pool.Instance.Spawn(spawnMarker, randomSpawnPosition_4, Quaternion.identity);


            PooledObject enemy_1 = Pool.Instance.Spawn(basicEnemy, randomSpawnPosition_1, Quaternion.identity);
            PooledObject enemy_2 = Pool.Instance.Spawn(basicEnemy, randomSpawnPosition_2, Quaternion.identity);
            PooledObject enemy_3 = Pool.Instance.Spawn(basicEnemy, randomSpawnPosition_3, Quaternion.identity);
            PooledObject enemy_4 = Pool.Instance.Spawn(basicEnemy, randomSpawnPosition_4, Quaternion.identity);
        }
    }

    private void SetSpawnBoundaries()
    {
        //Bottom Spawn Area
        bottomLeftBorder_1 = new Vector3(-90, -50, 0);
        topRightBorder_1 = new Vector3(90, -35, 0);

        //Left Spawn Area
        bottomLeftBorder_2 = new Vector3(-90, -50, 0);
        topRightBorder_2 = new Vector3(-75, 50, 0);

        //Top Spawn Area
        bottomLeftBorder_3 = new Vector3(-90, 35, 0);
        topRightBorder_3 = new Vector3(90, 50, 0);

        //Right Spawn Area
        bottomLeftBorder_4 = new Vector3(75, -50, 0);
        topRightBorder_4 = new Vector3(90, 50, 0);
    }

    private void ManageHordeGrowth()
    {
        hordeGrowObserver++;

        if (hordeGrowObserver == hordeGrowLimiter)
        {
            hordePopulation++;
            hordeGrowLimiter++;
            spawnRate++;
            hordeGrowObserver = 0;
        }
    }
}
