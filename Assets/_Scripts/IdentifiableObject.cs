using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class IdentifiableObject : MonoBehaviour
{
    [SerializeField] protected IdentifiableData data;
    [SerializeField] protected List<LocationBasedPosition> locationBasedPositions;
    [SerializeField] protected Collider[] collidersToHide;
    [SerializeField] protected Renderer[] renderersToHide;
    [SerializeField] protected GameObject[] childrenToHide;

    [SerializeField] protected AudioClip identifiedSound;
    [SerializeField] protected AudioClip interactedSound;

    private AudioSource audioSource;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        
    }

    public void UpdatePositionBasedOnLocation(Location l)
    {
        foreach(LocationBasedPosition lbp in locationBasedPositions) {
            if(lbp.location == l) {
                transform.position = l.transform.position + lbp.position;
                OnPositionUpdated();
                return;
            }
        }
    }

#if UNITY_EDITOR
    public void AddLocationBasedPosition(Vector3 anchor)
    {
        for(int i = 0; i < locationBasedPositions.Count; i++) {
            if(locationBasedPositions[i].location == EditorHelper.CurrentlyActiveLocation) {
                locationBasedPositions[i].position = transform.position - EditorHelper.CurrentlyActiveLocation.transform.position;
                EditorUtility.SetDirty(this);
                return;
            }
        }

        LocationBasedPosition lbp = new LocationBasedPosition(EditorHelper.CurrentlyActiveLocation,  transform.position - EditorHelper.CurrentlyActiveLocation.transform.position);
        locationBasedPositions.Add(lbp);

        EditorUtility.SetDirty(this);
    }
#endif

    public virtual IdentifiableData Identify()
    {
        if (data.name == "") {
            data.name = name;
        }

        if(identifiedSound != null) {
            audioSource.clip = identifiedSound;
            audioSource.Play();
        }

        OnIdentified();
        return data;
    }

    public virtual void Unidentify()
    {
        OnUnidentified();
    }

    public virtual void Hide()
    {
        foreach(Collider c in collidersToHide)
        {
            c.enabled = false;
        }

        foreach (Renderer r in renderersToHide)
        {
            r.enabled = false;
        }

        if(childrenToHide != null)
        {
            foreach (GameObject go in childrenToHide)
            {
                go.SetActive(false);
            }
        }
    }

    public virtual void UnHide()
    {
        foreach (Collider c in collidersToHide)
        {
            c.enabled = true;
        }

        foreach (Renderer r in renderersToHide)
        {
            r.enabled = true;
        }

        foreach(GameObject go in childrenToHide)
        {
            go.SetActive(true);
        }
    }

    public virtual void Interact()
    {
        if (interactedSound != null) {
            audioSource.clip = interactedSound;
            audioSource.Play();
        }

        OnInteractedWith();
    }





    public event EventHandler InteractedWith;

    protected void OnInteractedWith()
    {
        InteractedWith?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler Identified;

    protected void OnIdentified()
    {
        Identified?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler Unidentified;

    protected void OnUnidentified()
    {
        Unidentified?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler PositionUpdated;

    protected void OnPositionUpdated()
    {
        PositionUpdated?.Invoke(this, EventArgs.Empty);
    }

}
