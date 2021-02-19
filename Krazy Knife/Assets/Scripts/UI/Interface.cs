using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Interface : MonoBehaviour
{

    public GameObject homeMenu;
    public Text stageRecordText;
    public Text hitRecordText;
    [Space]
    public GameObject gameUI;
    public Text hitCountText;
    public KnifCount knifCountBar;
    public StageStatus stageStatus;
    public GameObject bossFaght;
    [Space]
    public GameObject defetMenu;
    public Text hitText;
    public Text stageText;
    [Space]
    public GameObject setingMenu;
    public Toggle vibration;
    public Toggle sounds;
    public Toggle music;
    [Space]
    public GameObject confirmWindow;
    [Space]
    public Text appleCountText;

    public Setings gameSeting;
    public GameController gameController;
    public AudioMixerGroup mixer;
    public AudioSource aS;

    void Start()
    {
        appleCountText.text = Convert.ToString(gameController.saveData.AppleCout);
        Music(gameController.saveData.music);
        Sounds(gameController.saveData.sounds);
        music.isOn = gameController.saveData.music;
        sounds.isOn = gameController.saveData.sounds;
        vibration.isOn = gameController.saveData.vbration;
        SetHomeMenu();
    }
    public void SetHomeMenu()
    {
        homeMenu.SetActive(true);
        gameUI.SetActive(false);
        defetMenu.SetActive(false);
        setingMenu.SetActive(false);
        confirmWindow.SetActive(false);
        stageRecordText.text = "STAGE " + gameController.saveData.maxStage;
        hitRecordText.text = "HITS " + gameController.saveData.maxHit;
    }
    public void SetGameUI()
    {
        aS.Stop();
        homeMenu.SetActive(false);
        gameUI.SetActive(true);
        defetMenu.SetActive(false);
        setingMenu.SetActive(false);
        confirmWindow.SetActive(false);
        knifCountBar.SetMaxKnifs(gameController.carrentKnifCount);
        stageStatus.SetStage(gameController.progresStage);
        hitCountText.text = Convert.ToString(gameController.hitCount);
    }
    public void SetDefetMenu()
    {
        homeMenu.SetActive(false);
        gameUI.SetActive(true);
        defetMenu.SetActive(true);
        setingMenu.SetActive(false);
        confirmWindow.SetActive(false);
        hitText.text = Convert.ToString(gameController.hitCount);
        stageText.text = "STAGE " + gameController.progresStage;
    }
    public void SetSetingMenu()
    {
        homeMenu.SetActive(false);
        gameUI.SetActive(false);
        defetMenu.SetActive(false);
        setingMenu.SetActive(true);
        confirmWindow.SetActive(false);
    }
    public void SetConfirmWindow()
    {
        homeMenu.SetActive(false);
        gameUI.SetActive(false);
        defetMenu.SetActive(false);
        setingMenu.SetActive(true);
        confirmWindow.SetActive(true);
    }
    public void ResetProgres()
    {
        gameController.saveData.AppleCout = 0;
        gameController.saveData.maxStage = 0;
        gameController.saveData.maxHit = 0;
        gameController.saveData.Save();
        appleCountText.text = Convert.ToString(gameController.saveData.AppleCout);
        stageRecordText.text = "STAGE " + gameController.saveData.maxStage;
        hitRecordText.text = "HITS " + gameController.saveData.maxHit;
    }
    public void Sounds(bool enabled)
    {
        if (enabled) mixer.audioMixer.SetFloat("Saunds", 0f);
        else mixer.audioMixer.SetFloat("Saunds", -80f);

        gameController.saveData.sounds = enabled;
        gameController.saveData.Save();
    }
    public void Music(bool enabled)
    {
        if (enabled) mixer.audioMixer.SetFloat("Music", 0f);
        else mixer.audioMixer.SetFloat("Music", -80f);

        gameController.saveData.music = enabled;
        gameController.saveData.Save();
    }
    public void Vibration(bool enabled)
    {
        if (enabled) Handheld.Vibrate();
        gameController.saveData.vbration = enabled;
        gameController.saveData.Save();
    }
}
