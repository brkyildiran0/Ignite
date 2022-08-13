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

    // Update is called once per frame
    void Update()
    {
        if (!spawnParticle.IsAlive())
        {
            GetComponent<PooledObject>().Finish();
        }
    }
}
