using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class Game
{
    public static Game current;

    //options
    public float Volume;
    public bool FullScreen;

    //player stats
    public int Money;
    public int RepairLevel;
    public int StrengthLevel;
    public int CleaningLevel;
    public int StaminaLevel;

    public static void SaveGame()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/pdata.sn");
            bf.Serialize(file, current);
            file.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public static void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/pdata.sn"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/pdata.sn", FileMode.Open);
            current = (Game)bf.Deserialize(file);
            file.Close();
        }
    }
}
