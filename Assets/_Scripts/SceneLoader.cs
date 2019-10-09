using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader instance_ = null;
    public static SceneLoader Instance { get { return instance_; } }

    [SerializeField] private string[] menuSceneNames;

    private void Awake()
    {
        if(instance_ == null) { instance_ = this; }
        else { Destroy(this); }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (IsMenuScene(scene))
        {
            VRManager.Instance.SetVREnabled(false);
        } else
        {
            VRManager.Instance.SetVREnabled(GameManager.Instance.IsUsingVRDevice);
        }


    }

    public void ChangeToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public bool IsMenuScene(Scene scene)
    {
        foreach(string s in menuSceneNames)
        {
            if(scene.name == s) { return true; }
        }

        return false;
    }

    private void OnDestroy()
    {
        if(instance_ == this) {
            instance_ = null;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }
    }


}
