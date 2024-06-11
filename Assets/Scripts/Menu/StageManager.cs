using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameManager gameManager;
    private LoadManager loadManager;
    private int count;
    //private bool[] stageComp;
    //private bool[] stageMission;
    [SerializeField] private Sprite activeImage;
    [SerializeField] private Sprite competeImage;
    [SerializeField] private Sprite notAvaibleImage;
    void Start()
    {
        loadManager = gameManager.gameObject.GetComponent<LoadManager>();
        //stageComp = player.stageComp.ToArray();
        //stageMission = player.stageMission.ToArray();
        count = transform.childCount;
        StageSetter();
    }
    private void StageSetter()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject stage = transform.GetChild(i).gameObject;
            Button stageButton = stage.GetComponent<Button>();
            int loadIndex = i + 1;
            TextMeshProUGUI stageText = stage.GetComponentInChildren<TextMeshProUGUI>();
            Image[] imageHolder = stage.GetComponentsInChildren<Image>();
            Image stageImage = imageHolder[0];
            Image star = imageHolder[2];
            Image activeBorder = imageHolder[3];

            //set stage text to level
            stageText.text = (i + 1).ToString();

            if (i < (player.playerStage - 1))
            {
                stageImage.sprite = competeImage;
                stageButton.onClick.AddListener(delegate { LoadButton(loadIndex); });
                star.enabled = true;
            }
            else if (i == (player.playerStage - 1))
            {
                stageImage.sprite = activeImage;
                stageButton.onClick.AddListener(delegate { LoadButton(loadIndex); });
                activeBorder.enabled = true;
            }
            else
            {
                stageImage.sprite = notAvaibleImage;
                stageButton.interactable = false;
            }
        }
    }

    public void LoadButton(int index)
    {
        gameManager.StageButton();
        loadManager.LoadSceneX(index);
        //SceneManager.LoadScene(index);
    }
}


/*
   private void StageButtonsManager()
   {
       stageButtons = stages.GetComponentsInChildren<Button>();
       for (int i = 0; i < stageButtons.Length; i++)
       {
           int index = i + 2;
           stageButtons[i].onClick.AddListener(delegate { LoadStageButton(index); });
       }
       for (int i = 0; i < playerLevel - 1; i++)
       {
           stageButtons[i].interactable = true;
       }
   }
   private void StageSetter()
   {
       for (int i = 0; i < count; i++)
       {
           GameObject stage = transform.GetChild(i).gameObject;
           Button stageButton = stage.GetComponent<Button>();
           int loadIndex = i + 1;
           TextMeshProUGUI stageText = stage.GetComponentInChildren<TextMeshProUGUI>();
           Image stageImage = stage.GetComponent<Image>();
           GameObject star = stage.transform.GetChild(1).GetChild(1).gameObject;
           stageText.text = (i + 1).ToString();
           if (i == (player.playerStage - 1))
           {
               stageImage.sprite = activeImage;
               stageButton.onClick.AddListener(delegate { LoadButton(loadIndex); });
           }
           else
           {
               if (stageComp[i])
               {
                   stageImage.sprite = competeImage;
                   stageButton.onClick.AddListener(delegate { LoadButton(loadIndex); });
               }
           }
           //sets start for mission ??
           if (stageMission[i])
           {
               star.SetActive(true);
           }
       }
   }
   */
