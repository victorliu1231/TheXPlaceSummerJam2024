using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static float GetStage(TimeSlowdown script)
    {
        float stage = GameManager.Instance.stage / (script?.stage ?? 1);
        return stage;
    }
    public static float GetRecriprocalStage(TimeSlowdown script)
    {
        float stage = GameManager.Instance.stage / (script?.stage ?? 1);
        return 1f/stage;
    }
}
