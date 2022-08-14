using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] Transform child;
    [SerializeField] int firstUpgradeThreshold = 100;
    [SerializeField] int secondUpgradeThreshold = 250;
    [SerializeField] int thirdUpgradeThreshold = 500;
    [SerializeField] float weaponProtectionDurationInSeconds = 4f;
    [SerializeField] float protectionDecaySpeed = 2f;
    [SerializeField] float decayAmountPerFixedTime = 0.2f;
    [SerializeField] float decayIncreaseAmount = 0.02f;

    [Range(-1, 10000)]public static float killCounter = 0;
    public static int weaponLevel = 1;

    private Animator swordAnimator;
    private float remainingProtectionDuration;
    private bool isProtected = false;

    void Start()
    {
        swordAnimator = GetComponentInChildren<Animator>();
        remainingProtectionDuration = weaponProtectionDurationInSeconds;
    }

    void Update()
    {
        HandleProtection();
        HandleWeaponUpgrade();
        HandleWeaponDowngrade();

        print(killCounter);
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

    void FixedUpdate()
    {
        if (!isProtected)
        {
            decayAmountPerFixedTime += decayIncreaseAmount;
            killCounter = killCounter - decayAmountPerFixedTime;
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
            //Handle hitbox change
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
            //Handle hitbox change
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
            //Handle hitbox change
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
