using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneAdmin : MonoBehaviour
{
    private List<GameObject> zones = new List<GameObject>();
    private List<ZoneManager> zoneManagers = new List<ZoneManager>();
    void Start()
    {
        AssingZones();
    }
    private void AssingZones()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject zn = transform.GetChild(i).gameObject;
            ZoneManager targetZone = zn.transform.GetChild(0).GetComponent<ZoneManager>();

            targetZone.zoneName = "Zone " + (i + 1).ToString();

            zones.Add(zn);
            //zoneManagers.Add(zn.transform.GetChild(0).GetComponent<ZoneManager>());
            zoneManagers.Add(targetZone);
        }
        for (int i = 0; i < zones.Count; i++)
        {
            if ((i + 1) < zones.Count)
            {
                zoneManagers[i].nextZoneManager = zoneManagers[i + 1];
            }
            if ((i - 2) >= 0 && i > 0)
            {
                zoneManagers[i].DestroyZone = zoneManagers[i-2].transform.parent.gameObject;
            }
        }
    }
}
