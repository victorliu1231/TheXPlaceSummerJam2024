using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contains all the info needed in a save file
[Serializable]
public class SaveData
{
    public List<PlayerData> playerDatas; // list of player data
    public bool fullScreen; // bool for whether the game is fullscreen or not
    public float volumeBGM; // slider volume of the background music
    public float volumeSFX; // slider volume of the SFX
}

[Serializable]
public class PlayerData {
    public string playerName;
    public int playerLevel;
    public float playerTotalTime;
}
