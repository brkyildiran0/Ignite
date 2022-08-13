using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMarkerBehavior : MonoBehaviour
{
    private Animator spawnParticleAnimator; 

    void Awake()
    {
        spawnParticleAnimator = GetComponent<Animator>();
    }
    void Start()
    {
        spawnParticleAnimator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        spawnParticleAnimator.Play("EnemySpawnMarker");
        StartCoroutine(Disabler(SpawnManager.spawnRate));

    }

    IEnumerator Disabler(float duration)
    {
        yield return new WaitForSeconds(duration);

        GetComponent<PooledObject>().Finish();
    }
}
