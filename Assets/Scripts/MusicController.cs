using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] AudioSource NormalBGM;
    [SerializeField] AudioSource DeadBGM;

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.currentHP == 0)
        {
            NormalBGM.mute = true;
            DeadBGM.mute = false;
        }
        else
        {
            NormalBGM.mute = false;
            DeadBGM.mute = true;
        }
    }
}
