using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : MonoBehaviour
{
    private EnemyManager enemyManager;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject orbParent;
    //[SerializeField] private GameObject[] fireOrbs;
    [SerializeField] private List<GameObject> fireOrbs;
    private GameObject activeOrb;
    [SerializeField] private float pushForce;

    private void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
        for (int i = 0; i < orbParent.transform.childCount; i++)
        {
            fireOrbs.Add(orbParent.transform.GetChild(i).gameObject);
        }
    }

    private void ShootObj()
    {
        foreach (GameObject orb in fireOrbs)
        {
            if (!orb.activeSelf)
            {
                activeOrb = orb;
                break;
            }
        }
        if (activeOrb == null)
        {
            fireOrbs[0].SetActive(false);
            activeOrb = fireOrbs[0];
        }
        Rigidbody orbRB = activeOrb.GetComponent<Rigidbody>();
        EnemyRangeOrb enemyRangeOrb = activeOrb.GetComponent<EnemyRangeOrb>();

        enemyRangeOrb.OrbDamageSetter(enemyManager.agentDamage);
        activeOrb.transform.position = firePoint.position;
        activeOrb.transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);

        activeOrb.SetActive(true);
        if (orbRB != null)
        {
            orbRB.velocity = Vector3.zero;
            orbRB.AddForce(transform.forward * pushForce, ForceMode.Impulse);
        }
        activeOrb = null;
        orbRB = null;
    }
    /*
    GameObject orb = null;
        orb.transform.position = castLoc.position;
        orb.transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        Rigidbody orbRB = orb.GetComponent<Rigidbody>();

        orb.SetActive(true);
        if (orbRB != null)
        {
            orbRB.velocity = Vector3.zero;
            orbRB.AddForce(transform.forward * castForce, ForceMode.Impulse);
        }
    */
}
