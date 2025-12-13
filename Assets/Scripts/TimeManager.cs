using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance;

    [Header("Timer Settings")]
    public float totalTime = 180f;   // 전체 타이머 시간 (초)
    public bool autoStart = true;    // 씬 시작과 동시에 시작할지

    [Header("UI")]
    public Canvas timerCanvas;             // 타이머가 붙어 있는 캔버스
    public TextMeshProUGUI timerText;      // 타이머 숫자 텍스트

    private float currentTime;
    private bool isRunning = false;

    public float RemainingTime => currentTime;

    void Awake()
    {
        // 싱글톤 + 씬 전환 시 파괴되지 않게
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (timerCanvas != null)
            DontDestroyOnLoad(timerCanvas.gameObject);

        // 씬 로드 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
        }
    }

    void Start()
    {
        currentTime = totalTime;
        if (autoStart)
            isRunning = true;
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isRunning = false;

            // 시간 다 되면 게임오버 씬으로
            SceneManager.LoadScene("BadEnd");
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        timerText.text = $"{minutes:0}:{seconds:00}";
    }

    // 씬이 로드될 때마다 호출
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StartScene")
        {
            if (timerCanvas != null) Destroy(timerCanvas.gameObject);
            Destroy(gameObject);
            return;
        }

        bool isGameScene = scene.name == "InGameScene1" || scene.name == "InGameScene2";

        if (timerCanvas != null)
            timerCanvas.gameObject.SetActive(isGameScene);

        //  게임씬이면 계속 흐르게 (이어지는 타이머 목적)
        isRunning = isGameScene;
    }


    // 필요 시 외부에서 쓸 수 있는 함수들
    public void StopTimer() => isRunning = false;
    public void StartTimer() => isRunning = true;
    public void ResetTimer() => currentTime = totalTime;
}

