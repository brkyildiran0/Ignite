using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMarkerBehavior : MonoBehaviour
{
    private ParticleSystem spawnParticle;

    void Awake()
    {
         spawnParticle = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (!spawnParticle.IsAlive())
        {
            GetComponent<PooledObject>().Finish();
        }
    }
}
