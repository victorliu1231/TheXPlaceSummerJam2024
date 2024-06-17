using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public Animator anim;

    void Start()
    {
        anim.Play("Player_Run_Down_Loop");
    }
}
