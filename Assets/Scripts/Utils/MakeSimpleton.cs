using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSimpleton : MonoBehaviour
{
    // Purely for edge cases with settings duplicating or not persisting between scenes
    private static MakeSimpleton _instance;
	public static MakeSimpleton Instance { get { return _instance; } }

    void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            DontDestroyOnLoad(this.gameObject);
            _instance = this;
        }
    }
}