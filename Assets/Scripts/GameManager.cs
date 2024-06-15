using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Kino;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
	public static GameManager Instance { get { return _instance; } }
    [Header("Level")]
    public int level = 0;
    public float nextLevelTransitionDuration = 2f;
    public TextMeshProUGUI levelText;
    [Header("Stage")]
    public float nextStageTransitionDuration = 2f;
    public int stage = 0;
    [Tooltip("The time slow multiplier for the game. 1 is normal time, 2 is twice as slow, 3 is three times as slow, etc.")]
    public float[] timeSlowdowns = new float[3]{1f, 2f, 3f};
    [Tooltip("The time in seconds that each stage ends at.")]
    public float[] stageEndTimes = new float[3]{20f, 40f, 60f};
    [Header("Time")]
    public Transform secondHand;
    public Transform minuteHand;
    public Transform hourHand;
    public TextMeshProUGUI stageText;
    public const int hoursInDay = 12, minutesInHour = 60, secondsInMinute = 60;
    float totalTime = 0f;
    public float currentTime = 0f;
    const float hoursToDegrees = 360f / hoursInDay, minutesToDegrees = 360 / minutesInHour, secondsToDegrees = 360 / secondsInMinute;
    [Header("Misc")]
    public AnalogGlitch glitch;
    public bool inTransition = false;

    void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
            DontDestroyOnLoad(_instance);

            ResetLevel();
            StartCoroutine(TickSecondHand());
        }
    }

    public void ResetLevel(){
        stage = 0;
        currentTime = 0f;
        stageText.text = "Stage 1";
        levelText.text = "Level 1";
        glitch.scanLineJitter = 0f;
        glitch.horizontalShake = 0f;
        glitch.colorDrift = 0f;
        glitch.verticalJump = 0f;
        secondHand.localRotation = Quaternion.Euler(0, 0, 0);
        minuteHand.localRotation = Quaternion.Euler(0, 0, -level*minutesToDegrees);
    }

    float GetHour(){
        return (currentTime / secondsInMinute / minutesInHour) % hoursInDay;
    }

    float GetMinute(){
        return (currentTime / secondsInMinute) % minutesInHour;
    }

    float GetSecond(){
        return currentTime % secondsInMinute;
    }

    void Update()
    {
        totalTime += Time.deltaTime;
        if (!inTransition){
            currentTime += Time.deltaTime / timeSlowdowns[stage];
            hourHand.localRotation = Quaternion.Euler(0, 0, -GetHour()*hoursToDegrees);
            minuteHand.localRotation = Quaternion.Euler(0, 0, -GetMinute()*minutesToDegrees);
            //secondHand.localRotation = Quaternion.Euler(0, 0, -GetSecond()*secondsToDegrees);
            if (currentTime >= stageEndTimes[stage]){
                StartCoroutine(NextStage());
            }
        }
        if (currentTime >= 60f){
            ResetLevel();
            StartCoroutine(NextLevel());
        }
    }

    IEnumerator TickSecondHand(){
        while (true){
            float originalSecond = GetSecond();
            for (int i = 0; i < 4; i++){
                secondHand.localRotation = Quaternion.Euler(0, 0, -(originalSecond-0.05f)*secondsToDegrees);
                yield return new WaitForSeconds(0.1f);
                secondHand.localRotation = Quaternion.Euler(0, 0, -(originalSecond)*secondsToDegrees);
                yield return new WaitForSeconds(0.1f);
                secondHand.localRotation = Quaternion.Euler(0, 0, -(originalSecond+0.05f)*secondsToDegrees);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator NextStage(){
        inTransition = true;
        stage++;
        stageText.text = "Stage " + (stage+1);
        glitch.horizontalShake = 0.1f;
        glitch.scanLineJitter += 0.15f;
        glitch.colorDrift = 1f;
        glitch.verticalJump = 0.4f;
        yield return new WaitForSeconds(nextStageTransitionDuration);
        glitch.horizontalShake = 0f;
        glitch.colorDrift = 0f;
        glitch.verticalJump = 0f;
        inTransition = false;
    }

    [ContextMenu("Test Next Stage")]
    public void TestNextStage(){
        StartCoroutine(NextStage());
    }

    IEnumerator NextLevel(){
        inTransition = true;
        level++;
        levelText.text = "Level " + (level + 1);
        glitch.scanLineJitter = 1f;
        glitch.verticalJump = 0.4f;
        yield return new WaitForSeconds(nextLevelTransitionDuration);
        glitch.scanLineJitter = 0f;
        glitch.verticalJump = 0f;
        inTransition = false;
    }

    [ContextMenu("Test Next Level")]
    public void TestNextLevel(){
        StartCoroutine(NextLevel());
    }
}
