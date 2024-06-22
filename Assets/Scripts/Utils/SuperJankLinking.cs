using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJankLinking : MonoBehaviour
{
    public GameObject settings;
    
    void Start(){
        Settings[] settingsComponents = GameObject.FindObjectsOfType<Settings>(true);
        foreach (Settings s in settingsComponents){
            if (s.gameObject.tag == "Settings"){
                settings = s.gameObject;
            }
        }
    }

    void Update(){
        if (settings == null){
            Settings[] settingsComponents = GameObject.FindObjectsOfType<Settings>(true);
            foreach (Settings s in settingsComponents){
                if (s.gameObject.tag == "Settings"){
                    settings = s.gameObject;
                }
            }
        }
    }

    [ContextMenu("Check Settings")]
    public void CheckSettings(){
        Debug.Log(settings);
    }

    // This script only exists to be used in Button components in the Unity Editor.
    public void PlaySoundtrack(string name){
        AudioManager.GetSoundtrack(name)?.Play();
    }

    public void PlaySFX(string name){
        AudioManager.GetSFX(name)?.Play();
    }

    public void PlayButton(){
        if (PersistentData.Instance.playerName != ""){
            AudioManager.GetSFX("ButtonClick").Play();
        } else {
            AudioManager.GetSFX("Error").Play();
        }
    }

    public void StopSoundtrack(string name){
        AudioManager.GetSoundtrack(name)?.Stop();
    }

    public void StopSFX(string name){
        AudioManager.GetSFX(name)?.Stop();
    }

    public void SetSettingsActive(){
        if (settings != null) settings.SetActive(true);
        else Debug.Log("Settings not found.");
    }

    public void SetSettingsInactive(){
        if (settings != null) settings.SetActive(false);
        else Debug.Log("Settings not found.");
    }
}
