using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Time")]
    public Transform secondHand;
    public Transform minuteHand;
    public Transform hourHand;
    public const int hoursInDay = 12, minutesInHour = 60, secondsInMinute = 60;
    [Tooltip("The time slow multiplier for the game. 1 is normal time, 2 is twice as slow, 3 is three times as slow, etc.")]
    public float[] timeSlowdowns = new float[3]{1f, 2f, 3f};
    [Tooltip("The stage of the level you are on. 0 is the first stage, 1 is the second stage, etc.")]
    public int stage = 0;
    float totalTime = 0f;
    float currentTime = 0f;
    constant float hoursToDegrees = 360f / hoursInDay, minutesToDegrees = 360 / minutesInHour, secondsToDegrees = 360 / secondsInMinute;

    void GetHour(){
        return (currentTime / secondsInMinute / minutesInHour) % hoursInDay;
    }

    void GetMinute(){
        return (currentTime / secondsInMinute) % minutesInHour;
    }

    void GetSecond(){
        return currentTime % secondsInMinute;
    }

    void Update()
    {
        totalTime += Time.deltaTime;
        currentTime += Time.deltaTime / timeSlowdowns[stage];
        hourHand.localRotation = Quaternion.Euler(0, 0, -GetHour()*hoursToDegrees);
        minuteHand.localRotation = Quaternion.Euler(0, 0, -GetMinute()*minutesToDegrees);
        secondHand.localRotation = Quaternion.Euler(0, 0, -GetSecond()*secondsToDegrees);
    }


}
