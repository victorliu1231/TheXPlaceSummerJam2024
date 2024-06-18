using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PersistentData : MonoBehaviour
{
    public AudioManager audioManager;
    public Settings settings;
    public Transform contentPanel;
    public GameObject numOneBanner;
    public GameObject numTwoBanner;
    public GameObject numThreeBanner;
    public GameObject genericNumBanner;
    public GameObject noSavesYet;
    public string playerName;

    // currSaveData and currLevelDatas are private vars accessible through getters
    static SaveData currSaveData;

    void Awake(){
        DontDestroyOnLoad(gameObject);
    }

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

            // Clear out previous high scores
            foreach (Transform child in contentPanel)
            {
                Destroy(child.gameObject);
            }

            if (currSaveData.playerDatas.Count == 0){
                noSavesYet.SetActive(true);
            } else {
                // Add in previous high scores
                GameObject newPlayerData;
                currSaveData.playerDatas = currSaveData.playerDatas.OrderBy(playerData => playerData.playerTotalTime).ToList();
                for (int i = 0; i < currSaveData.playerDatas.Count; i++)
                {
                    if (i == 0){
                        newPlayerData = Instantiate(numOneBanner, contentPanel);
                    } else if (i == 1){
                        newPlayerData = Instantiate(numTwoBanner, contentPanel);
                    } else if (i == 2){
                        newPlayerData = Instantiate(numThreeBanner, contentPanel);
                    } else {
                        newPlayerData = Instantiate(genericNumBanner, contentPanel);
                    }
                    HighScoreBanner highScoreBanner = newPlayerData.GetComponent<HighScoreBanner>();
                    highScoreBanner.playerNameText.text = currSaveData.playerDatas[i].playerName;
                    highScoreBanner.playerLevelText.text = $"Level {currSaveData.playerDatas[i].playerLevel}";
                    highScoreBanner.playerTotalTimeText.text = $"{(int)currSaveData.playerDatas[i].playerTotalTime % 3600}H {(int)currSaveData.playerDatas[i].playerTotalTime / 60}M {(int)currSaveData.playerDatas[i].playerTotalTime % 60}S";
                    if (i > 2){
                        highScoreBanner.playerRankText.text = (i + 1).ToString();
                    }
                }
            }
        } else {
            // Clear out previous high scores
            foreach (Transform child in contentPanel)
            {
                Destroy(child.gameObject);
            }
            noSavesYet.SetActive(true);
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

        PlayerData newPlayerData = new PlayerData();
        newPlayerData.playerName = GameManager.Instance.playerName;
        newPlayerData.playerLevel = GameManager.Instance.level;
        newPlayerData.playerTotalTime = GameManager.Instance.totalTime;
        if (currSaveData != null){
            currSaveData.playerDatas.Add(newPlayerData);
            newSave.playerDatas = currSaveData.playerDatas;
        } else {
            newSave.playerDatas = new List<PlayerData>();
            newSave.playerDatas.Add(newPlayerData);
        }
        newSave.playerDatas = newSave.playerDatas.OrderBy(playerData => playerData.playerTotalTime).ToList();

        newSave.fullScreen = settings.fullScreen;
        newSave.volumeBGM = audioManager.volumeBGM;
        newSave.volumeSFX = audioManager.volumeSFX;

        // write the current save data to the saveIndex save
        DataManager.writeFile(ref newSave, saveIndex);
    }
}