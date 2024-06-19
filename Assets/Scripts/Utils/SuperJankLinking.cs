using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJankLinking : MonoBehaviour
{
    // This script only exists to be used in Button components in the Unity Editor.
    public void PlaySoundtrack(string name){
        AudioManager.GetSoundtrack(name)?.Play();
    }

    public void PlaySFX(string name){
        AudioManager.GetSFX(name)?.Play();
    }
}
