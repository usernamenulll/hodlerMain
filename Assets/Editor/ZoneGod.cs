using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ZoneCreator))]
public class ZoneGod : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ZoneCreator zoneCreator = (ZoneCreator)target;

        EditorGUILayout.LabelField("One Button to rule them all");

        if (GUILayout.Button("One Button"))
        {
            zoneCreator.CreateObjects();
            zoneCreator.CreateWalls();
            zoneCreator.CreateBaseTile();
            zoneCreator.SelectTile(true);
            zoneCreator.CreateTiles();
            zoneCreator.CreateSides();
        }

        EditorGUILayout.LabelField("Create Destroy Main");

        if (GUILayout.Button("Create GameObjects"))
        {
            zoneCreator.CreateObjects();
        }
        if (GUILayout.Button("Destroy Main"))
        {
            zoneCreator.DeleteMainObject();
        }

        EditorGUILayout.LabelField("Set");

        if (GUILayout.Button("Create Walls"))
        {
            zoneCreator.CreateWalls();
        }
        if (GUILayout.Button("SelectTile"))
        {
            zoneCreator.SelectTile(false);
        }
        if (GUILayout.Button("Create Base Tile"))
        {
            zoneCreator.CreateBaseTile();
        }
        if (GUILayout.Button("Create Tiles"))
        {
            zoneCreator.CreateTiles();
        }
        if (GUILayout.Button("Create Sides"))
        {
            zoneCreator.CreateSides();
        }
    }
}

/*
public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        ZoneCreator zoneCreator = (ZoneCreator)target;

        zoneCreator.origin = EditorGUILayout.Vector3Field("Origin" , zoneCreator.origin);
        zoneCreator.wallObject = (GameObject)EditorGUILayout.ObjectField("Wall" ,zoneCreator.wallObject , typeof(GameObject) , true);
    } 
*/
