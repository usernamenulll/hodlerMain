using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoostManager : MonoBehaviour
{
    private LevelManager levelManager;
    private UIManager uIManager;
    private PlayerController playerController;
    [Header("Boost Manage")]
    [SerializeField] private int[] boostCounts;
    [SerializeField] private int[] boostLevels;
    [SerializeField] private Button[] boosts;
    [SerializeField] private TextMeshProUGUI[] boostTexts;

    [Header("Speed Boost")]
    [SerializeField] private float speedMlt;
    [SerializeField] private float speedLevelMlt;
    [SerializeField] private float speedCD;
    [SerializeField] private Button speedButton;
    [SerializeField] private Image speedCDImage;
    [SerializeField] private ParticleSystem speedParticle;
    [SerializeField] public bool reduceDamage;
    [Header("Damage Boost")]
    [SerializeField] private float damageMlt;
    [SerializeField] private float damageLevelMlt;
    [SerializeField] private float damageCD;
    [SerializeField] private Button damageButton;
    [SerializeField] private Image damageCDImage;
    [SerializeField] private ParticleSystem damagePArticle;
    [Header("Health Boost")]
    [SerializeField] private float healMlt;
    [SerializeField] private float healthLevelMlt;
    [SerializeField] private float healthCD;
    [SerializeField] private Button healthButton;
    [SerializeField] private Image healthCDImage;
    private void Start()
    {
        levelManager = LevelManager.levelManagerStatic.GetComponent<LevelManager>();
        uIManager = LevelManager.levelManagerStatic.gameObject.GetComponent<UIManager>();
        playerController = LevelManager.guardStatic.GetComponent<PlayerController>();

        boostCounts = levelManager.player.boostCounts;
        boostLevels = levelManager.player.boostLevels;

        BoostUIHandle();
    }
    public void BoostUIHandle()
    {
        for (int i = 0; i < boosts.Length; i++)
        {
            if (boostCounts[i] <= 0)
            {
                boosts[i].interactable = false;
            }
            boostTexts[i].text = boostCounts[i].ToString();
        }
    }
    private void BoostSpend(int x)
    {
        boostCounts[x]--;
        levelManager.player.boostCounts = boostCounts;
        playerController.levelPlayer.boostCounts = boostCounts;
        levelManager.player.SavePlayer();
        BoostUIHandle();
    }
    public void SpeedBoost()
    {
        if (playerController.canHit)
        {
            if (boostCounts[0] > 0)
            {
                playerController.PlayerAttack("b_power", false, 0, 0);
                BoostSpend(0);
                StartCoroutine(SBCoroutine());
            }
            else
            {
                boosts[0].interactable = false;
            }
        }
    }
    private IEnumerator SBCoroutine()
    {
        float cd = speedCD;
        //speed = playerController.levelPlayer.playerSpeed;
        float speed = playerController.guardSpeed;
        reduceDamage = true;
        speedButton.interactable = false;
        speedCDImage.enabled = true;
        speedCDImage.fillAmount = 1f;
        //playerController.levelPlayer.playerSpeed = speed * speedMlt;
        playerController.guardSpeed = speed * (speedMlt + (boostLevels[0] * speedLevelMlt));
        yield return new WaitForSeconds(0.1f);
        speedParticle.Play();
        while (0 < cd)
        {
            yield return null;
            cd -= Time.deltaTime;
            speedCDImage.fillAmount = cd / speedCD;
            playerController.guardSpeed = speed * speedMlt;
        }
        playerController.guardSpeed = playerController.levelPlayer.playerSpeed;
        speedParticle.Stop();
        reduceDamage = false;
        speedCDImage.enabled = false;
        if (boostCounts[0] > 0)
        {
            speedButton.interactable = true;
        }
    }
    public void DamageBoost()
    {
        if (playerController.canHit)
        {
            if (boostCounts[1] > 0)
            {
                playerController.PlayerAttack("b_power", false, 0, 0);
                BoostSpend(1);
                StartCoroutine(DBCoroutine());
            }
            else
            {
                boosts[1].interactable = false;
            }
        }
    }
    private IEnumerator DBCoroutine()
    {
        float cd = damageCD;
        //damage = playerController.levelPlayer.playerDamage;
        float damage = playerController.guardDamage;
        damageButton.interactable = false;
        damageCDImage.enabled = true;
        damageCDImage.fillAmount = 1f;
        playerController.guardDamage = damage * (damageMlt + (boostLevels[1] * damageLevelMlt));
        yield return new WaitForSeconds(0.3f);
        damagePArticle.Play();
        while (0 < cd)
        {
            yield return null;
            cd -= Time.deltaTime;
            damageCDImage.fillAmount = cd / damageCD;
        }
        playerController.guardDamage = playerController.levelPlayer.playerDamage;
        damagePArticle.Stop();
        damageCDImage.enabled = false;
        if (boostCounts[1] > 0)
        {
            damageButton.interactable = true;
        }
    }
    public void HealthBoost()
    {
        if (playerController.canHit)
        {
            if (boostCounts[2] > 0)
            {
                playerController.PlayerAttack("b_power", false, 0, 0);
                BoostSpend(2);
                StartCoroutine(HBCoroutine());
            }else
            {
                boosts[2].interactable = false;
            }
        }
    }
    private IEnumerator HBCoroutine()
    {
        float cd = healthCD;
        healthButton.interactable = false;
        healthCDImage.enabled = true;
        healthCDImage.fillAmount = 1f;
        yield return new WaitForSeconds(0.15f);
        playerController.PlayerHealer(healMlt + ((boostLevels[2] * healthLevelMlt)));
        while (0 < cd)
        {
            yield return null;
            cd -= Time.deltaTime;
            healthCDImage.fillAmount = cd / healthCD;
        }
        healthCDImage.enabled = false;
        if (boostCounts[1] > 0)
        {
            healthButton.interactable = true;
        }

    }
    private void BoostProtector(string value)
    {
        switch (value)
        {
            case "true":
                playerController.canTakeDamage = true;
                break;
            case "false":
                playerController.canTakeDamage = false;
                break;
            default:
                playerController.canTakeDamage = true;
                break;
        }
    }
}
