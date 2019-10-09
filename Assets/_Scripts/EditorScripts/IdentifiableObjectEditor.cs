#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(IdentifiableObject), editorForChildClasses: true)]
public class IdentifiableObjectEditor : Editor
{


    private void OnEnable()
    {
        
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        IdentifiableObject io = (IdentifiableObject)target;
        bool validLocation = EditorHelper.CurrentlyActiveLocation != null;


        if (validLocation) { GUI.color = Color.green; }
        else { GUI.color = Color.red; }

        EditorGUILayout.LabelField("Active Location", validLocation ?  EditorHelper.CurrentlyActiveLocation.name : "null");

        GUI.color = Color.white;
        GUI.backgroundColor = Color.green;
        if(GUILayout.Button("Add Location Based Position"))
        {
            if (!validLocation)
            {
                Debug.LogError("No valid location selected. Please 'Align View' to the location you would like to work with.");
            } else
            {
                io.AddLocationBasedPosition(EditorHelper.CurrentlyActiveLocation.transform.position);
            }
            
        }
        GUI.backgroundColor = Color.blue;
        if (GUILayout.Button("Hide at This Location"))
        {
            EditorHelper.CurrentlyActiveLocation.RemoveVisibleObject(io);
            io.Hide();
        }

        if (GUILayout.Button("Show at This Location"))
        {
            EditorHelper.CurrentlyActiveLocation.AddVisibleObject(io);
            io.UnHide();
        }
    }



}

#endif
