using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private TargetPointer targetPointer;
    [SerializeField] private List<GameObject> tutorialPanels = new List<GameObject>();

    void Start()
    {
        targetPointer = LevelManager.targetPointerStatic;
        if (PlayerPrefs.HasKey("tutorial_pass"))
        {
            Debug.Log("player has played tutorial");
        }
        else
        {
            PlayerPrefs.SetString("tutorial_pass", "yes");
            StartTutorial();
        }
    }

    void Update()
    {

    }

    public void StartTutorial()
    {
        Time.timeScale = 0.0f;
        OpenTutorialPanel(0);
    }
    public void OpenTutorialPanel(int value)
    {
        for (int i = 0; i < tutorialPanels.Capacity; i++)
        {
            if (i == value)
            {
                tutorialPanels[i].SetActive(true);
            }
            else
            {
                tutorialPanels[i].SetActive(false);
            }
        }
        if (value == tutorialPanels.Capacity)
        {
            Time.timeScale = 1.0f;
        }
    }
}
