using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Toggle cardboardToggle;
    // Start is called before the first frame update
    void Start()
    {
        cardboardToggle.isOn = GameManager.Instance.IsUsingVRDevice;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Quit()
    {
        Application.Quit();
    }
}
