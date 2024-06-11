using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private Sprite[] notifySprites;
    public float notDisableTime;
    [SerializeField] private Animator notAnim;
    [SerializeField] private Image notImage;
    [SerializeField] private TextMeshProUGUI notText;
    [SerializeField] private Button notButton;
    private IEnumerator notCor;
    public void NotPlayer(int iconNumber, string text)
    {
        notAnim.enabled = true;
        notText.text = text;
        notImage.sprite = notifySprites[iconNumber];
        notAnim.SetTrigger("t_start");
        notButton.interactable = true;
        notCor = notCoroutine();
        StartCoroutine(notCor);
    }
    public void NotPressed()
    {
        notButton.interactable = false;
        notAnim.SetTrigger("t_end");
        StopCoroutine(notCor);
    }
    private IEnumerator notCoroutine()
    {
        yield return new WaitForSeconds(notDisableTime);
        notButton.interactable = false;
        notAnim.SetTrigger("t_end");
    }
}
