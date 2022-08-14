using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();    
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
                break;
            case 2:
                slider.maxValue = WeaponManager.secondUpgradeThreshold;
                break;
            case 3:
                slider.maxValue = WeaponManager.thirdUpgradeThreshold;
                break;
            default:
                slider.maxValue = 1000;
                break;
        }
    }
}
