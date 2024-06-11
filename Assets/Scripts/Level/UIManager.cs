using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    [Header("Skill")]
    private LevelManager levelManager;
    private PlayerController playerController;
    [SerializeField] private GameObject[] skills;
    [Header("Level UI")]
    [SerializeField] private GameObject MainUI;
    [SerializeField] private GameObject ResumeUI;
    [SerializeField] private GameObject ResultUI;
    [SerializeField] public GameObject OptionsUI;
    [Header("Player UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private GameObject AttackButton;
    private IEnumerator zoneNotify;
    [Header("Gold")]
    [SerializeField] private TextMeshProUGUI goldText;


    [Header("DefeatUI")]
    [SerializeField] private GameObject DefeatUI;
    [SerializeField] private TextMeshProUGUI rewardText;
    private int randomBoost;
    private string randomBoostTxt;
    private int randomBoostCount;
    private void Start()
    {
        levelManager = GetComponent<LevelManager>();
        playerController = LevelManager.guardStatic.GetComponent<PlayerController>();
        SkillButtonUpdate();

        MainUI.SetActive(true);
        ResumeUI.SetActive(false);
        DefeatUI.SetActive(false);
        ResultUI.SetActive(false);
        OptionsUI.SetActive(false);

        UpdatePlayerBar();
        PlayerGoldControl();
        healthSlider.value = 1f;
    }
    public void UpdatePlayerBar()
    {
        //level and exp
        float maxExp = playerController.levelPlayer.playerLevel * 1000;
        float exp = playerController.levelPlayer.playerExp;
        levelText.text = playerController.levelPlayer.playerLevel.ToString();
        expText.text = exp.ToString() + "/" + maxExp.ToString();
        expSlider.value = exp / maxExp;
    }
    public void PlayerHealthControl(float _val)
    {
        healthSlider.value = _val;
        if (_val == 0)
        {
            MainUI.SetActive(false);
            if (levelManager.replayAmount > 0)
            {
                PlayerResume();
            }
            else
            {
                PlayerDefeat();
            }
        }
    }
    public void PlayerGoldControl()
    {
        goldText.text = playerController.levelPlayer.playerCoin.ToString();
    }
    public void ContinuePlaying()
    {
        //show some ads
        Debug.Log("show ads");
        playerController.levelPlayer.playerHealth = playerController.playerMaxHealth;

        ResumeUI.SetActive(false);
        MainUI.SetActive(true);

        healthSlider.value = 1f;
        Time.timeScale = 1;
        playerController.PlayerStartedAgain(5f);
    }
    private void PlayerResume()
    {
        Time.timeScale = 0;
        levelManager.replayAmount--;
        ResumeUI.SetActive(true);
    }
    private void PlayerDefeat()
    {
        randomBoost = Random.Range(0, 3);
        randomBoostCount = Random.Range(1, 2);
        switch (randomBoost)
        {
            case 0:
                rewardText.text = randomBoostCount.ToString() + " Speed Boost";
                break;
            case 1:
                rewardText.text = randomBoostCount.ToString() + " Damage Boost";
                break;
            case 2:
                rewardText.text = randomBoostCount.ToString() + " Health Potion";
                break;
            default:
                rewardText.text = "Error 500";
                Debug.Log("Error");
                break;
        }
        Time.timeScale = 0;
        DefeatUI.SetActive(true);
    }
    public void RestartSceneWithReward()
    {
        levelManager.player.boostCounts[randomBoost] += randomBoostCount;
        levelManager.player.SavePlayer();
        LevelManager.guardStatic.GetComponent<BoostManager>().BoostUIHandle();
        //Adds
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void PassLevel()
    {
        //player.SavePlayer();
        playerController.PlayerVictory();
        StartCoroutine(levelPassCor());
    }
    IEnumerator levelPassCor()
    {
        MainUI.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f;
        ResultUI.SetActive(true);
    }

    public void SkillButtonUpdate()
    {
        int set = playerController.levelPlayer.playerSet[playerController.levelPlayer.playerLevel];
        switch (set)
        {
            case 0:
                foreach (GameObject _skill in skills)
                {
                    _skill.SetActive(false);
                }
                break;
            case 1:
                skills[0].SetActive(true);
                skills[1].SetActive(false);
                break;
            case 2:
                foreach (GameObject _skill in skills)
                {
                    _skill.SetActive(true);
                }
                break;
            default:
                Debug.Log("player hit max level");
                break;
        }
    }

    //options fucs
    public void OpenOptions()
    {
        Time.timeScale = 0f;
        OptionsUI.SetActive(true);
    }
    public void CloseOptions()
    {
        OptionsUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
