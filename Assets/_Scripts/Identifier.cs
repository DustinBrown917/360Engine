using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class Identifier : MonoBehaviour
{
    [SerializeField] private Image fillBar;

    [SerializeField] private float fillSpeed = 1;

    [SerializeField] private LineRendererDraw[] lineRendererDraw;

    private Coroutine cr_TickFillAmount = null;

    private Outline lr;

    [SerializeField] private Color unidentifiedColour;
    [SerializeField] private Color identifiedColour;

    private void Awake()
    {
        lr = GetComponent<Outline>();
    }

    public void StartIdentify()
    {

        if(cr_TickFillAmount != null) { StopIdentify(); }

        fillBar.fillAmount = 0;
        cr_TickFillAmount = StartCoroutine(TickFillAmount());
        lr.effectColor = identifiedColour;
    }

    private IEnumerator TickFillAmount()
    {
        while (fillBar.fillAmount < 1)
        {
            fillBar.fillAmount += fillSpeed * Time.deltaTime;
            yield return null;
        }

        foreach(LineRendererDraw l in lineRendererDraw)
        {
            l.DrawLine();
        }
    }

    public void StopIdentify()
    {
        fillBar.fillAmount = 0;
        StopCoroutine(cr_TickFillAmount);
        cr_TickFillAmount = null;
        foreach (LineRendererDraw l in lineRendererDraw)
        {
            l.StopDrawLine();
        }

        lr.effectColor = unidentifiedColour;
    }
}
