using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float spawnRate = 3f;
    public PooledObject projectile;


    void Start()
    {
        StartCoroutine(SpawnUnarmedEnemy());
    }

    IEnumerator SpawnUnarmedEnemy()
    {
        while (true)
        {
            PooledObject instance = Pool.Instance.Spawn(projectile, new Vector3(1.2f, 1.2f, 0f), Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
