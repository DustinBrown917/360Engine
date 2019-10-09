#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Location))]
[ExecuteInEditMode]
public class LocationEditorBehaviour : MonoBehaviour
{

    private Location location;


    private void OnEnable()
    {
        location = GetComponent<Location>();
        location.CheckForChildrenTeleporters();
    }

    private void LateUpdate()
    {
        location.UpdateAllTeleporterPositions();
        Vector3 newPos = location.transform.position;
        newPos.y = location.MinDynamicHeight;
        location.transform.position = newPos;
    }


}
#endif
