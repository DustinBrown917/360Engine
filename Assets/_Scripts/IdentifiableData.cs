using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct IdentifiableData
{
    public string name;
    [TextArea] public string description;
    public Sprite objectImage;
}
