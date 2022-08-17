using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip weaponLevelUpSFX;

    [SerializeField] Transform child;
    [SerializeField] Animator swordAnimator;

    [SerializeField] SpawnManager spawner;

    public static float killCounter = 0;
    public int weaponLevel = 1;

    [SerializeField] float maxDecayProtectionDuration = 4f;
    [SerializeField] float decayProtectionDecrementRate = 1f;

    [SerializeField] float currentDecayPerFixedTime = 0.2f;
    [SerializeField] float initialDecayPerFixedTime = 0.2f;
    [SerializeField] float decayIncreasePerFixedTime = 0.0002f;
    [SerializeField] float maxDecayRatePerFixedTime = 0.35f;

    private float remainingDecayProtectionDuration;
    private bool isProtected = true;

    public int firstUpgradeThreshold = 50;
    public int secondUpgradeThreshold = 200;
    public int thirdUpgradeThreshold = 350;

    private BoxCollider2D swordCollider;

    private void Awake()
    {
        swordCollider = GetComponent<BoxCollider2D>();
        remainingDecayProtectionDuration = maxDecayProtectionDuration;
    }

    private void OnEnable()
    {
        ResetSword();
        TriggerWeaponDecayProtection();
    }

    void Update()
    {
        ProtectKillCounter();
        HandleDecayProtection();
        HandleWeaponChange();
        print(killCounter);
    }

    private void ProtectKillCounter()
    {
        if (killCounter < 0)
        {
            killCounter = -1;
        }
    }

    void FixedUpdate()
    {
        DecayWeapon();
    }

    private void HandleWeaponChange()
    {
        HandleWeaponUpgradeAndDowngrade();
        AdjustWeaponProperties();
    }

    private void DecayWeapon()
    {
        if (!isProtected)
        {
            if (currentDecayPerFixedTime < maxDecayRatePerFixedTime)
            {
                currentDecayPerFixedTime += decayIncreasePerFixedTime;
            }
            killCounter -= currentDecayPerFixedTime;
        }
    }

    private void HandleDecayProtection()
    {
        if (remainingDecayProtectionDuration > 0f)
        {
            isProtected = true;
            remainingDecayProtectionDuration -= Time.deltaTime * decayProtectionDecrementRate;
        }
        else
        {
            isProtected = false;
        }
    }

    public void ResetSword()
    {
        weaponLevel = 1;
        killCounter = 1;
        AdjustWeaponProperties();
        TriggerWeaponDecayProtection();
    }

    private void HandleWeaponUpgradeAndDowngrade()
    {
        if (weaponLevel == 1 && killCounter > firstUpgradeThreshold)
        {
            UpgradeWeaponAndGiveScore(200);
            TriggerWeaponDecayProtection();

            currentDecayPerFixedTime = initialDecayPerFixedTime;
            audioSource.PlayOneShot(weaponLevelUpSFX, 0.4f);
        }
        else if (weaponLevel == 2 && killCounter > secondUpgradeThreshold)
        {
            UpgradeWeaponAndGiveScore(500);
            TriggerWeaponDecayProtection();

            currentDecayPerFixedTime = initialDecayPerFixedTime;
            audioSource.PlayOneShot(weaponLevelUpSFX, 0.4f);
        }
        else if (weaponLevel == 3 && killCounter > thirdUpgradeThreshold)
        {
            UpgradeWeaponAndGiveScore(1250);
            TriggerWeaponDecayProtection();

            currentDecayPerFixedTime = initialDecayPerFixedTime;
            audioSource.PlayOneShot(weaponLevelUpSFX, 0.4f);
        }
        //DOWNGRADE
        else if (killCounter < 0 && weaponLevel != 1)
        {
            DowngradeWeapon();
            TriggerWeaponDecayProtection();

            currentDecayPerFixedTime = initialDecayPerFixedTime;
        }
    }

    private void DowngradeWeapon()
    {
        weaponLevel--;
        killCounter = 1;
        swordAnimator.ResetTrigger("downgrade");
        swordAnimator.SetTrigger("downgrade");
    }

    private void UpgradeWeaponAndGiveScore(int pointContribution)
    {
        weaponLevel++;
        killCounter = 1;
        swordAnimator.ResetTrigger("upgrade");
        swordAnimator.SetTrigger("upgrade");
        ScoreManager.score += pointContribution;
    }

    private void AdjustWeaponProperties()
    {
        switch (weaponLevel)
        {
            case 1:
                swordCollider.offset = new Vector2(0, -10.65f);
                swordCollider.size = new Vector2(4, 18.3f);
                swordAnimator.Play("Sword1");
                break;
            case 2:
                swordCollider.offset = new Vector2(0f, -7.38f);
                swordCollider.size = new Vector2(4f, 24.83f);
                swordAnimator.Play("Sword2");
                break;
            case 3:
                swordCollider.offset = new Vector2(-0.14f, -3.10f);
                swordCollider.size = new Vector2(4.7f, 32.64f);
                swordAnimator.Play("Sword3");
                break;
            case 4:
                swordCollider.offset = new Vector2(-0.14f, 2.12f);
                swordCollider.size = new Vector2(4.7f, 43.09f);
                swordAnimator.Play("Sword4");
                break;
        }
    }

    private void TriggerWeaponDecayProtection()
    {
        isProtected = true;
        remainingDecayProtectionDuration = maxDecayProtectionDuration;
        currentDecayPerFixedTime = 0f;
    }
}



/*
*                       offset         size
* Weapon 1 Dimensions:  0,-13.5        4,24
* Weapon 2 Dimensions:  0,-10          4,31
* Weapon 3 Dimensions:  0,-6           4.5,39
* Weapon 4 Dimensions:  0,-0.4         4.5,50
*/
