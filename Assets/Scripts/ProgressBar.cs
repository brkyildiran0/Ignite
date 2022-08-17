using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] WeaponManager weaponManager;

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
        SetSliderMaxValue();
        SetSliderCurrentValue();
    }

    private void SetSliderCurrentValue()
    {
        slider.value = WeaponManager.killCounter;
    }

    private void SetSliderMaxValue()
    {
        switch (weaponManager.weaponLevel)
        {
            case 1:
                slider.maxValue = weaponManager.firstUpgradeThreshold;
                animator.SetBool("animate", false);
                break;
            case 2:
                slider.maxValue = weaponManager.secondUpgradeThreshold;
                animator.SetBool("animate", false);
                break;
            case 3:
                slider.maxValue = weaponManager.thirdUpgradeThreshold;
                animator.SetBool("animate", true);
                break;
            case 4:
                slider.maxValue = 1000;
                animator.SetBool("animate", true);
                break;
        }
    }
}
