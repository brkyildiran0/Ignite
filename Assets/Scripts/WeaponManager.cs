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
    private BoxCollider2D swordCollider;
    private float remainingProtectionDuration;
    private bool isProtected = false;

    void Start()
    {
        swordAnimator = GetComponentInChildren<Animator>();
        swordCollider = GetComponent<BoxCollider2D>();
        remainingProtectionDuration = weaponProtectionDurationInSeconds;
    }

    void Update()
    {
        HandleProtection();
        HandleWeaponUpgrade();
        HandleWeaponDowngrade();
        HandleWeaponHitbox();

        print(killCounter);
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
                break;
            case 2:
                swordCollider.offset = new Vector2(0f, -10f);
                swordCollider.size = new Vector2(4f, 31f);
                break;
            case 3:
                swordCollider.offset = new Vector2(0f, -6f);
                swordCollider.size = new Vector2(4.5f, 39f);
                break;
            case 4:
                swordCollider.offset = new Vector2(0f, -0.4f);
                swordCollider.size = new Vector2(4.5f, 50f);
                break;
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