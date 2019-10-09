#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Location))]
public class LocationEditor : Editor
{

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
    }

    // Update is called once per frame
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Location location = (Location)target;

        bool validLocation = EditorHelper.CurrentlyActiveLocation != null;

        if (validLocation) { GUI.color = Color.green; }
        else { GUI.color = Color.red; }

        EditorGUILayout.LabelField("Active Location", validLocation ? EditorHelper.CurrentlyActiveLocation.name : "null");

        GUI.color = Color.white;
        GUI.backgroundColor = Color.green;

        if (GUILayout.Button("Align View To Location"))
        {
            Vector3 newPos = location.transform.position;
            newPos.y = 0;
            SceneView.lastActiveSceneView.pivot = newPos;
            SceneView.lastActiveSceneView.size = 0.0001f;
            SceneView.lastActiveSceneView.Repaint();
            location.UpdateMaterial();
            EditorHelper.SetCurrentlyActiveLocation(location);
        }

        GUI.backgroundColor = Color.blue;

        if (GUILayout.Button("Set Active Location"))
        {
            EditorHelper.SetCurrentlyActiveLocation(location);
        }

        if (GUILayout.Button("Spawn Teleporters"))
        {
            location.SpawnTeleportersIfNeeded();
        }

        if(GUILayout.Button ("Delete Teleporters"))
        {
            location.RemoveAllTeleporters();
        }

        serializedObject.ApplyModifiedProperties();
    }




}
#endif
