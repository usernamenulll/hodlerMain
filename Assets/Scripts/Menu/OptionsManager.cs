using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class OptionsManager : MonoBehaviour
{
    private LevelManager levelManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioListener audioListener;
    [SerializeField] private GameObject popPanel;
    [SerializeField] private Button yesButton;
    void Start()
    {
        levelManager = LevelManager.levelManagerStatic;
        Toggle[] soundToggle = GetComponentsInChildren<Toggle>();
        if (soundToggle.Length > 0)
        {
            if (gameManager)
            {
                if (gameManager.player.playerSound)
                {
                    soundToggle[0].isOn = true;
                }
                else
                {
                    soundToggle[0].isOn = false;
                }
            }else
            {
                if (levelManager.player.playerSound)
                {
                    soundToggle[0].isOn = true;
                }
                else
                {
                    soundToggle[0].isOn = false;
                }
            }
        }
    }
    void Update()
    {

    }
    public void PopHandler()
    {
        popPanel.SetActive(!popPanel.activeSelf);
    }
    public void ControlSound(bool value)
    {
        if (value)
        {
            //audioListener.enabled = true;
            AudioListener.volume = 1;
        }
        else
        {
            //audioListener.enabled = false;
            AudioListener.volume = 0;
        }

        if (levelManager)
        {
            levelManager.player.playerSound = value;
            levelManager.player.SavePlayer();
        }

        if (gameManager)
        {
            gameManager.player.playerSound = value;
            gameManager.player.SavePlayer();
        }
    }

    private void YesPopAssing(Action method)
    {
        yesButton.onClick.AddListener(delegate { method(); });
    }
    private void Deleter()
    {
        if (gameManager != null)
        {
            Debug.Log("deleting all data (prefs and local save)");
            PlayerPrefs.DeleteAll();
            gameManager.player.DeletePlayer();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    public void PopOpener(int value)
    {
        PopHandler();
        switch (value)
        {
            case 1:
                YesPopAssing(Deleter);
                break;
            case 2:
                YesPopAssing(LoadThisScene);
                break;
            case 3:
                YesPopAssing(LoadMainScene);
                break;
            default:
                Debug.Log("wrong type data");
                break;
        }
    }
    private void LoadMainScene()
    {
        levelManager.LoadTargetScene(0);
    }
    private void LoadThisScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void DeleterOpt()
    {
        PopHandler();
        YesPopAssing(Deleter);
    }
}
