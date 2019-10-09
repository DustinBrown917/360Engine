#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class IdentifiableObjectEditorBehaviour : MonoBehaviour
{

    private IdentifiableObject identifiableObject;
    // Start is called before the first frame update
    void Start()
    {
        EditorHelper.ActiveLocationChanged += EditorHelper_ActiveLocationChanged;
    }

    private void OnEnable()
    {
        identifiableObject = GetComponent<IdentifiableObject>();
    }

    private void EditorHelper_ActiveLocationChanged(object sender, System.EventArgs e)
    {
        identifiableObject.UpdatePositionBasedOnLocation(EditorHelper.CurrentlyActiveLocation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        EditorHelper.ActiveLocationChanged -= EditorHelper_ActiveLocationChanged;
    }


}
#endif
