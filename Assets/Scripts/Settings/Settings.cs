using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public bool fullScreen;
    public Slider musicSlider;
    public Slider sfxSlider;
    
    [Header("UI Elements")]
    public GameObject sfxIconOn;
    public GameObject sfxIconOff;
    public GameObject musicIconOn;
    public GameObject musicIconOff;
    
    void Awake(){
        DontDestroyOnLoad(gameObject);
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        fullScreen = false;
    }

    public void OnMusicVolumeChanged(float newValue){
        if (newValue == 0){
            musicIconOn.SetActive(false);
            musicIconOff.SetActive(true);
        }
        else{
            musicIconOn.SetActive(true);
            musicIconOff.SetActive(false);
        }
        AudioManager.Instance.OnMusicVolumeChanged(newValue);
    }

    // Changes SFX volumes. Is triggered by SFX slider in settings.
    public void OnSFXVolumeChanged(float newValue){
        if (newValue == 0){
            sfxIconOn.SetActive(false);
            sfxIconOff.SetActive(true);
        }
        else{
            sfxIconOn.SetActive(true);
            sfxIconOff.SetActive(false);
        }
        AudioManager.Instance.OnSFXVolumeChanged(newValue);
    }

    public void loadScreen(bool settingsFullScreen){
        if (settingsFullScreen){
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            fullScreen = true;
        } else {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            fullScreen = false;
        }
    }

    public void loadVolumeSliders(float volumeBGM, float volumeSFX){
        musicSlider.value = volumeBGM;
        sfxSlider.value = volumeSFX;
    }

    // TODO: need to test if this works.
    public void setFullScreen(){
        if( !fullScreen ) {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            fullScreen = true;
        }
        else {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            fullScreen = false;
        }
    }

    // Loads in all save file UIs in the savePanel of the Settings menu
    public void LoadSaveFileUIs(){
        SaveData currSaveData;

        currSaveData = null;
        DataManager.readFile(ref currSaveData, 0);
        // Load in level datas for easier reference, since level id != elem index.
        if(currSaveData != null) // Triggers if save file exists for that save index.
        {
            
        }
    }

    public void LoadSaveFileUIsForTitleScreen(){
        SaveData currSaveData;
        currSaveData = null;

        DataManager.readFile(ref currSaveData, 0);
        // Load in level datas for easier reference, since level id != elem index.
        if(currSaveData != null) // Triggers if save file exists for that save index.
        {
                
        }
    }
}
