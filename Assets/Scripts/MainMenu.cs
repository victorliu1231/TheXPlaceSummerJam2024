using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator anim;
    public string gameScene = "Scene_VL";
    public string tutorialScene = "Tutorial";

    void Start()
    {
        anim.Play("Player_Run_Down_Loop");
    }

    public void PlayGame(){
        SceneManager.LoadScene(gameScene);
    }

    public void PlayTutorial(){
        SceneManager.LoadScene(tutorialScene);
    }

    public void ExitGame(){
        Application.Quit();
    }
}
