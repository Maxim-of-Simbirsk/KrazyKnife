using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveData : MonoBehaviour
{
    [HideInInspector] public int AppleCout = 0;
    [HideInInspector] public int maxStage = 0;
    [HideInInspector] public int maxHit = 0;
    [HideInInspector] public bool vbration = true;
    [HideInInspector] public bool sounds = true;
    [HideInInspector] public bool music = true;
    void Awake()
    {
        Load();
    }
    public void Save()
    {
        PlayerPrefs.SetString("Save", AppleCout + "," + maxStage + "," + maxHit + "," + vbration + "," + sounds + "," + music);
    }
    public void Load()
    {
        string[] saveData = PlayerPrefs.GetString("Save").Split(',');
        AppleCout = Int32.Parse(saveData[0]);
        maxStage = Int32.Parse(saveData[1]);
        maxHit = Int32.Parse(saveData[2]);
        vbration = saveData[3] == "True";
        sounds = saveData[4] == "True";
        music = saveData[5] == "True";
    }
}
