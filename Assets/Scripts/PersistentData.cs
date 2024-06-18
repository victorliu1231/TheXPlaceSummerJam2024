using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PersistentData : MonoBehaviour
{
    public GameObject player;
    public AudioManager audioManager;
    public Settings settings;

    // currSaveData and currLevelDatas are private vars accessible through getters
    static SaveData currSaveData;

    // Save index 0 is always auto save; Save >= 1 (up to max) is manual save. 
    public void LoadSave(int saveIndex)
    {
        // Delete current in-game dynamic data and replace it with save file data
        currSaveData = null;

        DataManager.readFile(ref currSaveData, saveIndex);

        // Load in level datas for easier reference, since level id != elem index.
        if(currSaveData != null)
        {
            // Initialize settings from settings in currSaveData
            settings.fullScreen = currSaveData.fullScreen;
            settings.loadScreen(false);
            audioManager.volumeBGM = currSaveData.volumeBGM;
            audioManager.OnMusicVolumeChanged(audioManager.volumeBGM);
            audioManager.volumeSFX = currSaveData.volumeSFX;
            audioManager.OnSFXVolumeChanged(audioManager.volumeSFX);
            settings.loadVolumeSliders(audioManager.volumeBGM, audioManager.volumeSFX);
        }
        else
        {
            currSaveData = new SaveData();
            
        }
    }

    public static void WriteToSave(int saveIndex)
    {
        DataManager.writeFile(ref currSaveData, saveIndex);
    }

    // This function is subject to change and finalization as dynamic variables increase!
    public void CreateNewSave(int saveIndex) // things we ignore are automatically obvious default values.
    {
        SaveData newSave = new SaveData();

        

        // write the current save data to the saveIndex save
        DataManager.writeFile(ref newSave, saveIndex);
    }
}
