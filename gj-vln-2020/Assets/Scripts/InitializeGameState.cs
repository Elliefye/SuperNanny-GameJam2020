using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InitializeGameState : MonoBehaviour
{
    void Awake()
    {
        if(Game.current == null && !File.Exists(Application.persistentDataPath + "/pdata.sn")) //no save file
        {
            Game.current = new Game();
            Game.current.Volume = 1;
            Game.current.FullScreen = Screen.fullScreen;
            Game.current.Money = 0;
            Game.current.RepairLevel = 0;
            Game.current.StrengthLevel = 0;
            Game.current.CleaningLevel = 0;
            Game.current.SweepingLevel = 0;
            Game.SaveGame();
        }
        else if (Game.current == null) //with save file
        {
            Game.LoadGame(); //load save file
            Screen.SetResolution(Screen.width, Screen.height, Game.current.FullScreen);
            AudioListener.volume = Game.current.Volume;
        }
    }
}
