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

    public List<GameObject> allLevels = new List<GameObject>();

    [Header("Level")]
    public int level = 0;
    public int levelsBetweenBosses = 5;
    public GameObject bossGUI;
    public float nextLevelTransitionDuration = 2f;
    public TextMeshProUGUI levelText;
    [Header("Buffs")]
    public int everyXLevelsPlayerGetsStronger = 2;
    public float playerStrengthMultiplier = 1f;
    public float playerStrengthBoostMultiplier;
    public int everyXLevelsEnemyGetsStronger = 5;
    public float enemyStrengthMultiplier = 1f;
    public float enemyStrengthBoostMultiplier;
    [Header("Enemy Waves")]
    [Tooltip("The list of enemy waves. Index 0 is level 1, index 1 is level 2, etc.")]
    [SerializeField]public List<CircularEnemyGenerator> enemyWaves;
    public Transform generatorsParent;
    public List<GameObject> bosses;
    [Header("Stage")]
    public float nextStageTransitionDuration = 2f;
    public int stage = 1;
    [Tooltip("The time slow multiplier for the game. 1 is normal time, 2 is twice as slow, 3 is three times as slow, etc.")]
    public float[] timeSlowdowns = new float[3]{1f, 2f, 3f};
    [Tooltip("The time in seconds that each stage ends at.")]
    public float[] stageEndTimes = new float[3]{20f, 40f, 60f};
    public LayerMask stageLayer;
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
    public GameObject blackScreen;
    [Header("Collectibles")]
    public Vector3 weaponSpawnPositionOne;
    public Vector3 weaponSpawnPositionTwo;
    public List<GameObject> weaponCollectibles;
    public Transform collectiblesParent;
    [Header("Misc Bindings")]
    public AnalogGlitch glitch;
    [Tooltip("Auto-bound")]
    public GameObject player;
    public Transform enemiesParent;
    public Transform ghostsParent;
    public Transform spawnedWallsParent;
    public Transform projectilesParent;
    public PlayerRecorder playerRecorder;
    [Header("Misc")]
    public bool inTransition = false;
    public string playerName;
    public Vector3 spawnPosition;
    public float clockRadius = 12.5f;
    public GameObject pauseMenu;
    [Header("Tutorial")]
    public bool inTutorial = false;
    public GameObject doneWithTutorialGameObject;
    public bool isBeginTime = false;
    
    [Header("Debugging")]
    public bool isDebugging = true;
    public int jumpToLevel;
    public int jumpToStage;
    public int jumpToTime;

    void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;

            PlayGame();
        }
    }

    [ContextMenu("Play Game")]
    public void PlayGame(){
        ResetLevel();
        Time.timeScale = 1f;
        playerStrengthMultiplier = 1f;
        enemyStrengthMultiplier = 1f;
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
        if (data != null) playerName = data.GetComponent<PersistentData>().playerName;

        if (!isDebugging){
            if (!inTutorial){
                currentLevelTemplate = allLevels[Random.Range(0, allLevels.Count - 1)];
                currentLevelInstance = Instantiate(currentLevelTemplate, Vector2.zero, Quaternion.identity);
            }
            AudioManager.GetSoundtrack("MainTheme").Play();
            player.transform.position = spawnPosition;
            foreach (Transform child in collectiblesParent){
                Destroy(child.gameObject);
            }
    
            foreach (Transform child in enemiesParent){
                Destroy(child.gameObject);
            }
        } else {
            level = jumpToLevel;
            stage = jumpToStage;
            currentTime = jumpToTime;
            stageText.text = "Stage " + (stage);
            levelText.text = "Level " + (level + 1);
            if ((level + 1) % levelsBetweenBosses == 0){
                AudioManager.GetSoundtrack("BossTheme").Play();
                float angle = Random.Range(0f, 360f);
                float x = transform.position.x + clockRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
                float y = transform.position.y + clockRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
                Instantiate(bosses[(level+1)/levelsBetweenBosses - 1], new Vector2(x,y), Quaternion.identity, enemiesParent);
            } else {
                AudioManager.GetSoundtrack("MainTheme").Stop();
                AudioManager.GetSoundtrack("MainTheme").Play();
            }
            playerStrengthMultiplier += (level % everyXLevelsPlayerGetsStronger)*playerStrengthBoostMultiplier;
            enemyStrengthMultiplier += (level % everyXLevelsEnemyGetsStronger)*enemyStrengthBoostMultiplier;
        }
    }

    public void ResetLevel(){
        stage = 1;
        stageLayer = LayerMask.NameToLayer("Stage1");
        currentTime = 0f;
        if (!inTutorial){
            stageText.text = "Stage 1";
            levelText.text = $"Level {level+1}";
        }
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
        if (Input.GetKeyDown(KeyCode.Escape)){
            PauseGame();
        }
        totalTime += Time.deltaTime;
        if (!inTutorial){
            if (currentTime >= 60f){
                // see if all children are inactive or destroyed
                bool allInactive = true;
                foreach (Transform child in enemiesParent){
                    if (child.gameObject.activeSelf){
                        allInactive = false;
                    }
                }
                if (enemiesParent.childCount == 0 || allInactive){
                    ResetLevel();
                } else {
                    if (!isDebugging) {
                        StopAllCoroutines();
                        StartCoroutine(GameOver(true));
                    } else {
                        ResetLevel();
                        StartCoroutine(NextLevel());
                    }
                }
            }
            else if (!inTransition){
                hourHand.localRotation = Quaternion.Euler(0, 0, -GetHour()*hoursToDegrees);
                minuteHand.localRotation = Quaternion.Euler(0, 0, -GetMinute()*minutesToDegrees);
                //secondHand.localRotation = Quaternion.Euler(0, 0, -GetSecond()*secondsToDegrees);
                if (currentTime >= stageEndTimes[stage-1] && stage != 3){
                    Debug.Log("moving onto next stage");
                    StartCoroutine(NextStage());
                }
                currentTime += Time.deltaTime * (1 + 0.1f * level);
            }
        } else {
            // if in tutorial
            if (!inTransition){
                hourHand.localRotation = Quaternion.Euler(0, 0, -GetHour()*hoursToDegrees);
                minuteHand.localRotation = Quaternion.Euler(0, 0, -GetMinute()*minutesToDegrees);
                //secondHand.localRotation = Quaternion.Euler(0, 0, -GetSecond()*secondsToDegrees);
                if (currentTime >= stageEndTimes[stage-1] && stage < 3){
                    StartCoroutine(NextStage());
                }
                if (isBeginTime) currentTime += Time.deltaTime;
                if (enemiesParent.childCount == 0 && currentTime >= 30 && !inTransition){
                    doneWithTutorialGameObject.SetActive(true);
                }
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

    public GameObject currentLevelTemplate;
    public GameObject currentLevelInstance;

    public GameObject playerGhost;

    public Queue<Snapshot> ghost1 = new Queue<Snapshot>();
    public Queue<Snapshot> ghost2 = new Queue<Snapshot>();

    public void InstantiateGhosts()
    {
        if (stage >= 2)
        {
            GameObject ghost = Instantiate(playerGhost, new Vector2(0, 0), Quaternion.identity, ghostsParent);
            ghost.layer = LayerMask.NameToLayer("Stage1");
            Queue<Snapshot> g1 = new Queue<Snapshot>();
            foreach (Snapshot snap in ghost1)
            {
                g1.Enqueue(snap.CopySelf());
            }
            ghost.GetComponent<PlayerGhost>().snapshots = g1;
        }
        if (stage >= 3)
        {
            GameObject ghost = Instantiate(playerGhost, new Vector2(0, 0), Quaternion.identity, ghostsParent);
            ghost.layer = LayerMask.NameToLayer("Stage2");
            Queue<Snapshot> g2 = new Queue<Snapshot>();
            foreach (Snapshot snap in ghost2)
            {
                g2.Enqueue(snap.CopySelf());
            }
            ghost.GetComponent<PlayerGhost>().snapshots = g2;
            ghost.GetComponent<TimeSlowdown>().ChangeStage(2);
        }
    }

    IEnumerator NextStage(){
        foreach (Transform child in enemiesParent){
            Destroy(child.gameObject);
        }
        foreach (Transform child in ghostsParent){
            Destroy(child.gameObject);
        }
        foreach (Transform child in spawnedWallsParent){
            Destroy(child.gameObject);
        }
        foreach (Transform child in projectilesParent){
            Destroy(child.gameObject);
        }
        foreach (Transform child in enemiesParent)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in collectiblesParent){
            Destroy(child.gameObject);
        }

        inTransition = true;
        stage++;
        stageLayer = LayerMask.NameToLayer("Stage" + stage);
        player.GetComponent<TimeSlowdown>().ChangeStage(stage);
        if (!inTutorial) stageText.text = "Stage " + (stage);
        glitch.horizontalShake = 0.1f;
        glitch.scanLineJitter = 0.25f;
        glitch.colorDrift = 1f;
        glitch.verticalJump = 0.4f;
        AudioManager.GetSFX("TimeWarp")?.Play();
        yield return new WaitForSeconds(nextStageTransitionDuration);
        glitch.horizontalShake = 0f;
        glitch.colorDrift = 0f;
        glitch.verticalJump = 0f;
        inTransition = false;
        player.transform.position = spawnPosition;
        if (!isDebugging){
            if (stage == 2)
            {
                foreach (Snapshot snap in playerRecorder.snapshots)
                {
                    ghost1.Enqueue(snap.CopySelf());
                }
            }
            else if (stage == 3)
            {
                foreach (Snapshot snap in playerRecorder.snapshots)
                {
                    ghost2.Enqueue(snap.CopySelf());
                }
            }
            playerRecorder.FlushRecordings();
            if (player.GetComponent<Player>().weaponInHand != null) Destroy(player.GetComponent<Player>().weaponInHand.gameObject);
            InstantiateGhosts();
            Destroy(currentLevelInstance);
            currentLevelInstance = Instantiate(currentLevelTemplate, Vector2.zero, Quaternion.identity);
        }
    }

    [ContextMenu("Test Next Stage")]
    public void TestNextStage(){
        StartCoroutine(NextStage());
    }

    IEnumerator NextLevel(){
        foreach (Transform child in enemiesParent){
            Destroy(child.gameObject);
        }
        foreach (Transform child in ghostsParent){
            Destroy(child.gameObject);
        }
        foreach (Transform child in spawnedWallsParent){
            Destroy(child.gameObject);
        }
        foreach (Transform child in collectiblesParent){
            Destroy(child.gameObject);
        }
        foreach (Transform child in projectilesParent){
            Destroy(child.gameObject);
        }
        inTransition = true;
        level++;
        levelText.text = "Level " + (level + 1);
        glitch.scanLineJitter = 1f;
        glitch.verticalJump = 0.4f;
        PersistentData.Instance.CreateNewSave(0);
        AudioManager.GetSFX("TimeWarp")?.Play();
        yield return new WaitForSeconds(nextLevelTransitionDuration);
        ResetLevel();
        player.GetComponent<TimeSlowdown>().ChangeStage(1);
        player.transform.position = spawnPosition;
        if ((level + 1) % levelsBetweenBosses == 0){
            bossGUI.SetActive(true);
            AudioManager.GetSoundtrack("BossTheme").Play();
            yield return new WaitForSeconds(1.5f);
            bossGUI.SetActive(false);
            float angle = Random.Range(0f, 360f);
            float x = transform.position.x + clockRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = transform.position.y + clockRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
            Instantiate(bosses[(level+1)/levelsBetweenBosses], new Vector2(x,y), Quaternion.identity, enemiesParent);
        } else {
            AudioManager.GetSoundtrack("MainTheme").Stop();
            AudioManager.GetSoundtrack("MainTheme").Play();
            Debug.Log("next level is called");
        }
        inTransition = false;
        if (level % everyXLevelsPlayerGetsStronger == 0){
            playerStrengthMultiplier += playerStrengthBoostMultiplier;
        }
        if (level % everyXLevelsEnemyGetsStronger == 0){
            enemyStrengthMultiplier += enemyStrengthBoostMultiplier;
        }
    }

    [ContextMenu("Test Next Level")]
    public void TestNextLevel(){
        StartCoroutine(NextLevel());
    }

    public IEnumerator GameOver(bool ranOutOfTime){
        AudioManager.StopAllSoundtracks();
        AudioManager.StopAllSFXs();
        AudioManager.GetSFX("GameOver").Play();
        foreach (Transform child in enemiesParent){
            Destroy(child.gameObject);
        }
        foreach (Transform child in ghostsParent){
            Destroy(child.gameObject);
        }
        foreach (Transform child in spawnedWallsParent){
            Destroy(child.gameObject);
        }
        foreach (Transform child in collectiblesParent){
            Destroy(child.gameObject);
        }
        foreach (Transform child in projectilesParent){
            Destroy(child.gameObject);
        }
        deathScreen.SetActive(true);
        blackScreen.SetActive(true);
        if (ranOutOfTime){
            youRanOutOfTimeText.SetActive(true);
        } else {
            youDiedText.SetActive(true);
        }
        player.GetComponent<Player>().anim.Play("Player_Death");
        isGameOver = true;
        PersistentData.Instance.CreateNewSave(0);
        yield return new WaitForSeconds(nextLevelTransitionDuration);
        blackScreen.SetActive(false);
        player.SetActive(false);
        AudioManager.GetSoundtrack("BossTheme").Play();
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

    public void ReturnToMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main_Menu_VL");
    }

    public void PauseGame(){
        pauseMenu.SetActive(true);
        AudioManager.PauseAllSoundtracks();
        AudioManager.PauseAllSFXs();
        Time.timeScale = 0f;
        inTransition = true;
    }

    public void ResumeGame(){
        pauseMenu.SetActive(false);
        AudioManager.UnpauseAllSoundtracks();
        AudioManager.UnpauseAllSFXs();
        Time.timeScale = 1f;
        inTransition = false;
    }
}
