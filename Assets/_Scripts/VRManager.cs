using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRManager : MonoBehaviour
{
    private static VRManager instance_;
    public static VRManager Instance { get { return instance_; } }

    private bool vrEnabled = false;

    private void Awake()
    {
        if (instance_ == null) { instance_ = this; }
        else { Destroy(this); }
        XRSettings.enabled = false;
    }

    private void OnDestroy()
    {
        if(instance_ == this) { instance_ = null; }
    }

    public void SetVREnabled(bool vrEnabled)
    {
        XRSettings.enabled = this.vrEnabled = vrEnabled;
    }
}
