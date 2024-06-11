using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadManager : MonoBehaviour
{
    private IEnumerator loadCoroutine;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TextMeshProUGUI loadingValueText;

    private void Start()
    {
        loadingPanel.SetActive(false);
    }
    public void LoadSceneX(int targetScene)
    {
        StartCoroutine(LoaderCoroutine(targetScene));
    }

    private IEnumerator LoaderCoroutine(int index)
    {
        if (index > SceneManager.sceneCountInBuildSettings - 1)
        {
            index = 0;
        }
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        operation.allowSceneActivation = false;
        loadingPanel.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Round(operation.progress * 100) * 0.01f;
            loadingValueText.text = "%" + Mathf.Round(progress * 100);
            loadingSlider.value = progress;
            if (operation.progress >= 0.9f)
            {
                progress = Mathf.Round(operation.progress * 100) * 0.01f;
                loadingValueText.text = "%" + Mathf.Round(progress * 100);
                loadingSlider.value = progress;
                float hold = 1f;
                while (hold > 0)
                {
                    float randomHolder = Random.Range(0.1f, 0.3f);
                    hold -= randomHolder;
                    progress += 0.01f;
                    loadingValueText.text = "%" + Mathf.Round(progress * 100);
                    loadingSlider.value = progress;
                    yield return new WaitForSecondsRealtime(randomHolder);
                }
                loadingValueText.text = "%" + 100;
                loadingSlider.value = 1f;
                yield return new WaitForSecondsRealtime(0.3f);
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}


// while (!operation.isDone)
// {
//     float progress = Mathf.Round(operation.progress * 100) * 0.01f;
//     loadingValueText.text = "%" + Mathf.Round(progress) * 100f;
//     loadingSlider.value = progress;

//     yield return null;
// }
