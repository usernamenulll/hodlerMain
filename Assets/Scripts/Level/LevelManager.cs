using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] public Player player;
    [SerializeField] private GameObject Guard;
    [SerializeField] private TargetPointer targetPointer;
    private OptionsManager optionsManager;
    private LoadManager loadManager;
    private PlayerController playerController;
    public static GameObject guardStatic;
    public static LevelManager levelManagerStatic;
    public static TargetPointer targetPointerStatic;
    private UIManager uIManager;

    [Header("Level Control")]
    [SerializeField] public int replayAmount;

    private void Awake()
    {
        player.LoadPlayer();
        levelManagerStatic = this;
        guardStatic = Guard;
        targetPointerStatic = targetPointer;
    }
    private void Start()
    {
        uIManager = GetComponent<UIManager>();
        playerController = Guard.GetComponent<PlayerController>();
        loadManager = GetComponent<LoadManager>();
        optionsManager = uIManager.OptionsUI.GetComponentInChildren<OptionsManager>();

        SoundManager();
    }

    private void SoundManager()
    {
        if (player.playerSound)
        {
            optionsManager.ControlSound(true);
        }
        else
        {
            optionsManager.ControlSound(false);
        }
    }

    public void BackToMenuForTest()
    {
        //SceneManager.LoadScene(0);
        loadManager.LoadSceneX(0);
    }
    public void LoadNextStage()
    {
        int x = SceneManager.GetActiveScene().buildIndex + 1;
        loadManager.LoadSceneX(x);
    }
    public void LoadTargetScene(int target)
    {
        loadManager.LoadSceneX(target);
    }
    public void ExpManger(int _expAmount)
    {
        playerController.playerEarnExp(_expAmount);
        uIManager.UpdatePlayerBar();
    }
}
