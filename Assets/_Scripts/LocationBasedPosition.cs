using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LocationBasedPosition
{
    public Location location;
    public Vector3 position;

    public LocationBasedPosition(Location l, Vector3 p)
    {
        location = l;
        position = p;
    }
}
