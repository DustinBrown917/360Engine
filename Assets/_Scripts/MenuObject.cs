using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuObject : IdentifiableObject
{
    public UnityEvent interactedWith; 
    public override void Interact()
    {
        base.Interact();
        interactedWith.Invoke();
    }

    public override void Hide()
    {
        base.Hide();

        foreach(Transform t in transform)
        {
            t.gameObject.SetActive(false);
        }
    }

    public override void UnHide()
    {
        base.UnHide();

        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(true);
        }
    }
}
