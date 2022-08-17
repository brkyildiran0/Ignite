using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardUIUpdater : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;

    private void OnEnable()
    {
        playerManager.UpdateScoreboard();
    }
}
