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
    public Toggle fullScreenToggleCheckbox;
    public GameObject quitPanel;
    public Slider musicSlider;
    public Slider sfxSlider;
    
    void Awake(){
        DontDestroyOnLoad(gameObject);
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        fullScreen = false;
    }

    public void loadScreen(bool settingsFullScreen){
        fullScreenToggleCheckbox.isOn = settingsFullScreen;
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
