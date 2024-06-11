using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActivator : MonoBehaviour
{
    // [SerializeField] private TrapManager[] connectedTraps;
    private BoxCollider startCollider;
    private BoxCollider endCollider;
    private bool trapsActivated;
    private List<TrapManager> connectedTrap = new List<TrapManager>();
    private void Start()
    {
        startCollider = GetComponent<BoxCollider>();
        endCollider = transform.GetChild(0).GetComponent<BoxCollider>();
        trapsActivated = false;

        GameObject trapParent = transform.GetChild(1).gameObject;
        for (int i = 0; i < trapParent.transform.childCount; i++)
        {
            connectedTrap.Add(trapParent.transform.GetChild(i).GetComponent<TrapManager>());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!trapsActivated)
            {
                startCollider.enabled = false;
                endCollider.enabled = true;
                trapsActivated = true;
                ActivateTraps();
            }
            else
            {
                endCollider.enabled = false;
                DisableTraps();
            }

        }
    }

    public void DisableTraps()
    {
        foreach (TrapManager _tm in connectedTrap)
        {
            if (_tm.gameObject.activeSelf)
            {
                _tm.DisableTrap();
            }
        }
    }
    private void ActivateTraps()
    {
        foreach (TrapManager _tm in connectedTrap)
        {
            _tm.TrapActivate();
        }
    }
}
