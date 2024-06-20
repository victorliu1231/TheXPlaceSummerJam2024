using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlowdown : MonoBehaviour
{
    public SpriteRenderer StageIndicator;
    public int stage = 1;

    void Start()
    {
        ChangeStage(stage, false);
    }

    public void ChangeStage(int newStage, bool changeLayer = true)
    {
        stage = newStage;
        switch (stage)
        {
            case 1:
                if (StageIndicator) StageIndicator.color = Color.red;
                if (changeLayer) gameObject.layer = LayerMask.NameToLayer("Stage1");
                break;
            case 2:
                if (StageIndicator) StageIndicator.color = Color.blue;
                if (changeLayer) gameObject.layer = LayerMask.NameToLayer("Stage2");
                break;
            case 3:
                if (StageIndicator) StageIndicator.color = Color.green;
                if (changeLayer) gameObject.layer = LayerMask.NameToLayer("Stage3");
                break;
            default:
                break;
        }
    }
}
