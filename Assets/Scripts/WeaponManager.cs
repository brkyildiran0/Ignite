using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] Transform child;
    [SerializeField] float weaponProtectionDurationInSeconds = 4f;
    [SerializeField] float protectionDecaySpeed = 2f;
    [SerializeField] float decayAmountPerFixedTime = 0.2f;
    [SerializeField] float decayIncreaseAmountBaseValue = 0.02f;

    [SerializeField] float levelOneDecayIncrease = 0.02f;
    [SerializeField] float levelTwoDecayIncrease = 0.021f;
    [SerializeField] float levelThreeDecayIncrease = 0.022f;
    [SerializeField] float levelFourDecayIncrease = 0.023f;

    [Range(-1, 10000)]public static float killCounter = 0;
    public static int weaponLevel = 1;
    public static int firstUpgradeThreshold = 100;
    public static int secondUpgradeThreshold = 250;
    public static int thirdUpgradeThreshold = 425;

    private Animator swordAnimator;
    private BoxCollider2D swordCollider;
    private float remainingProtectionDuration;
    private bool isProtected = false;
    private float decayIncreaseAmountUpdated;

    void Start()
    {
        swordAnimator = GetComponentInChildren<Animator>();
        swordCollider = GetComponent<BoxCollider2D>();
        remainingProtectionDuration = weaponProtectionDurationInSeconds;
        decayIncreaseAmountUpdated = decayIncreaseAmountBaseValue;
    }

    void Update()
    {
        HandleProtection();
        LimitDecayRate();
        HandleWeaponUpgrade();
        HandleWeaponDowngrade();
        HandleWeaponHitbox();
    }

    private void LimitDecayRate()
    {
        if (decayAmountPerFixedTime > 0.35f)
        {
            decayAmountPerFixedTime = 0.35f;
        }
    }

    /*
 *                       offset         size
 * Weapon 1 Dimensions:  0,-13.5        4,24
 * Weapon 2 Dimensions:  0,-10          4,31
 * Weapon 3 Dimensions:  0,-6           4.5,39
 * Weapon 4 Dimensions:  0,-0.4         4.5,50
 */

    private void HandleWeaponHitbox()
    {
        switch (weaponLevel)
        {
            case 1:
                swordCollider.offset = new Vector2(0, -13.5f);
                swordCollider.size = new Vector2(4, 24);
                decayIncreaseAmountUpdated = levelOneDecayIncrease;
                break;
            case 2:
                swordCollider.offset = new Vector2(0f, -10f);
                swordCollider.size = new Vector2(4f, 31f);
                decayIncreaseAmountUpdated = levelTwoDecayIncrease;
                break;
            case 3:
                swordCollider.offset = new Vector2(0f, -6f);
                swordCollider.size = new Vector2(4.5f, 39f);
                decayIncreaseAmountUpdated = levelThreeDecayIncrease;
                break;
            case 4:
                swordCollider.offset = new Vector2(0f, -0.4f);
                swordCollider.size = new Vector2(4.5f, 50f);
                decayIncreaseAmountUpdated = levelFourDecayIncrease;
                break;
        }
    }

    void FixedUpdate()
    {
        if (!isProtected)
        {
            decayAmountPerFixedTime += decayIncreaseAmountUpdated;
            killCounter = killCounter - decayAmountPerFixedTime;
        }
    }

    private void HandleWeaponDowngrade()
    {
        if (killCounter < 0 && weaponLevel != 1)
        {
            weaponLevel--;
            killCounter = 0;
            swordAnimator.ResetTrigger("downgrade");
            swordAnimator.SetTrigger("downgrade");
            isProtected = true;
            remainingProtectionDuration = weaponProtectionDurationInSeconds;
            decayAmountPerFixedTime = 0f;
        }
    }

    private void HandleWeaponUpgrade()
    {
        if (weaponLevel == 1 && killCounter > firstUpgradeThreshold)
        {
            weaponLevel++;
            killCounter = 0;
            swordAnimator.ResetTrigger("upgrade");
            swordAnimator.SetTrigger("upgrade");
            //Handle rigidbody change
            isProtected = true;
            remainingProtectionDuration = weaponProtectionDurationInSeconds;
            decayAmountPerFixedTime = 0f;
        }
        else if (weaponLevel == 2 && killCounter > secondUpgradeThreshold)
        {
            weaponLevel++;
            killCounter = 0;
            swordAnimator.ResetTrigger("upgrade");
            swordAnimator.SetTrigger("upgrade");
            //Handle rigidbody change
            isProtected = true;
            remainingProtectionDuration = weaponProtectionDurationInSeconds;
            decayAmountPerFixedTime = 0f;
        }
        else if (weaponLevel == 3 && killCounter > thirdUpgradeThreshold)
        {
            weaponLevel++;
            killCounter = 0;
            swordAnimator.ResetTrigger("upgrade");
            swordAnimator.SetTrigger("upgrade");
            //Handle rigidbody change
            isProtected = true;
            remainingProtectionDuration = weaponProtectionDurationInSeconds;
            decayAmountPerFixedTime = 0f;
        }
    }

    private void HandleProtection()
    {
        if (remainingProtectionDuration > 0f)
        {
            isProtected = true;
            remainingProtectionDuration -= Time.deltaTime * protectionDecaySpeed;
        }
        else
        {
            isProtected = false;
        }
    }
}
