using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public Player player;
    private LoadManager loadManager;

    [Header("Top Left Paneş")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI[] stats;
    [SerializeField] private Image statPopImage;
    [SerializeField] private GameObject statPopPanel;

    [Header("Canvas")]
    [SerializeField] private GameObject StageCanvas;
    [SerializeField] private GameObject ShopCanvas;
    [SerializeField] private GameObject OptionsCanvas;
    private void Awake()
    {
        // QualitySettings.vSyncCount = 0;
        // Application.targetFrameRate = 60;
        PlayerStarter();
    }
    private void Start()
    {
        loadManager = GetComponent<LoadManager>();
        ShopCanvas.SetActive(false);
        OptionsCanvas.SetActive(false);
        statPopImage.enabled = true;
        statPopPanel.SetActive(false);
        UIManage();
        SoundManager();
    }
    private void Update()
    {
        Time.timeScale = 1f;
    }
    public void PlayerStarter()
    {
        if (PlayerPrefs.HasKey("saved"))
        {
            Debug.Log("data found");
            player.LoadPlayer();
        }
        else
        {
            Debug.Log("no data found");
            player.SavePlayer();
            PlayerPrefs.SetString("saved", "yes");
            PlayerPrefs.Save();
        }
    }
    public void UIManage()
    {
        float maxExp = player.playerLevel * 1000;
        float exp = player.playerExp;

        //level and exp
        levelText.text = player.playerLevel.ToString();
        expText.text = exp.ToString() + "/" + maxExp.ToString();
        expSlider.value = exp / maxExp;

        //coin
        coinText.text = player.playerCoin.ToString();

        //stats
        stats[0].text = player.playerHealth.ToString();
        stats[1].text = player.playerSpeed.ToString();
        stats[2].text = player.playerDamage.ToString();
    }
    private void SoundManager()
    {
        OptionsManager optionsManager = OptionsCanvas.GetComponentInChildren<OptionsManager>();
        if (player.playerSound)
        {
            optionsManager.ControlSound(true);
        }
        else
        {
            optionsManager.ControlSound(false);
        }
    }

    public void StatPopper()
    {
        statPopImage.enabled = !statPopImage.enabled;
        statPopPanel.SetActive(!statPopPanel.activeSelf);
    }
    public void playButton()
    {
        loadManager.LoadSceneX(player.playerStage);
    }
    //Canvas Buttons
    public void StageButton()
    {
        StageCanvas.SetActive(!StageCanvas.activeSelf);
    }
    public void ShopButon()
    {
        ShopCanvas.SetActive(!ShopCanvas.activeSelf);
    }
    public void OptionsButton()
    {
        OptionsCanvas.SetActive(!OptionsCanvas.activeSelf);
    }

}
