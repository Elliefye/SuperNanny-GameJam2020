using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject MainButtons;
    public GameObject OptionsMenu;

    [SerializeField]
    private Slider volumeSlider;
    [SerializeField]
    private Toggle fullScreenToggle;

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
        MainButtons.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    public void ExitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }

    public void ToggleFullScreen()
    {
        Screen.SetResolution(Screen.width, Screen.height, fullScreenToggle.isOn);
    }

    public void BackClicked()
    {
        //save options to file
        OptionsMenu.SetActive(false);
        MainButtons.SetActive(true);
    }
}
