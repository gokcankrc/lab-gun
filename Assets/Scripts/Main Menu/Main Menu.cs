using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField]GameObject mainContainer, optionsContainer;
    [SerializeField]Slider soundSlider, musicSlider;
    static bool firstSetupDone;
    bool startGame = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!firstSetupDone)
        {
            MusicManager.musicVolume = 0.4f;
            MusicManager.soundsVolume = 0.4f;
            firstSetupDone = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startGame)
        {
            DialogueManager.StartDialogueScene(0);
        }
    }
    public void StartGame()
    {
        //print ("starting game from main menu");
        startGame = true;
    }
    public void Quit ()
    {
        // it's going to be web-based, can't really quit...
    }
    void SetAllScreensInactive()
    {
        mainContainer.SetActive(false);
        optionsContainer.SetActive(false);
    }
    public void SetOptionsScreen()
    {
        SetScreen(MainMenuLayers.options);
    }
    public void SetMainScreen()
    {
        SetScreen(MainMenuLayers.main);
    }
    void SetScreen (MainMenuLayers layer)
    {
        SetAllScreensInactive();
        switch ((int)layer)
        {
            case (int)MainMenuLayers.main:
            {
                mainContainer.SetActive(true);
                break;
            }
            case (int)MainMenuLayers.options:
            {
                optionsContainer.SetActive(true);
                break;
            }
        }
    }
    public void SetMusicVolume ()
    {
        MusicManager.musicVolume = musicSlider.value;
    }
    public void SetSFXVolume ()
    {
        MusicManager.soundsVolume = soundSlider.value;
    }
}
[Serializable]
public enum MainMenuLayers
{
    main,
    options
}

