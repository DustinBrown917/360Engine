using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeToMove : MonoBehaviour
{
#if UNITY_IOS || UNITY_ANDROID || UNITY_WEBGL
    private Vector3 touchPosition;

    private const string AXIS_MOUSE_X = "Touch X";
    private const string AXIS_MOUSE_Y = "Touch Y";

    private float mouseX = 0;
    private float mouseY = 0;
    private float mouseZ = 0;

    [SerializeField] private float mouseSpeedModifier = 0.1f;
    [SerializeField] private float swipeSpeedModifier = 0.075f;

    private static readonly Vector3 NECK_OFFSET = new Vector3(0, 0.075f, 0.08f);

    public Vector3 HeadPosition { get; private set; }

    public Quaternion HeadRotation { get; private set; }

    private static Camera[] allCameras = new Camera[32];

    private Vector3 currentVelocity;
    [SerializeField] private float deccelerationRate;
    [SerializeField] private float minMoveDistanceThresholdTouch;

    private Vector2 previousMousePos;
    private Vector2 currentMousePos;
    private Vector2 deltaMousePos { get { return currentMousePos - previousMousePos; } }

    private void Update()
    {
        if (GameManager.Instance.IsUsingVRDevice) { return; }

        if (currentVelocity.magnitude > 0)
        {
            currentVelocity *= deccelerationRate;
        }

        if (Input.GetMouseButtonDown(0))
        {
            previousMousePos = currentMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            currentMousePos = Input.mousePosition;
            if (currentMousePos != previousMousePos)
            {

                currentVelocity.x = deltaMousePos.x * -mouseSpeedModifier;
                currentVelocity.y = deltaMousePos.y * -mouseSpeedModifier;

            }
            previousMousePos = currentMousePos;
        }



        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Moved)
            {
                if(Input.touches[0].deltaPosition.magnitude >= minMoveDistanceThresholdTouch)
                {
                    currentVelocity.x = Input.touches[0].deltaPosition.x * -swipeSpeedModifier;
                    currentVelocity.y = Input.touches[0].deltaPosition.y * -swipeSpeedModifier;
                }
            }
        }

        mouseX += currentVelocity.x;
        if (mouseX <= -180) { mouseX += 360; }
        else if (mouseX > 180) { mouseX -= 360; }

        mouseY -= currentVelocity.y;
        mouseY = Mathf.Clamp(mouseY, -85, 85);

        UpdateHeadPositionAndRotation();
        ApplyHeadOrientationToVRCameras();
    }

    private void UpdateHeadPositionAndRotation()
    {
        HeadRotation = Quaternion.Euler(mouseY, mouseX, mouseZ);
        HeadPosition = (HeadRotation * NECK_OFFSET) - (NECK_OFFSET.y * Vector3.up);
    }


    private void ApplyHeadOrientationToVRCameras()
    {
        UpdateAllCameras();

        // Update all VR cameras using Head position and rotation information.
        for (int i = 0; i < Camera.allCamerasCount; ++i)
        {
            Camera cam = allCameras[i];

            // Check if the Camera is a valid VR Camera, and if so update it to track head motion.
            if (cam && cam.enabled && cam.stereoTargetEye != StereoTargetEyeMask.None)
            {
                cam.transform.localPosition = HeadPosition * cam.transform.lossyScale.y;
                cam.transform.localRotation = HeadRotation;
            }
        }

        LineRenderer lr = new LineRenderer();
    }

    private void UpdateAllCameras()
    {
        // Get all Cameras in the scene using persistent data structures.
        if (Camera.allCamerasCount > allCameras.Length)
        {
            int newAllCamerasSize = Camera.allCamerasCount;
            while (Camera.allCamerasCount > newAllCamerasSize)
            {
                newAllCamerasSize *= 2;
            }

            allCameras = new Camera[newAllCamerasSize];
        }

        // The GetAllCameras method doesn't allocate memory (Camera.allCameras does).
        Camera.GetAllCameras(allCameras);
    }

    public void Recenter()
    {
        mouseX = mouseZ = 0;  // Do not reset pitch, which is how it works on the phone.
        UpdateHeadPositionAndRotation();
        ApplyHeadOrientationToVRCameras();
    }
#endif
}
