using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererDraw : MonoBehaviour
{
    private LineRenderer line;

    private Vector3[] targetPositions;

    [SerializeField]private float[] stageDurations;

    private Vector3 lineStartPosition;

    private Coroutine cr_DrawLine = null;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        lineStartPosition = line.GetPosition(0);
        AssignTargetPositions();
        SetPositionsToDefault();
    }

    private void SetPositionsToDefault()
    {
        for (int i = 0; i < line.positionCount; i++)
        {
            line.SetPosition(i, lineStartPosition);
        }
    }

    private void AssignTargetPositions()
    {
        targetPositions = new Vector3[line.positionCount];
        for (int i = 0; i < line.positionCount; i++)
        {
            targetPositions[i] = line.GetPosition(i);
        }
    }

    public void DrawLine()
    {
        StopDrawLine();
        cr_DrawLine = StartCoroutine(AnimateLine());
    }

    public void StopDrawLine()
    {
        if (cr_DrawLine != null) {
            StopCoroutine(cr_DrawLine);
            cr_DrawLine = null;
        }
        SetPositionsToDefault();
    }

    IEnumerator AnimateLine()
    {
        float timeElapsed = 0;


        for (int i = 0; i < line.positionCount; i++)
        {
            timeElapsed = 0;
            while (timeElapsed < stageDurations[i])
            {
                //Lerp the points new pos
                if(i > 0) { line.SetPosition(i, Vector3.Lerp(line.GetPosition(i - 1), targetPositions[i], timeElapsed / stageDurations[i])); }
                else { line.SetPosition(i, Vector3.Lerp(lineStartPosition, targetPositions[i], timeElapsed / stageDurations[i])); }
                
                //Set all following points to match point i
                for(int x = i + 1; x < line.positionCount; x++)
                {
                    line.SetPosition(x, line.GetPosition(i));
                }

                //accumulate deltatime
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            line.SetPosition(i, targetPositions[i]);
        }
    }
}
