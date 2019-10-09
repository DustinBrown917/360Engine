using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Signifier : MonoBehaviour
{
    //Placeholder script in case we want to add some more indpendent functionality to the signifier

    private IdentifiableObject parentIdentifiable;

    protected virtual void Awake()
    {
        parentIdentifiable = GetComponentInParent<IdentifiableObject>();

        parentIdentifiable.Identified += ParentIdentifiable_Identified;
        parentIdentifiable.InteractedWith += ParentIdentifiable_InteractedWith;
        parentIdentifiable.Unidentified += ParentIdentifiable_Unidentified;
        parentIdentifiable.PositionUpdated += ParentIdentifiable_PositionUpdated;
    }

    

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    protected abstract void ParentIdentifiable_Unidentified(object sender, System.EventArgs e);

    protected abstract void ParentIdentifiable_InteractedWith(object sender, System.EventArgs e);

    protected abstract void ParentIdentifiable_Identified(object sender, System.EventArgs e);

    protected abstract void ParentIdentifiable_PositionUpdated(object sender, System.EventArgs e);
}
