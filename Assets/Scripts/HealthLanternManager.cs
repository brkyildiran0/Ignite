using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthLanternManager : MonoBehaviour
{
    public List<Transform> lanters;
    public List<Animator> lanternAnimators;
    public List<bool> isLanternBurning;
    public static bool lighten;
    public static bool extinguish;

    void Awake()
    {
        lanters = new List<Transform>();
        lanternAnimators = new List<Animator>();
        isLanternBurning = new List<bool>();

        foreach (Transform child in transform)
        {
            lanters.Add(child);
            lanternAnimators.Add(child.GetComponent<Animator>());
            isLanternBurning.Add(false);
            lighten = false;
            extinguish = false;
        }
    }

    void Update()
    {
        HandleLanternLights();
    }

    public static void AddHP()
    {
        lighten = true;
    }

    public static void LoseHP()
    {
        extinguish = true;
    }

    private void HandleLanternLights()
    {
        if (lighten)
        {
            for (int i = 0; i < 3; i++)
            {
                if (!isLanternBurning[i])
                {
                    lighten = false;
                    lanternAnimators[i].ResetTrigger("HP_Gain");
                    lanternAnimators[i].SetTrigger("HP_Gain");
                    isLanternBurning[i] = true;
                    return;
                }
            }
        }

        if (extinguish)
        {
            for (int i = 0; i < 3; i++)
            {
                if (isLanternBurning[i])
                {
                    extinguish = false;
                    lanternAnimators[i].ResetTrigger("HP_Lose");
                    lanternAnimators[i].SetTrigger("HP_Lose");
                    isLanternBurning[i] = false;
                    return;
                }
            }
        }
    }
}
