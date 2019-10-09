using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Teleporter : IdentifiableObject
{
    [SerializeField] private Location targetLocation_;
    public Location TargetLocation { get { return targetLocation_; } }
    [SerializeField] private Location ownerLocation;
    [SerializeField] private bool manuallyPlaced_ = false;
    public bool ManuallyPlaced { get { return manuallyPlaced_; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Teleport()
    {
        //Activates target location.
        targetLocation_.gameObject.SetActive(true);

        //Moves the player to the referenced position.
        PlayerController.Instance.TeleportToLocation(targetLocation_);

        //Turns off owner location.
        ownerLocation.gameObject.SetActive(false);
        //Do nothing after this! It won't happen!
    }

    public void SetOwner(Location location)
    {
        ownerLocation = location;
    }

    public void SetTargetLocation(Location location)
    {
        targetLocation_ = location;
    }

    public override void Interact()
    {
        Teleport();
    }




    public event EventHandler<TeleportArgs> Teleported;

    public class TeleportArgs : EventArgs
    {
        public Location newLocation;
        public Location oldLocation;

        public TeleportArgs(Location newL, Location oldL)
        {
            newLocation = newL;
            oldLocation = oldL;
        }
    }

    private void OnTeleported(TeleportArgs args)
    {
        EventHandler<TeleportArgs> handler = Teleported;

        if (handler != null)
        {
            handler(this, args);
        }
    }
}
