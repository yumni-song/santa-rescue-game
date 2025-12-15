using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance;

    [Header("Timer Settings")]
    public float totalTime = 180f;   // 전체 타이머 시간 (초)
    public bool autoStart = true;    // 씬 시작과 동시에 시작할지

    [Header("UI References")]
    public Canvas timerCanvas;             // 타이머가 붙어 있는 캔버스
    public TextMeshProUGUI timerText;      // 타이머 숫자 텍스트

    [Header("Auto Find UI")]
    [Tooltip("씬 로드 시 자동으로 UI 찾기")]
    public bool autoFindUI = true;

    [Tooltip("타이머 텍스트 오브젝트 이름")]
    public string timerTextName = "TimerText";

    [Tooltip("타이머 캔버스 오브젝트 이름")]
    public string timerCanvasName = "TimerCanvas";

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

        // 초기 UI 찾기
        if (autoFindUI)
        {
            FindUIReferences();
        }

        if (autoStart)
            isRunning = true;

        UpdateTimerUI();
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
        // UI가 없으면 다시 찾기 시도
        if (timerText == null && autoFindUI)
        {
            FindUIReferences();
        }

        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = $"{minutes:0}:{seconds:00}";
    }

    /// <summary>
    /// 씬에서 UI 참조 자동으로 찾기
    /// </summary>
    void FindUIReferences()
    {
        // Canvas 찾기
        if (timerCanvas == null)
        {
            GameObject canvasObj = GameObject.Find(timerCanvasName);
            if (canvasObj != null)
            {
                timerCanvas = canvasObj.GetComponent<Canvas>();

                // DontDestroyOnLoad 적용
                if (timerCanvas != null)
                {
                    DontDestroyOnLoad(timerCanvas.gameObject);
                }
            }
        }

        // TimerText 찾기
        if (timerText == null)
        {
            // 방법 1: 이름으로 찾기
            GameObject textObj = GameObject.Find(timerTextName);
            if (textObj != null)
            {
                timerText = textObj.GetComponent<TextMeshProUGUI>();
            }

            // 방법 2: Canvas 안에서 찾기
            if (timerText == null && timerCanvas != null)
            {
                timerText = timerCanvas.GetComponentInChildren<TextMeshProUGUI>();
            }

            if (timerText != null)
            {
                Debug.Log("타이머 UI 참조 찾기 성공!");
            }
            else
            {
                Debug.LogWarning($"타이머 텍스트를 찾을 수 없습니다! (찾는 이름: {timerTextName})");
            }
        }
    }

    // 씬이 로드될 때마다 호출
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"씬 로드됨: {scene.name}");

        if (scene.name == "StartScene")
        {
            if (timerCanvas != null) Destroy(timerCanvas.gameObject);
            Destroy(gameObject);
            return;
        }

        bool isGameScene = scene.name == "InGameScene1" || scene.name == "InGameScene2";

        // 게임 씬일 때 UI 참조 다시 찾기
        if (isGameScene && autoFindUI)
        {
            // 약간의 지연 후 UI 찾기 (씬 로드 완료 대기)
            Invoke(nameof(FindUIReferences), 0.1f);
        }

        if (timerCanvas != null)
        {
            timerCanvas.gameObject.SetActive(isGameScene);
        }

        // 게임씬이면 계속 흐르게 (이어지는 타이머 목적)
        isRunning = isGameScene;

        // UI 즉시 업데이트
        UpdateTimerUI();
    }

    // 필요 시 외부에서 쓸 수 있는 함수들
    public void StopTimer() => isRunning = false;
    public void StartTimer() => isRunning = true;
    public void ResetTimer()
    {
        currentTime = totalTime;
        UpdateTimerUI();
    }

    /// <summary>
    /// 수동으로 UI 참조 설정 (디버그용)
    /// </summary>
    public void ManuallySetUI(Canvas canvas, TextMeshProUGUI text)
    {
        timerCanvas = canvas;
        timerText = text;
        Debug.Log("타이머 UI 수동 설정 완료");
        UpdateTimerUI();
    }
}