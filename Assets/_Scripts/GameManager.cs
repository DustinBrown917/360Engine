using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance_;
    public static GameManager Instance { get { return instance_; } }

    [SerializeField] private bool isUsingVRDevice_ = false;
    public bool IsUsingVRDevice { get { return isUsingVRDevice_; } }

    [SerializeField] private bool debugMode_ = false;
    public bool DebugMode { get { return debugMode_; } }

    void Awake()
    {
        if(instance_ == null) { instance_ = this; }
        else { Destroy(this); }
    }

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if(instance_ == this) {
            instance_ = null;
            
        }
        Destroy(transform.GetComponent<DDOL>());
    }

    public void SetIsUsingVRDevice(bool usingVRDevice)
    {
        this.isUsingVRDevice_ = usingVRDevice;
    }
}
