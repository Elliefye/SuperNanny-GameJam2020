using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField]
    private Button StartBtn;
    [SerializeField]
    private Button OptionsBtn;
    [SerializeField]
    private Button ExitBtn;

    // Start is called before the first frame update
    void Start()
    {
        //if save file found change start btn text
    }

    public void StartClicked()
    {
        //load next scene
    }

    public void OptionsClicked()
    {
        //load options menu
    }

    public void ExitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
