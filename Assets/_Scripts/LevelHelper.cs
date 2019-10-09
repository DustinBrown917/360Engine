using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHelper : MonoBehaviour
{

    public void LoadLevel(string levelName)
    {
        SceneLoader.Instance.ChangeToScene(levelName);
    }

    public void SetUseVRDevice(bool useDevice)
    {
        GameManager.Instance.SetIsUsingVRDevice(useDevice);
    }
}
