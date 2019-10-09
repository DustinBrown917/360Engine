using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPanel : MonoBehaviour
{
    [SerializeField] private Image objectImage;
    [SerializeField] private Text objectName;
    [SerializeField] private Text objectDescription;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private float imageRevealTime = 0.5f;
    [SerializeField] private float nameRevealDuration = 0.5f;
    [SerializeField] private float startNameRevealTime = 0.5f;
    [SerializeField] private RectTransform descriptionMask;
    [SerializeField] private RectTransform imageMask;
    [SerializeField] private RectTransform nameMask;
    private float descriptionMaskFullHeight;
    private float imageMaskFullHeight;
    private float nameMaskWidth;

    private bool isDisplaying = false;

    private Coroutine cr_InteractionRoutine = null;
    private Coroutine cr_IdentifyRoutine = null;

    private void Start()
    {
        descriptionMaskFullHeight = descriptionMask.rect.height;
        imageMaskFullHeight = imageMask.rect.height;
        nameMaskWidth = nameMask.rect.width;
        StopInteractRoutine();
    }

    public void UpdateData(IdentifiableData data)
    {
        objectImage.sprite = data.objectImage == null ? defaultSprite : data.objectImage;
        objectName.text = data.name;
        objectDescription.text = data.description;
    }

    public void Interact()
    {
        if (!isDisplaying) {
            StopInteractRoutine();
            cr_InteractionRoutine = StartCoroutine(InteractRoutine());
        } else {
            StopInteractRoutine(); //Had to separate this into an IF because StopIdentifyRoutine() sets isDisplaying to false.
        }
    }

    public void Identify()
    {
        StopIdentifyRoutine();
        cr_IdentifyRoutine = StartCoroutine(IdentifyRoutine());
    }

    public void ResetPanel()
    {
        StopInteractRoutine();
        StopIdentifyRoutine();
    }

    private void StopInteractRoutine()
    {
        if(cr_InteractionRoutine != null) {
            StopCoroutine(cr_InteractionRoutine);
            cr_InteractionRoutine = null;
        }

        descriptionMask.sizeDelta = new Vector2(descriptionMask.rect.width, 0);
        imageMask.sizeDelta = new Vector2(imageMask.rect.width, 0);

        isDisplaying = false;
    }

    private void StopIdentifyRoutine()
    {
        if (cr_IdentifyRoutine != null)
        {
            StopCoroutine(cr_IdentifyRoutine);
            cr_IdentifyRoutine = null;
        }

        nameMask.sizeDelta = new Vector2(0, nameMask.rect.height);
    }

    private IEnumerator InteractRoutine()
    {
        isDisplaying = true;
        float elapsedTime = 0;
        float t = 0;

        while(elapsedTime < imageRevealTime)
        {
            t = elapsedTime / imageRevealTime;
            descriptionMask.sizeDelta = new Vector2(descriptionMask.rect.width, descriptionMaskFullHeight * t);
            imageMask.sizeDelta = new Vector2(imageMask.rect.width, imageMaskFullHeight * t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        descriptionMask.sizeDelta = new Vector2(descriptionMask.rect.width, descriptionMaskFullHeight);
        imageMask.sizeDelta = new Vector2(imageMask.rect.width, imageMaskFullHeight);

        cr_InteractionRoutine = null;
    }

    private IEnumerator IdentifyRoutine()
    {
        float elapsedTime = 0;
        float t = 0;

        while (elapsedTime < nameRevealDuration + startNameRevealTime)
        {
            if(elapsedTime > startNameRevealTime)
            {
                t = (elapsedTime - startNameRevealTime) / nameRevealDuration;
                nameMask.sizeDelta = new Vector2(nameMaskWidth * t, nameMask.rect.height);
                
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        nameMask.sizeDelta = new Vector2(nameMaskWidth, nameMask.rect.height);

        cr_IdentifyRoutine = null;
    }
}
