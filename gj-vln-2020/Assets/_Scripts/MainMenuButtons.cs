using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject MainButtons;
    public GameObject OptionsMenu;

    [SerializeField]
    private Slider volumeSlider = null;
    [SerializeField]
    private Toggle fullScreenToggle;

    public void StartClicked()
    {
        SceneManager.LoadScene(1);
    }
    void OnEnable()
    {
        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            Time.timeScale = 0;
        }
    }
    void OnDisable()
    {
        Time.timeScale = 1;
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
