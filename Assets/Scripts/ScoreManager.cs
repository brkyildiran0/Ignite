using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text hiScoreText;

    public static int score = 0;
    private int hiScore = 0;

    private float scoreDivider = 0;

    void Start()
    {
        hiScore = PlayerPrefs.GetInt("highscore", 0);
        scoreText.text = score.ToString();
        hiScoreText.text = hiScore.ToString();
    }

    void Update()
    {
        scoreDivider += Time.deltaTime;

        if (scoreDivider > 0.1f)
        {
            score++;
            scoreText.text = score.ToString();
            scoreDivider = 0;
        }

        if (hiScore < score)
        {
            PlayerPrefs.SetInt("highscore", score);
        }
    }
}
