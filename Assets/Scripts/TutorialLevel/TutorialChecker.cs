using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialChecker : MonoBehaviour
{
    [SerializeField] private GameObject nextObject;
    private TargetPointer targetPointer;
    void Start()
    {
        targetPointer = LevelManager.targetPointerStatic;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(GetComponent<BoxCollider>());
            targetPointer.Hide();
            if (nextObject.GetComponent<PassController>())
            {
                ZoneManager zoner = GetComponentInParent<ZoneManager>();
                zoner.ZoneScoreManager(ZoneManager.ZonerType.Other);
            }
            else
            {
                targetPointer.Show(nextObject.transform);
            }
        }
    }
}
