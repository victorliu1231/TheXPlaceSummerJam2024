using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public Animator anim;
    public string cutscene = "Cutscene_VL";
    public string tutorialScene = "Tutorial_VL";
    public TextMeshProUGUI usernameText;

    void Start()
    {
        anim.Play("Player_Run_Down_Loop");
        AudioManager.GetSoundtrack("BossTheme").Play();
        if (PersistentData.Instance.justBootedUp) {
            Debug.Log("loading save");
            PersistentData.Instance.LoadSave(0);
            Invoke("TurnBootedUpBoolFalse", 0.01f);
        }
    }

    void TurnBootedUpBoolFalse(){
        PersistentData.Instance.justBootedUp = false;
    }

    public void PlayGame(){
        AudioManager.GetSoundtrack("BossTheme").Stop();
        SceneManager.LoadScene(cutscene);
    }

    public void PlayTutorial(){
        AudioManager.GetSoundtrack("BossTheme").Stop();
        SceneManager.LoadScene(tutorialScene);
    }

    public void ExitGame(){
        Application.Quit();
    }

    public void GrabFromInputField(string input){
        PersistentData.Instance.playerName = input;
        DisplayReactionToInput();
    }

    public void DisplayReactionToInput(){
        usernameText.text = PersistentData.Instance.playerName;
    }
}
