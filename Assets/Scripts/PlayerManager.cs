using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public LeaderBoard leaderboard;
    public TMP_InputField playerNameInputField;
    public bool sessionStarted = false;

    void Start()
    {
        StartCoroutine(SetupRoutine());
    }

    public void SetPlayerName()
    {
        LootLockerSDKManager.SetPlayerName(playerNameInputField.text, (response) =>
        {
            if (response.success)
            {
                print("Set player name successful");
            }
            else
            {
                print("Player name set failed: " + response.Error);
            }
        });
    }

    public void UpdateScoreboard()
    {
        if (sessionStarted)
        {
            StartCoroutine(leaderboard.FetchTopHighscoresRoutine());
        }
    }

    IEnumerator SetupRoutine()
    {
        yield return LoginRoutine();
        yield return leaderboard.FetchTopHighscoresRoutine();
    }

    IEnumerator LoginRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                print("Player logged in.");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
                sessionStarted = true;
            }
            else
            {
                print("Session start failed." + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
}
