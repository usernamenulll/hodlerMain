using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneCreator : MonoBehaviour
{
    [SerializeField] private Vector3 origin;
    [SerializeField] private GameObject wallObject;
    [SerializeField] private List<GameObject> floorObjects;
    [SerializeField] private GameObject floorTile;
    [SerializeField] private GameObject baseTile;
    [SerializeField] private GameObject sider;
    [SerializeField] private float width;
    [SerializeField] private float height;
    [SerializeField] private int size;
    [SerializeField] private float offset;
    [SerializeField] private float siderMlt;
    [HideInInspector] public GameObject Zone;
    [HideInInspector] public GameObject zoneController;
    [HideInInspector] public GameObject pass;
    [HideInInspector] public GameObject kill;
    [HideInInspector] public GameObject floor;
    [HideInInspector] public GameObject tileParent;
    [HideInInspector] public GameObject baseParent;
    [HideInInspector] public GameObject wallParento;
    [HideInInspector] public GameObject walls;
    [HideInInspector] public GameObject siderParent;
    public void CreateObjects()
    {
        Zone = new GameObject("Zone X");

        zoneController = new GameObject("ZoneController");
        zoneController.AddComponent<ZoneManager>();
        zoneController.transform.SetParent(Zone.transform);

        pass = new GameObject("Pass");
        pass.transform.SetParent(zoneController.transform);

        kill = new GameObject("Kill");
        kill.transform.SetParent(zoneController.transform);

        floor = new GameObject("Floor");
        floor.transform.SetParent(Zone.transform);

        tileParent = new GameObject("Tiles");
        tileParent.transform.SetParent(floor.transform);

        baseParent = new GameObject("Base");
        baseParent.transform.SetParent(floor.transform);

        wallParento = new GameObject("WallParent");
        wallParento.transform.SetParent(Zone.transform);

        walls = new GameObject("Walls");
        walls.transform.SetParent(wallParento.transform);

        siderParent = new GameObject("Sider");
        siderParent.transform.SetParent(wallParento.transform);
    }
    public void CreateWalls()
    {
        if (Zone)
        {
            Vector3 pos = origin;
            Quaternion rot = Quaternion.Euler(0, 0, 0);
            pos = new Vector3(origin.x, origin.y, origin.z - offset);
            //bott wing
            for (int i = 0; i < width; i++)
            {
                Instantiate(wallObject, pos, rot).transform.SetParent(walls.transform);
                pos = new Vector3(pos.x + size, pos.y, pos.z);
            }
            //left wing
            pos = new Vector3(origin.x - size - offset, origin.y, origin.z);
            rot = Quaternion.Euler(0, 90, 0);
            for (int i = 0; i < height; i++)
            {
                Instantiate(wallObject, pos, rot).transform.SetParent(walls.transform);
                pos = new Vector3(pos.x, pos.y, pos.z + size);
            }
            // top wing 
            pos = new Vector3(origin.x - size, origin.y, origin.z + (height * size) + offset);
            rot = Quaternion.Euler(0, 180, 0);
            for (int i = 0; i < width; i++)
            {
                Instantiate(wallObject, pos, rot).transform.SetParent(walls.transform);
                pos = new Vector3(pos.x + size, pos.y, pos.z);
            }
            //right wing
            pos = new Vector3(origin.x + (size * (width - 1)) + offset, origin.y, origin.z + size);
            rot = Quaternion.Euler(0, -90, 0);
            for (int i = 0; i < height; i++)
            {
                Instantiate(wallObject, pos, rot).transform.SetParent(walls.transform);
                pos = new Vector3(pos.x, pos.y, pos.z + size);
            }
        }
        else
        {
            Debug.Log("please create  main first");
        }
    }
    public void CreateBaseTile()
    {
        if (Zone)
        {
            GameObject bt = Instantiate(baseTile, origin, Quaternion.identity);
            bt.transform.localScale = new Vector3(width, baseTile.transform.localScale.y, height);
            bt.transform.position = new Vector3(origin.x + ((width - 1) * size), origin.y, origin.z);
            bt.transform.SetParent(baseParent.transform);
        }
        else
        {
            Debug.Log("please create main first");
        }
    }

    public void CreateTiles()
    {
        if (Zone)
        {
            Vector3 pos = origin;
            Quaternion rot = Quaternion.identity;
            for (int i = 0; i < height; i++)
            {
                for (int x = 0; x < width; x++)
                {
                    pos = new Vector3(origin.x + (size * x), origin.y, origin.z + (size * i));
                    Instantiate(floorTile, pos, rot).transform.SetParent(tileParent.transform);
                }
            }
        }
        else
        {
            Debug.Log("please create main first");
        }
    }

    public void CreateSides()
    {
        if (Zone)
        {
            Vector3 sideOrgn = new Vector3(origin.x, 6.5f, origin.z);
            Quaternion sRot = Quaternion.Euler(0, -180, -180);
            float sideOffset = size / 2;
            //int wCount = Mathf.RoundToInt(width / 2);
            int wCount = Mathf.CeilToInt(width / 2);
            if (wCount < 2)
            {
                wCount = 2;
            }
            int hCount = Mathf.CeilToInt(height / 2);
            if (hCount < 2)
            {
                hCount = 2;
            }
            //bot wing
            for (int i = 0; i < wCount; i++)
            {
                Vector3 sPos = new Vector3((sideOrgn.x) + (i * size * siderMlt) - (size), sideOrgn.y, origin.z - (2 * size));
                Instantiate(sider, sPos, sRot).transform.SetParent(siderParent.transform);
            }
            //top wing
            sRot = Quaternion.Euler(0, 0, -180);
            for (int i = 0; i < wCount; i++)
            {
                Vector3 sPos = new Vector3((sideOrgn.x) + (i * size * siderMlt) - (size), sideOrgn.y, sideOrgn.z + (2 * size) + (height * size));
                Instantiate(sider, sPos, sRot).transform.SetParent(siderParent.transform);
            }
            //left wing
            sRot = Quaternion.Euler(0, -90, -180);
            for (int i = 0; i < hCount; i++)
            {
                Vector3 sPos = new Vector3(sideOrgn.x - (3 * size), sideOrgn.y, sideOrgn.z - (size) + (i * size * siderMlt));
                Instantiate(sider, sPos, sRot).transform.SetParent(siderParent.transform);
            }
            //right wing 
            sRot = Quaternion.Euler(0, -270, -180);
            for (int i = 0; i < hCount; i++)
            {
                Vector3 sPos = new Vector3(sideOrgn.x + (width * size) + size, sideOrgn.y, sideOrgn.z - (size) + (i * size * siderMlt));
                Instantiate(sider, sPos, sRot).transform.SetParent(siderParent.transform);
            }
        }
        else
        {
            Debug.Log("please add floor objects");
        }
    }
    public void SelectTile(bool value)
    {
        if (floorObjects.Count > 0)
        {
            if (value)
            {
                floorTile = floorObjects[0];
            }
            else
            {
                int sayi = Random.Range(0, floorObjects.Count);
                floorTile = floorObjects[sayi];
            }
        }
        else
        {
            Debug.Log("please add floor objects");
        }
    }
    public void DeleteMainObject()
    {
        if (Zone)
        {
            DestroyImmediate(Zone);
        }
        else
        {
            Debug.Log("there is no main object");
        }
    }
}
