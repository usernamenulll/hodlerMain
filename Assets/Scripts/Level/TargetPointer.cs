using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetPointer : MonoBehaviour
{

    [Header("Pointer")]
    [SerializeField] private Transform guardTransform;
    private Transform targetPos;
    [SerializeField] private RectTransform pointerRectTransform;
    [SerializeField] private Image pointerImage;
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private Sprite crossSprite;

    void Start()
    {
        Hide();
        //guardTransform = LevelManager.guardStatic.transform;
        //pointerRectTransform = GetComponentInChildren<RectTransform>();
        //pointerImage = GetComponentInChildren<Image>();
        if (pointerImage == null)
        {
            pointerImage = transform.GetChild(0).GetComponent<Image>();
        }
    }
    private void OnEnable()
    {
        //pointerImage = GetComponentInChildren<Image>();
    }

    private void Update()
    {
        PointerUpdater();
    }

    private void PointerRotator()
    {
        Vector3 toPos = targetPos.position;
        Vector3 fromPos = Camera.main.transform.position;
        //Vector3 fromPos = guardTransform.position;
        toPos.y = 0.0f;
        fromPos.y = 0.0f;

        Vector3 dir = (toPos - fromPos).normalized;

        float angle = Vector3.Angle(dir, toPos);
        if (dir.x > 0.0f)
        {
            angle = 360 - angle;
        }
        pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }
    private void PointerUpdater()
    {

        float borderSize = 100f;
        Vector3 targetScreenPoint = Camera.main.WorldToScreenPoint(targetPos.position);
        bool isOffScreen = targetScreenPoint.x <= borderSize || targetScreenPoint.x >= Screen.width - borderSize
         || targetScreenPoint.y <= borderSize || targetScreenPoint.y >= Screen.height - borderSize;
        if (isOffScreen)
        {
            pointerImage.sprite = arrowSprite;
            PointerRotator();
            Vector3 cappedPos = targetScreenPoint;
            if (cappedPos.x <= borderSize) cappedPos.x = borderSize;
            if (cappedPos.x >= Screen.width - borderSize) cappedPos.x = Screen.width - borderSize;
            if (cappedPos.y <= borderSize) cappedPos.y = borderSize;
            if (cappedPos.y >= Screen.height - borderSize) cappedPos.y = Screen.height - borderSize;

            //Vector3 pointerWorldPos = Camera.main.ScreenToWorldPoint(cappedPos);

            pointerRectTransform.position = cappedPos;
            pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0.0f);
        }
        else
        {
            pointerImage.sprite = crossSprite;
            pointerRectTransform.position = targetScreenPoint;
            pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0.0f);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show(Transform _target)
    {
        gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(true);
        this.targetPos = _target;
    }
}
