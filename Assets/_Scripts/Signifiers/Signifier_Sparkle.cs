using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Signifier_Sparkle : Signifier
{
    private ParticleSystem ps;

    protected override void Awake()
    {
        base.Awake();
        ps = GetComponent<ParticleSystem>();
    }

    protected override void ParentIdentifiable_Identified(object sender, EventArgs e)
    {
        ParticleSystem.EmissionModule em = ps.emission;
        em.rateOverTimeMultiplier = 4.0f;
    }

    protected override void ParentIdentifiable_InteractedWith(object sender, EventArgs e)
    {
        
    }

    protected override void ParentIdentifiable_PositionUpdated(object sender, EventArgs e)
    {
        ps.Clear();
    }

    protected override void ParentIdentifiable_Unidentified(object sender, EventArgs e)
    {
        ParticleSystem.EmissionModule em = ps.emission;
        em.rateOverTimeMultiplier = 1.0f;
    }
}
