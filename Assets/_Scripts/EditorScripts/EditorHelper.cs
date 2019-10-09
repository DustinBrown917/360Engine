#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[ExecuteInEditMode]
public class EditorHelper : MonoBehaviour
{

    private static Location currentlyActiveLocation_ = null;
    public static Location CurrentlyActiveLocation { get { return currentlyActiveLocation_; } }

    public static void SetCurrentlyActiveLocation(Location location)
    {
        if(currentlyActiveLocation_ != null) {
            currentlyActiveLocation_.HideObjects();
        }
        currentlyActiveLocation_ = location;
        currentlyActiveLocation_.ShowObjects();
        OnActiveLocationChanged();
    }

    public static event EventHandler ActiveLocationChanged;

    private static void OnActiveLocationChanged()
    {
        ActiveLocationChanged?.Invoke(typeof(EditorHelper), EventArgs.Empty);
    }

}
#endif
