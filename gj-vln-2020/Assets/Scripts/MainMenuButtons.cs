using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    [SerializeField]
    private Text StartBtnText;

    void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/pdata.sn"))
        {
            StartBtnText.text = "Continue";
        }
    }

    public void StartClicked()
    {
        //load next scene
    }

    public void OptionsClicked()
    {
        MainButtons.SetActive(false);
        OptionsMenu.SetActive(true);
        volumeSlider.value = Game.current.Volume;
        fullScreenToggle.isOn = Game.current.FullScreen;
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
        Game.current.Volume = volumeSlider.value;
    }

    public void ToggleFullScreen()
    {
        Screen.SetResolution(Screen.width, Screen.height, fullScreenToggle.isOn);
        Game.current.FullScreen = fullScreenToggle.isOn;
    }

    public void BackClicked()
    {
        Game.SaveGame();
        OptionsMenu.SetActive(false);
        MainButtons.SetActive(true);
    }
}
