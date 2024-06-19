using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSelf : MonoBehaviour
{
    // Call this method from other scripts or animation events
    public void InvokeDisable(float lifetime){
        Invoke("Disable", lifetime);
    }

    public void Disable(){
        gameObject.SetActive(false);
    }
}
