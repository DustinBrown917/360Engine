using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{

    private static IdentifiableObject[] identifiableObjects;
    private static Location[] locations;

    private void Awake()
    {
        identifiableObjects = FindObjectsOfType<IdentifiableObject>();
        locations = FindObjectsOfType<Location>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        InitLocations();
        PlayerController.Instance.Teleport += PlayerController_Teleport; 
    }

    private void PlayerController_Teleport(object sender, PlayerController.TeleportArgs e)
    {
        foreach(IdentifiableObject io in identifiableObjects)
        {
            if (io.gameObject.activeSelf) { io.UpdatePositionBasedOnLocation(e.to); }

            if (e.to.IsHiddenHere(io)) {
                io.Hide();
            } else {
                io.UnHide();
            }
        }
    }

    private void InitLocations()
    {
        foreach (Location l in locations)
        {
            if (l != PlayerController.Instance.StartingLocation)
            {
                l.gameObject.SetActive(false);
            }
        }
    }

    
}
