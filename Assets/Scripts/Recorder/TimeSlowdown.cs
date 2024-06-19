using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlowdown : MonoBehaviour
{
    public SpriteRenderer StageIndicator;

    public int stage = 1;

    void Start()
    {
        ChangeStage(stage);
    }

    public void ChangeStage(int newStage)
    {
        stage = newStage;
        if (StageIndicator)
        {
            switch (stage)
            {
                case 1:
                    StageIndicator.color = Color.red;
                    break;
                case 2:
                    StageIndicator.color = Color.blue;
                    break;
                case 3:
                    StageIndicator.color = Color.green;
                    break;
                default:
                    break;
            }
        }
    }
}
