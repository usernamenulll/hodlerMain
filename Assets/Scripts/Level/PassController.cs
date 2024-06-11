using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassController : MonoBehaviour
{
    private Animator passAnim;
    // [SerializeField] private GameObject DoorHinge;
    [SerializeField] private BoxCollider passTriggerCollider;
    [SerializeField] private GameObject passBlocker;
    [Header("Pointer")]
    [SerializeField] private Transform targetPos;
    private TargetPointer targetPointer;
    private bool isPassed = false;

    void Start()
    {
        passAnim = GetComponent<Animator>();
        passBlocker.SetActive(false);
        targetPointer = LevelManager.targetPointerStatic;
        passBlocker.SetActive(true);
    }

    public void PassZoneTrigger()
    {
        passBlocker.SetActive(false);
        passAnim.SetTrigger("t_open");
        targetPointer.Show(targetPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isPassed)
        {
            isPassed = true;
            targetPointer.Hide();
            if (transform.parent.CompareTag("ZoneManager"))
            {
                transform.parent.GetComponent<ZoneManager>().KillZone();
            }
            StartCoroutine(PassTriggerCourotine());
            //do some level stuff
        }
    }
    private IEnumerator PassTriggerCourotine()
    {
        Destroy(GetComponent<Rigidbody>());
        passBlocker.SetActive(true);
        passAnim.SetTrigger("t_close");
        yield return new WaitForSeconds(1.5f);
        passTriggerCollider.enabled  =false;
    }
}




//private void PointerUpdater()
// {
//     Vector3 toPos = targetPos.position;
//     Vector3 fromPos = Camera.main.transform.position;
//     //Vector3 fromPos = guardTransform.position;
//     toPos.y = 0.0f;
//     fromPos.y = 0.0f;

//     Vector3 dir = (toPos - fromPos).normalized;

//     float angle = Vector3.Angle(dir, toPos);
//     if (dir.x > 0.0f)
//     {
//         angle = 360 - angle;
//     }
//     pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);

//     float borderSize = 100f;
//     Vector3 targetScreenPoint = Camera.main.WorldToScreenPoint(targetPos.position);
//     bool isOffScreen = targetScreenPoint.x <= borderSize || targetScreenPoint.x >= Screen.width - borderSize
//      || targetScreenPoint.y <= borderSize || targetScreenPoint.y >= Screen.height - borderSize;
//     Debug.Log(isOffScreen);
//     if (isOffScreen)
//     {
//         Vector3 cappedPos = targetScreenPoint;
//         if (cappedPos.x <= borderSize) cappedPos.x = borderSize;
//         if (cappedPos.x >= Screen.width - borderSize) cappedPos.x = Screen.width - borderSize;
//         if (cappedPos.y <= borderSize) cappedPos.y = borderSize;
//         if (cappedPos.y >= Screen.height - borderSize) cappedPos.y = Screen.height - borderSize;

//         //Vector3 pointerWorldPos = Camera.main.ScreenToWorldPoint(cappedPos);

//         pointerRectTransform.position = cappedPos;
//         pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0.0f);
//     }
//     else
//     {
//         pointerRectTransform.position = targetScreenPoint;
//         pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0.0f);
//     }
// }
    // IEnumerator ZonePassCoroutine()
    // {
    //     yield return new WaitForSeconds(1f);
    //     passAnim.enabled = false;
    //     DoorHinge.transform.localRotation = Quaternion.Euler(new Vector3(0, -90, 0));
    // }

    // private void passDoorState()
    // {
    //     //StartCoroutine(ZonePassCoroutine());
    // }