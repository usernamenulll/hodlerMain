using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ZoneManager : MonoBehaviour
{
    public enum ZonerType
    {
        Enemy,
        Trap,
        Other
    }
    private UIManager uIManager;
    private LevelManager levelManager;
    [Header("Zone Control")]
    private List<GameObject> connectedZoners = new List<GameObject>();
    [SerializeField] public string zoneName;
    [SerializeField] private int zoneScore;
    [SerializeField] private int enemyZoneHit;
    [SerializeField] private int trapZoneHit;
    [SerializeField] private int otherZoneHit;

    [Header("Zone Pass")]
    [SerializeField] public bool isZonePassed;
    private PassController passController;
    public GameObject DestroyZone;
    public ZoneManager nextZoneManager;
    private void Awake()
    {
        //set zone scores to default (10) if nothing special happening
        TrapHitSetter();
    }
    private void Start()
    {
        uIManager = LevelManager.levelManagerStatic.gameObject.GetComponent<UIManager>();
        levelManager = LevelManager.levelManagerStatic;

        //gets pass controller if zone is not last
        if (transform.GetChild(0).GetComponent<PassController>())
        {
            passController = transform.GetChild(0).GetComponent<PassController>();
        }
        else
        {
            if (nextZoneManager != null)
            {
                Debug.Log("plese add passController at child0 to : " + gameObject.transform.parent.gameObject.name);
            }
        }

        //add zone objects to connectedZoners (not pass manager i starts from 1) to destroy later and disables connected zoners if
        //their zone is not first zone
        if (transform.parent.gameObject.name == "Zone1")
        {
            for (int i = 1; i < gameObject.transform.childCount; i++)
            {
                connectedZoners.Add(gameObject.transform.GetChild(i).gameObject);
            }
        }
        else
        {
            for (int i = 1; i < gameObject.transform.childCount; i++)
            {
                connectedZoners.Add(gameObject.transform.GetChild(i).gameObject);
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }


    public void ZoneScoreManager(ZonerType _type)
    {
        if (!isZonePassed)
        {
            switch (_type)
            {
                case ZonerType.Enemy:
                    zoneScore -= enemyZoneHit;
                    break;
                case ZonerType.Trap:
                    zoneScore -= trapZoneHit;
                    break;
                case ZonerType.Other:
                    zoneScore -= otherZoneHit;
                    break;
                default:
                    Debug.Log("error zonerType dosent exist");
                    break;
            }
            if (zoneScore <= 0)
            {
                isZonePassed = true;
                zoneScore = 0;
                PassZone();
            }
        }
        else
        {
            Debug.Log("you cant pass a passed zone sorry");
        }
    }
    public void AddZoneScore(ZonerType tip)
    {
        switch (tip)
        {
            case ZonerType.Enemy:
                zoneScore += enemyZoneHit;
                break;
            case ZonerType.Trap:
                zoneScore += trapZoneHit;
                break;
            case ZonerType.Other:
                zoneScore += otherZoneHit;
                break;
            default:
                Debug.Log("error zonerType dosent exist");
                break;
        }
    }
    private void PassZone()
    {
        NotificationManager notificationManager = levelManager.gameObject.GetComponent<NotificationManager>();
        //if there is no other zone tell level clear
        if (nextZoneManager != null)
        {
            //notify player for zone clear
            notificationManager.NotPlayer(1, zoneName + " passed! Please proceed");
            //open next zone door
            passController.PassZoneTrigger();

            //activate next zone
            nextZoneManager.ActivateZone();
        }
        else
        {
            notificationManager.NotPlayer(2, "Congratulations !!\n Level clear.");
            // PassLevel();
            StartCoroutine(PassCoroutine());
        }
    }
    private IEnumerator PassCoroutine()
    {
        ChestManager[] chests = GetComponentsInChildren<ChestManager>();
        foreach (ChestManager chest in chests)
        {
            if (!chest.chestCollected)
            {
                chest.ChestOpen();
            }
        }
        yield return new WaitForSeconds(1.5f);
        PassLevel();
    }
    private void PassLevel()
    {
        uIManager.PassLevel();
    }
    public void ActivateZone()
    {
        if (connectedZoners.Capacity > 0)
        {
            foreach (GameObject _obj in connectedZoners)
            {
                _obj.SetActive(true);
            }
        }
    }
    public void KillZone()
    {
        //destroy zone gameOb
        foreach (GameObject _obj in connectedZoners)
        {
            Destroy(_obj, 2f);
        }
        if (DestroyZone != null)
        {
            Destroy(DestroyZone);
        }
    }

    private void TrapHitSetter()
    {
        if (enemyZoneHit == 0)
        {
            enemyZoneHit = 10;
        }
        if (trapZoneHit == 0)
        {
            trapZoneHit = 10;
        }
    }
}
