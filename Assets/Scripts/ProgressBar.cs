using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;
    private Animator animator;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        DetermineSliderLength();
        slider.value = WeaponManager.killCounter;
    }

    private void DetermineSliderLength()
    {
        switch (WeaponManager.weaponLevel)
        {
            case 1:
                slider.maxValue = WeaponManager.firstUpgradeThreshold;
                animator.SetBool("animate", false);
                break;
            case 2:
                slider.maxValue = WeaponManager.secondUpgradeThreshold;
                animator.SetBool("animate", false);
                break;
            case 3:
                slider.maxValue = WeaponManager.thirdUpgradeThreshold;
                animator.SetBool("animate", true);
                break;
            case 4:
                slider.maxValue = 1000;
                animator.SetBool("animate", true);
                break;
        }
    }
}
