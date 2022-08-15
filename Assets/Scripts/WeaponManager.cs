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

    [SerializeField] float maxDecayForLevelOne = 0.22f;
    [SerializeField] float maxDecayForLevelTwo = 0.25f;
    [SerializeField] float maxDecayForLevelThree = 0.27f;
    [SerializeField] float maxDecayForLevelFour = 0.3f;

    [SerializeField] float levelOneDecayIncrease = 0.02f;
    [SerializeField] float levelTwoDecayIncrease = 0.021f;
    [SerializeField] float levelThreeDecayIncrease = 0.022f;
    [SerializeField] float levelFourDecayIncrease = 0.023f;

    [SerializeField] SpawnManager spawner;

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
        decayIncreaseAmountUpdated = levelOneDecayIncrease;
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
        if (weaponLevel == 1 && decayAmountPerFixedTime > maxDecayForLevelOne)
        {
            decayAmountPerFixedTime = maxDecayForLevelOne;
        }
        else if (weaponLevel == 2 && decayAmountPerFixedTime > maxDecayForLevelTwo)
        {
            decayAmountPerFixedTime = maxDecayForLevelTwo;
        }
        else if (weaponLevel == 3 && decayAmountPerFixedTime > maxDecayForLevelThree)
        {
            decayAmountPerFixedTime = maxDecayForLevelThree;
        }
        else if (weaponLevel == 4 && decayAmountPerFixedTime > maxDecayForLevelFour)
        {
            decayAmountPerFixedTime = maxDecayForLevelFour;
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
                swordAnimator.Play("Sword1");
                break;
            case 2:
                swordCollider.offset = new Vector2(0f, -10f);
                swordCollider.size = new Vector2(4f, 31f);
                decayIncreaseAmountUpdated = levelTwoDecayIncrease;
                swordAnimator.Play("Sword2");
                break;
            case 3:
                swordCollider.offset = new Vector2(0f, -6f);
                swordCollider.size = new Vector2(4.5f, 39f);
                decayIncreaseAmountUpdated = levelThreeDecayIncrease;
                swordAnimator.Play("Sword3");
                break;
            case 4:
                swordCollider.offset = new Vector2(0f, -0.4f);
                swordCollider.size = new Vector2(4.5f, 50f);
                decayIncreaseAmountUpdated = levelFourDecayIncrease;
                swordAnimator.Play("Sword4");
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

    private void OnEnable()
    {
        killCounter = 0;
        ResetSword();
        isProtected = true;
        remainingProtectionDuration = weaponProtectionDurationInSeconds;
        decayAmountPerFixedTime = 0f;
    }

    public void ResetSword()
    {
        while (weaponLevel != 1)
        {
            weaponLevel--;
            killCounter = 0;
            swordAnimator.ResetTrigger("downgrade");
            swordAnimator.SetTrigger("downgrade");
        }
        isProtected = true;
        remainingProtectionDuration = weaponProtectionDurationInSeconds;
        decayAmountPerFixedTime = 0f;
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

            spawner.DecreaseHordeByOne();
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
            ScoreManager.score += 100;
            isProtected = true;
            remainingProtectionDuration = weaponProtectionDurationInSeconds;
            decayAmountPerFixedTime = 0f;

            spawner.DecreaseHordeByOne();
        }
        else if (weaponLevel == 2 && killCounter > secondUpgradeThreshold)
        {
            weaponLevel++;
            killCounter = 0;
            swordAnimator.ResetTrigger("upgrade");
            swordAnimator.SetTrigger("upgrade");
            ScoreManager.score += 250;
            isProtected = true;
            remainingProtectionDuration = weaponProtectionDurationInSeconds;
            decayAmountPerFixedTime = 0f;

            spawner.DecreaseHordeByOne();
        }
        else if (weaponLevel == 3 && killCounter > thirdUpgradeThreshold)
        {
            weaponLevel++;
            killCounter = 0;
            swordAnimator.ResetTrigger("upgrade");
            swordAnimator.SetTrigger("upgrade");
            ScoreManager.score += 600;
            isProtected = true;
            remainingProtectionDuration = weaponProtectionDurationInSeconds;
            decayAmountPerFixedTime = 0f;

            spawner.DecreaseHordeByOne();
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
