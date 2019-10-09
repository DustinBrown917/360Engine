using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signifier_Line : Signifier 
{

    private LineRenderer lineRenderer;
    [SerializeField] private int lineSegments = 360;
    [SerializeField] private float baseRadius = 0.1f;
    [SerializeField] private float identifiedRadius = 1.0f;
    [SerializeField] private float lineWidth = 0.2f;
    [SerializeField] private float fullLerpTime = 0.4f;
    private float currentRadius;

    private Coroutine cr_lerpToRadius = null;

    protected override void Awake()
    {
        base.Awake();

        lineRenderer = GetComponentInChildren<LineRenderer>();

        InitCircle();
    }

    protected override void Update()
    {
        base.Update();
        //trailPivot.Rotate(Vector3.forward, currentRotationSpeed * Time.deltaTime);
        transform.LookAt(PlayerController.Instance.transform);
    }

    public void InitCircle()
    {
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = lineSegments + 1;

        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] points = new Vector3[lineRenderer.positionCount];

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            float rad = Mathf.Deg2Rad * (i * 360f / lineSegments);
            points[i] = new Vector3(Mathf.Cos(rad) * baseRadius, Mathf.Sin(rad) * baseRadius);
        }

        lineRenderer.SetPositions(points);
        currentRadius = baseRadius;
    }

    public void UpdateRadius(float radius)
    {
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            float rad = Mathf.Deg2Rad * (i * 360f / lineSegments);
            lineRenderer.SetPosition(i, new Vector3(Mathf.Cos(rad) * radius, Mathf.Sin(rad) * radius));
        }

        currentRadius = radius;
    }

    
    private IEnumerator LerpToRadius(float lerpTime, float targetRadius)
    {
        
        float elaspedTime = 0;
        float initialRadius = currentRadius;

        while(elaspedTime < lerpTime)
        {
            UpdateRadius(Mathf.Lerp(initialRadius, targetRadius, elaspedTime/lerpTime));
            elaspedTime += Time.deltaTime;
            yield return null;
        }

        UpdateRadius(targetRadius);
    }

    private void StartLerpToRadius(float lerpTime, float targetRadius)
    {
        StopLerpToRadius();
        if (!gameObject.activeSelf) { return; }
        cr_lerpToRadius = StartCoroutine(LerpToRadius(lerpTime, targetRadius));
    }

    private void StopLerpToRadius()
    {
        if(cr_lerpToRadius != null)
        {
            StopCoroutine(cr_lerpToRadius);
            cr_lerpToRadius = null;
        }
    }

    protected override void ParentIdentifiable_Identified(object sender, EventArgs e)
    {
        StartLerpToRadius(fullLerpTime * (1.0f - ((currentRadius - baseRadius)/(identifiedRadius - baseRadius))), identifiedRadius);
    }

    protected override void ParentIdentifiable_InteractedWith(object sender, EventArgs e)
    {
        
    }

    protected override void ParentIdentifiable_PositionUpdated(object sender, EventArgs e)
    {
        
    }

    protected override void ParentIdentifiable_Unidentified(object sender, EventArgs e)
    {
        StartLerpToRadius(fullLerpTime * ((currentRadius - baseRadius) / (identifiedRadius - baseRadius)), baseRadius);
    }
}
