using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
     private Vector3 Min;
     private  Vector3 Max;
     private  float _xAxis;
     private  float _yAxis;
     private Vector3 _randomPosition;

    public float spawnRate = 3f;
    public float hordeEnemyCount = 3f;
    public PooledObject projectile;

    private void Start()
    {
         SetRanges();
         StartCoroutine(SpawnUnarmedEnemy());
    }

     private void Update()
     {

     }

     private void SetRanges()
     {
         Min = new Vector3(-90, -50, 0);
         Max = new Vector3(90, 50, 0);
     }

    IEnumerator SpawnUnarmedEnemy()
    {
        while (true)
        {
            for (int i = 0; i < hordeEnemyCount; i++)
            {
                _xAxis = UnityEngine.Random.Range(Min.x, Max.x);
                _yAxis = UnityEngine.Random.Range(Min.y, Max.y);
                _randomPosition = new Vector3(_xAxis, _yAxis, 0f);
                PooledObject instance = Pool.Instance.Spawn(projectile, _randomPosition, Quaternion.identity);
            }
            hordeEnemyCount++;
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
