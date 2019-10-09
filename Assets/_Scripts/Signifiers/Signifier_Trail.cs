using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signifier_Trail : Signifier
{


    [SerializeField] private Transform trailPivot;
    [SerializeField] private TrailRenderer trailRenderer1;
    [SerializeField] private TrailRenderer trailRenderer2;

    [SerializeField] private float baseRotationSpeed = 180.0f;
    [SerializeField] private float identifiedRotationSpeed = 360.0f;
    [SerializeField] private float baseTrailDuration;
    [SerializeField] private float extendedTrailDuration;

    private float currentRotationSpeed;

    protected override void Awake()
    {
        base.Awake();

        currentRotationSpeed = baseRotationSpeed;

        trailRenderer1.time = baseTrailDuration;
        trailRenderer2.time = baseTrailDuration;

    }

    protected override void ParentIdentifiable_Identified(object sender, EventArgs e)
    {
        currentRotationSpeed = identifiedRotationSpeed;
        trailRenderer1.time = extendedTrailDuration;
        trailRenderer2.time = extendedTrailDuration;
    }

    protected override void ParentIdentifiable_InteractedWith(object sender, EventArgs e)
    {
        
    }

    protected override void ParentIdentifiable_PositionUpdated(object sender, EventArgs e)
    {
        trailRenderer1.Clear();
        trailRenderer2.Clear();
    }

    protected override void ParentIdentifiable_Unidentified(object sender, EventArgs e)
    {
        currentRotationSpeed = baseRotationSpeed;
        trailRenderer1.time = baseTrailDuration;
        trailRenderer2.time = baseTrailDuration;
    }

    protected override void Start()
    {
        base.Start();


    }

    protected override void Update()
    {
        trailPivot.Rotate(Vector3.forward, currentRotationSpeed * Time.deltaTime);
        transform.LookAt(PlayerController.Instance.transform);
    }
}
