using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private static MouseManager instance_ = null;
    public static MouseManager Instance { get { return instance_; } }

    public static Vector2 currentMousePos { get { return Input.mousePosition; } }
    public static Vector2 previousMousePos { get; private set; }
    public static Vector2 deltaMousePos { get { return currentMousePos - previousMousePos; } }
    public static Vector2 currentVelocity { get { return deltaMousePos * Time.deltaTime; } }

    private static float[] mouseClickDuration;
    private static Vector2[] mouseClickStartPos;
    private static float[] mouseDistanceTravelled;

    private void Awake()
    {
        if(instance_ == null) {
            instance_ = this;
            mouseClickDuration = new float[3];
            mouseClickStartPos = new Vector2[3];
            mouseDistanceTravelled = new float[3];
        } else {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if(instance_ == this) {
            instance_ = null;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            mouseClickDuration[0] = 0;
            mouseClickStartPos[0] = Input.mousePosition;
            mouseDistanceTravelled[0] = 0;
            OnMouseButtonPressed(new MouseButtonPressedArgs(0, Input.mousePosition));
        }
        else if(Input.GetMouseButton(0)) {
            mouseClickDuration[0] += Time.deltaTime;
            mouseDistanceTravelled[0] += deltaMousePos.magnitude;
        }
        else if (Input.GetMouseButtonUp(0)) {
            OnMouseButtonRelease(new MouseButtonReleaseArgs(0, mouseClickDuration[0], mouseDistanceTravelled[0], Vector2.Distance(Input.mousePosition, mouseClickStartPos[0])));
        }
           


        if (Input.GetMouseButtonDown(1)) {
            mouseClickDuration[1] = 0;
            mouseClickStartPos[1] = Input.mousePosition;
            mouseDistanceTravelled[1] = 0;
            OnMouseButtonPressed(new MouseButtonPressedArgs(1, Input.mousePosition));
        }
        else if (Input.GetMouseButton(1)) {
            mouseClickDuration[1] += Time.deltaTime;
            mouseDistanceTravelled[1] += deltaMousePos.magnitude;
        }
        else if (Input.GetMouseButtonUp(1)) {
            OnMouseButtonRelease(new MouseButtonReleaseArgs(1, mouseClickDuration[1], mouseDistanceTravelled[1], Vector2.Distance(Input.mousePosition, mouseClickStartPos[1])));
        }



        if (Input.GetMouseButtonDown(2)) {
            mouseClickDuration[2] = 0;
            mouseClickStartPos[2] = Input.mousePosition;
            mouseDistanceTravelled[2] = 0;
            OnMouseButtonPressed(new MouseButtonPressedArgs(2, Input.mousePosition));
        }
        else if (Input.GetMouseButton(2)) {
            mouseClickDuration[2] += Time.deltaTime;
            mouseDistanceTravelled[2] += deltaMousePos.magnitude;
        }
        else if (Input.GetMouseButtonUp(2)) {
            OnMouseButtonRelease(new MouseButtonReleaseArgs(2, mouseClickDuration[2], mouseDistanceTravelled[2], Vector2.Distance(Input.mousePosition, mouseClickStartPos[2])));
        }
    }

    private void LateUpdate()
    {
        previousMousePos = currentMousePos;
    }


    /********************************************************************************************************/
    /************************************************ EVENTS ************************************************/
    /********************************************************************************************************/

    public static event EventHandler<MouseButtonPressedArgs> MouseButtonPressed;

    public class MouseButtonPressedArgs : EventArgs
    {
        public int index;
        public Vector2 location;

        public MouseButtonPressedArgs(int index_, Vector2 location_)
        {
            index = index_;
            location = location_;
        }
    }
    
    private void OnMouseButtonPressed(MouseButtonPressedArgs args)
    {
        MouseButtonPressed?.Invoke(this, args);
    }



    public static event EventHandler<MouseButtonReleaseArgs> MouseButtonRelease;

    public class MouseButtonReleaseArgs : EventArgs
    {
        public int index;
        public float duration;
        public float distanceTravelled;
        public float linearDistanceTravelled;

        public MouseButtonReleaseArgs(int index_, float duration_, float distanceTravelled_, float linearDistanceTravelled_) : base()
        {
            index = index_;
            duration = duration_;
            distanceTravelled = distanceTravelled_;
            linearDistanceTravelled = linearDistanceTravelled_;
        }
    }

    private void OnMouseButtonRelease(MouseButtonReleaseArgs args)
    {
        MouseButtonRelease?.Invoke(this, args);
    }
}
