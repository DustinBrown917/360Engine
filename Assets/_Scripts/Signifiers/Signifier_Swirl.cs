using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signifier_Swirl : Signifier
{
    private ParticleSystem ps;

    [SerializeField] private Transform particleSystemContainer;

    [SerializeField] private float baseRotationSpeed = 180.0f;
    [SerializeField] private float identifiedRotationSpeed = 360.0f;

    private float currentRotationSpeed;

    protected override void Awake()
    {
        base.Awake();

        ps = particleSystemContainer.GetComponent<ParticleSystem>();

        currentRotationSpeed = baseRotationSpeed;
    }

    protected override void Start()
    {
        base.Start();
        

    }

    protected override void ParentIdentifiable_Identified(object sender, EventArgs e)
    {
        ParticleSystem.EmissionModule em = ps.emission;
        em.rateOverTimeMultiplier = 4.0f;
        currentRotationSpeed = identifiedRotationSpeed;
    }

    protected override void ParentIdentifiable_InteractedWith(object sender, EventArgs e)
    {
    }

    protected override void ParentIdentifiable_Unidentified(object sender, EventArgs e)
    {
        ParticleSystem.EmissionModule em = ps.emission;
        em.rateOverTimeMultiplier = 1.0f;
        currentRotationSpeed = baseRotationSpeed;
    }

    protected override void Update()
    {
        particleSystemContainer.Rotate(Vector3.forward, currentRotationSpeed * Time.deltaTime);
        transform.LookAt(PlayerController.Instance.transform);
    }

    protected override void ParentIdentifiable_PositionUpdated(object sender, EventArgs e)
    {
        ps.Clear();
    }
}
