using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Location : MonoBehaviour
{
    [SerializeField] private List<IdentifiableObject> visibleObjects;
    [SerializeField] private Location[] accessibleLocations;
    [SerializeField] private GameObject teleporterPrefab;
    [SerializeField] private Material skyboxMat;
    [SerializeField] private List<Teleporter> teleporters;
    [SerializeField] private float teleporterOffset = 3;
    [SerializeField] private float maxLookAngle = 70;
    [SerializeField] private float maxDynamicHeight = 2;
    [SerializeField] private float minDynamicHeight = -2;
    public float MinDynamicHeight { get { return minDynamicHeight; } }

    private void Awake()
    {
#if UNITY_EDITOR
        GetComponent<LocationEditorBehaviour>().enabled = false;
#endif
        CheckForChildrenTeleporters();
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnTeleportersIfNeeded();
    }

    private void Update()
    {
        if (GameManager.Instance.DebugMode) {
            Vector3 newPos = transform.position;
            newPos.y = minDynamicHeight;
            transform.position = newPos;
            UpdateAllTeleporterPositions();
        }
        else {
            return;
            //Code to make the location position change based on player look angle.
            Vector3 newPos = transform.position;

            newPos.y = Mathf.Lerp(maxDynamicHeight, minDynamicHeight, Mathf.Abs(PlayerController.Instance.GetLookXAngle() - 290) / maxLookAngle);

            transform.position = newPos;
        }
    }


    public void CheckForChildrenTeleporters()
    {
        foreach(Transform t in transform)
        {
            Teleporter tel = t.GetComponent<Teleporter>();
            if (tel != null)
            {
                tel.SetOwner(this);
                if (!teleporters.Contains(tel)) {
                    teleporters.Add(tel);
                }
                
            }
        }
    }

    private void SpawnTeleporters()
    {
        foreach(Location l in accessibleLocations)
        {
            GameObject tGO = Instantiate(teleporterPrefab, this.transform);

            Teleporter t = tGO.GetComponent<Teleporter>();
            t.SetOwner(this);
            t.SetTargetLocation(l);

            t.transform.Rotate(Vector3.up, Vector2.Angle(t.transform.position, l.transform.position));

            Vector3 dirNormal = (l.transform.position - transform.position).normalized;
            Vector3 newPos = dirNormal * teleporterOffset;

            tGO.transform.localPosition = newPos;

            teleporters.Add(t);
        }
    }

    public void SpawnTeleportersIfNeeded()
    {
        foreach(Location l in accessibleLocations)
        {
            bool hasTeleporter = false;
            foreach(Teleporter t in teleporters)
            {
                if(t.TargetLocation == l)
                {
                    hasTeleporter = true;
                    break;
                }
            }

            if (hasTeleporter) { continue; }

            SpawnTeleporter(l);
        }
    }
    
    public void SpawnTeleporter(Location targetLocation)
    {
        Teleporter tl = Instantiate(teleporterPrefab, this.transform).GetComponent<Teleporter>();

        tl.SetOwner(this);
        tl.SetTargetLocation(targetLocation);

        UpdateTeleporterPosition(tl);

        teleporters.Add(tl);
    }

    public void UpdateAllTeleporterPositions()
    {
        foreach(Teleporter t in teleporters)
        {
            if (t.ManuallyPlaced) { continue; }
            UpdateTeleporterPosition(t);
        }
    }

    public void UpdateTeleporterPosition(Teleporter t)
    {
        Location l = t.TargetLocation;
        t.transform.LookAt(new Vector3(l.transform.position.x, t.transform.position.y, l.transform.position.z), Vector3.up);

        Vector3 dirNormal = (l.transform.position - transform.position).normalized;
        Vector3 newPos = dirNormal * teleporterOffset;

        t.transform.localPosition = newPos;
    }

    public void UpdateMaterial()
    {
        RenderSettings.skybox = skyboxMat;
    }


    public void AddVisibleObject(IdentifiableObject io)
    {
        if (visibleObjects.Contains(io)) { return; }

        visibleObjects.Add(io);

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    public void RemoveVisibleObject(IdentifiableObject io)
    {
        visibleObjects.Remove(io);

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    public void ShowObjects()
    {
        foreach (IdentifiableObject io in visibleObjects)
        {
            io.UnHide();
        }
    }

    public void HideObjects()
    {
        foreach (IdentifiableObject io in visibleObjects)
        {
            io.Hide();
        }
    }


    public void RemoveAllTeleporters()
    {
        for(int i = 0; i < teleporters.Count; i++)
        {
            DestroyImmediate(teleporters[i].gameObject);
        }

        teleporters.Clear();
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    public bool IsHiddenHere(IdentifiableObject io)
    {
        return !visibleObjects.Contains(io);
    }
}


