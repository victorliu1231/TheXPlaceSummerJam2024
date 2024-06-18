using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Kino;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
	public static GameManager Instance { get { return _instance; } }
    [Header("Level")]
    public int level = 0;
    public int levelsBetweenBosses = 5;
    public GameObject bossGUI;
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
    public float totalTime = 0f;
    public float currentTime = 0f;
    const float hoursToDegrees = 360f / hoursInDay, minutesToDegrees = 360 / minutesInHour, secondsToDegrees = 360 / secondsInMinute;
    [Header("Death Screen")]
    public GameObject deathScreen;
    public GameObject playAgainButton;
    public GameObject youDiedText;
    public GameObject youRanOutOfTimeText;
    public bool isGameOver = false;
    [Header("Misc")]
    public bool inTransition = false;
    public AnalogGlitch glitch;
    public GameObject player;
    public Transform enemiesParent;
    public string playerName;
    public Vector3 spawnPosition;
    public Vector3 weaponSpawnPositionOne;
    public Vector3 weaponSpawnPositionTwo;
    public List<GameObject> weaponCollectibles;
    [Header("Production")]
    public bool isDebugging = true;
    public Transform collectiblesParent;

    void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
            //DontDestroyOnLoad(_instance);

            PlayGame();
        }
    }

    public void PlayGame(){
        ResetLevel();
        minuteHand.localRotation = Quaternion.Euler(0, 0, 0);
        minuteHand.localRotation = Quaternion.Euler(0, 0, 0);
        totalTime = 0f;
        isGameOver = false;
        inTransition = false;
        StartCoroutine(TickSecondHand());
        deathScreen.SetActive(false);
        playAgainButton.SetActive(false);
        youDiedText.SetActive(false);
        youRanOutOfTimeText.SetActive(false);
        bossGUI.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        GameObject data = GameObject.FindGameObjectWithTag("Data");
        AudioManager.GetSoundtrack("MainTheme").Play();
        if (data != null) playerName = data.GetComponent<PersistentData>().playerName;
        if (!isDebugging){
            player.transform.position = spawnPosition;
            foreach (Transform child in collectiblesParent){
                Destroy(child.gameObject);
            }
            Instantiate(weaponCollectibles[0], weaponSpawnPositionOne, Quaternion.identity, collectiblesParent);
        }
    }

    public void ResetLevel(){
        level = 0;
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
        if (currentTime >= 60f){
            if (enemiesParent.childCount == 0){
                ResetLevel();
                StartCoroutine(NextLevel());
            } else {
                StartCoroutine(GameOver(true));
            }
        }
        else if (!inTransition){
            currentTime += Time.deltaTime / timeSlowdowns[stage];
            hourHand.localRotation = Quaternion.Euler(0, 0, -GetHour()*hoursToDegrees);
            minuteHand.localRotation = Quaternion.Euler(0, 0, -GetMinute()*minutesToDegrees);
            //secondHand.localRotation = Quaternion.Euler(0, 0, -GetSecond()*secondsToDegrees);
            if (currentTime >= stageEndTimes[stage]){
                StartCoroutine(NextStage());
            }
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
        glitch.scanLineJitter += 0.1f;
        glitch.colorDrift = 1f;
        glitch.verticalJump = 0.4f;
        yield return new WaitForSeconds(nextStageTransitionDuration);
        glitch.horizontalShake = 0f;
        glitch.colorDrift = 0f;
        glitch.verticalJump = 0f;
        inTransition = false;
        player.transform.position = spawnPosition;
        AudioManager.GetSoundtrack("MainTheme").Stop();
        AudioManager.GetSoundtrack("MainTheme").Play();
        if (!isDebugging){
            foreach (Transform child in collectiblesParent){
                Destroy(child.gameObject);
            }
            if (stage == 1) Instantiate(weaponCollectibles[1], weaponSpawnPositionOne, Quaternion.identity, collectiblesParent);
            if (stage == 2){
                Instantiate(weaponCollectibles[2], weaponSpawnPositionOne, Quaternion.identity, collectiblesParent);
                Instantiate(weaponCollectibles[3], weaponSpawnPositionTwo, Quaternion.identity, collectiblesParent);
            }
        }
    }

    [ContextMenu("Test Next Stage")]
    public void TestNextStage(){
        StartCoroutine(NextStage());
    }

    IEnumerator NextLevel(){
        PersistentData.WriteToSave(0);
        inTransition = true;
        level++;
        levelText.text = "Level " + (level + 1);
        glitch.scanLineJitter = 1f;
        glitch.verticalJump = 0.4f;
        yield return new WaitForSeconds(nextLevelTransitionDuration);
        glitch.scanLineJitter = 0f;
        glitch.verticalJump = 0f;
        player.transform.position = spawnPosition;
        if (level % levelsBetweenBosses == 0){
            bossGUI.SetActive(true);
            AudioManager.GetSoundtrack("BossTheme").Play();
            yield return new WaitForSeconds(1.5f);
            bossGUI.SetActive(false);
        } else {
            AudioManager.GetSoundtrack("MainTheme").Stop();
            AudioManager.GetSoundtrack("MainTheme").Play();
        }
        inTransition = false;
    }

    [ContextMenu("Test Next Level")]
    public void TestNextLevel(){
        StartCoroutine(NextLevel());
    }

    public IEnumerator GameOver(bool ranOutOfTime){
        player.GetComponent<Player>().anim.Play("Player_Death");
        isGameOver = true;
        deathScreen.SetActive(true);
        if (ranOutOfTime){
            youRanOutOfTimeText.SetActive(true);
        } else {
            youDiedText.SetActive(true);
        }
        yield return new WaitForSeconds(1f);
        playAgainButton.SetActive(true);
        for (int i = 0; i < 10; i++){
            playAgainButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.1f*i);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void PlayAgain(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
