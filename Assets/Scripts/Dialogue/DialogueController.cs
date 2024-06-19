using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueController : MonoBehaviour
{
    public List<GameObject> texts;
    public List<float> timeBetweenTexts;

    void Start(){
        foreach (GameObject text in texts){
            text.SetActive(false);
        }
        texts[0].SetActive(true);
        StartCoroutine(ActivateDialogue());
    }

    IEnumerator ActivateDialogue(){
        for (int i = 1; i < texts.Count; i++){
            yield return new WaitForSeconds(timeBetweenTexts[i-1]);
            texts[i].SetActive(true);
            if (i != texts.Count - 1) texts[i-1].SetActive(false);
        }
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.P)){
            LoadGame();
        }
    }

    public void LoadGame(){
        SceneManager.LoadScene("Game_VL");
    }
}