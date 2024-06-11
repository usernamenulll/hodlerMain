using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherZoner : MonoBehaviour
{
    private ZoneManager zoneManager;
    void Start()
    {
        zoneManager = GetComponentInParent<ZoneManager>();
        zoneManager.AddZoneScore(ZoneManager.ZonerType.Other);
    }
}
