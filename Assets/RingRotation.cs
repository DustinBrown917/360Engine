using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingRotation : MonoBehaviour
{

    [SerializeField] GameObject[] rotatingSprites;
    [SerializeField] Vector3 rotationVector;

    void FixedUpdate()
    {
        rotatingSprites[0].transform.Rotate(rotationVector);
        rotatingSprites[1].transform.Rotate(-rotationVector);
        rotatingSprites[2].transform.Rotate(rotationVector);
        rotatingSprites[3].transform.Rotate(-rotationVector);
    }
}
