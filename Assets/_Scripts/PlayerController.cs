using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance_;
    public static PlayerController Instance { get { return instance_; } }

    public Location CurrentLocation { get; private set; }
    [SerializeField] private float rayCastRange = 30.0f;
    [SerializeField] private float moveCastThresholdDistance = 3.0f;
    [SerializeField] private Location startingLocation_;

    [SerializeField] private DisplayPanel displayPanel;

    [SerializeField] private Identifier identifier;

    [SerializeField] private LayerMask importantObjectLayer;
    [SerializeField] private LayerMask teleporterLayer;

    public Location StartingLocation { get { return startingLocation_; } }

    private IdentifiableObject currentlyObservedObject = null;

    private TouchData[] touchData = new TouchData[5];

    [SerializeField] private CanvasScaler canvasScaler;

    /********************************************************************************************************/
    /******************************************* UNITY BEHAVIOURS *******************************************/
    /********************************************************************************************************/

    private void Awake()
    {
        //Singleton Pattern
        if(instance_ == null) {
            instance_ = this;
            MouseManager.MouseButtonRelease += MouseManager_MouseButtonRelease;
        }
        else { Destroy(gameObject); }
    }

    private void MouseManager_MouseButtonRelease(object sender, MouseManager.MouseButtonReleaseArgs e)
    {       
        if(e.index == 0)
        {
            if (e.duration < 0.5f && e.distanceTravelled <= moveCastThresholdDistance)
            {
                CastRayInteract();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Create a late initialization (to avoid script execution order issues)
        StartCoroutine(LateInitialization());

        //This ensures the UI appears within the user's field of vision when using cardboard.
        if (GameManager.Instance.IsUsingVRDevice)
        {
            canvasScaler.referenceResolution = canvasScaler.referenceResolution * 3.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            
            if (Input.touches[0].phase == TouchPhase.Ended && touchData[0].duration < 0.15f)
            {
                CastRayInteract();               
            }
        }

        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.touches[i].phase == TouchPhase.Ended)
            {
                touchData[Input.touches[i].fingerId].duration = 0;
                touchData[Input.touches[i].fingerId].distanceTravelled = 0;
                continue;
            }

            touchData[Input.touches[i].fingerId].duration += Time.deltaTime;
            touchData[Input.touches[i].fingerId].distanceTravelled += Input.touches[i].deltaPosition.magnitude;
        }
        
        CastRayIdentify();
    }

    private void OnDestroy()
    {
        if (instance_ == this) {
            instance_ = null;
            MouseManager.MouseButtonRelease -= MouseManager_MouseButtonRelease;
        }
    }

    /********************************************************************************************************/
    /********************************************** BEHAVIOURS **********************************************/
    /********************************************************************************************************/

    /// <summary>
    /// Cast a ray to interact with an InteractableObject.
    /// </summary>
    private void CastRayInteract()
    {
        RaycastHit hit;
        bool hitSuccess = Physics.Raycast(transform.position, transform.forward, out hit, rayCastRange, importantObjectLayer);
        if (!hitSuccess)
        {
            hitSuccess = Physics.Raycast(transform.position, transform.forward, out hit, rayCastRange, teleporterLayer);
        }

        IdentifiableObject io = null;
        if (hitSuccess)
        {
            io = hit.collider.gameObject.GetComponent<IdentifiableObject>();
        }

        if(io != null) {
            io.Interact();
            displayPanel.Interact();
        }
    }

    /// <summary>
    /// Cast a ray to identify an InteractableObject
    /// </summary>
    private void CastRayIdentify()
    {
        RaycastHit hit;

        bool hitSuccess = Physics.Raycast(transform.position, transform.forward, out hit, rayCastRange, importantObjectLayer);
        if (!hitSuccess) {
            hitSuccess = Physics.Raycast(transform.position, transform.forward, out hit, rayCastRange, teleporterLayer);
        }

        IdentifiableObject io = null;
        if (hitSuccess) {
            io = hit.collider.gameObject.GetComponent<IdentifiableObject>();
        } else if(currentlyObservedObject != null) {
            currentlyObservedObject.Unidentify();
            currentlyObservedObject = null;

            displayPanel.ResetPanel();
            displayPanel.gameObject.SetActive(false);

            identifier.StopIdentify();
        }

        if (io != null && io != currentlyObservedObject) {
            identifier.StartIdentify();
            if(currentlyObservedObject != null) { currentlyObservedObject.Unidentify(); }
            currentlyObservedObject = io;

            displayPanel.gameObject.SetActive(true);
            displayPanel.UpdateData(io.Identify());
            displayPanel.ResetPanel();
            displayPanel.Identify();
        }
    }

    /// <summary>
    /// Teleport the player to a location.
    /// </summary>
    /// <param name="l">The location to teleport the player to.</param>
    public void TeleportToLocation(Location l)
    {
        Vector3 newPos = l.transform.position;
        newPos.y = 0;
        transform.parent.position = newPos;
        OnTeleport(new TeleportArgs(CurrentLocation, l));

        //if(CurrentLocation != null) {
        //    CurrentLocation.HideObjects();
        //}
        

        CurrentLocation = l;

        //CurrentLocation.HideObjects();
        l.UpdateMaterial();

    }

    /// <summary>
    /// Get the X axis angle of the player.
    /// </summary>
    /// <returns>The X axis angle of the player in degrees.</returns>
    public float GetLookXAngle()
    {
        return transform.rotation.eulerAngles.x;
    }

    /// <summary>
    /// A late initializer coroutine to avoid Execution Order conflicts.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LateInitialization()
    {
        yield return new WaitForEndOfFrame();
        TeleportToLocation(startingLocation_);
    }

    /********************************************************************************************************/
    /************************************************ EVENTS ************************************************/
    /********************************************************************************************************/

    public event EventHandler<TeleportArgs> Teleport;

    public class TeleportArgs : EventArgs
    {
        public Location from;
        public Location to;

        public TeleportArgs(Location from, Location to)
        {
            this.from = from;
            this.to = to;
        }
    }

    public void OnTeleport(TeleportArgs args)
    {
        Teleport?.Invoke(this, args);
    }
}
