using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;

    // HP 설정
    public int maxHP = 10;
    private int currentHP;

    // 피격 무적 시간 설정
    public float invincibleDuration = 0.8f;
    private float lastDamageTime = -999f;

    // 게임 오버 설정
    public float deathDelay = 2f;

    // 피격 사운드 설정
    public AudioClip hitSound;
    public AudioClip deathSound;  // 사망 효과음 추가!
    [Range(0f, 1f)]
    public float hitSoundVolume = 1f;

    // 페이드 아웃 설정
    public bool useFadeOut = true;
    public Color fadeColor = Color.black;

    // 내부 변수
    private bool isDead = false;
    private PlayerMovement playerMovement;

    // 프로퍼티
    public bool IsDead => isDead;
    public int CurrentHP => currentHP;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("PlayerHealth Singleton 초기화 및 DDOL 설정 완료.");
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
                Debug.LogWarning("중복 PlayerHealth 인스턴스 제거됨.");
            }
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    void Start()
    {
        if (currentHP == 0)
        {
            currentHP = maxHP;
        }

        playerMovement = GetComponent<PlayerMovement>();

        Debug.Log($"PlayerHealth 초기화 완료 - HP: {currentHP}/{maxHP}");
        UpdateHealthUI();
        MoveToStartPoint();
    }

    // 씬이 로드될 때마다 호출
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"씬 로드됨: {scene.name}");

        if (scene.name == "BadEnd")
        {
            return;
        }

        MoveToStartPoint();
        UpdateHealthUI();
    }

    // StartPoint로 플레이어 이동
    void MoveToStartPoint()
    {
        GameObject startPoint = GameObject.FindGameObjectWithTag("StartPoint");

        if (startPoint != null)
        {
            transform.position = startPoint.transform.position;
            transform.rotation = startPoint.transform.rotation;
            Debug.Log($"플레이어 위치 설정: {startPoint.transform.position}");
        }
        else
        {
            Debug.LogWarning("StartPoint 태그를 가진 오브젝트를 찾을 수 없습니다!");
        }
    }

    // 데미지 받기
    public void TakeDamage(int damage)
    {
        if (isDead) return;
        if (Time.time < lastDamageTime + invincibleDuration) return;

        lastDamageTime = Time.time;
        currentHP -= damage;

        Debug.Log($"플레이어 피격! HP: {currentHP}/{maxHP} (데미지: {damage})");

        PlayHitSound();

        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }

        UpdateHealthUI();
    }

    // 회복
    public void Heal(int amount)
    {
        if (isDead) return;

        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;

        Debug.Log($"플레이어 회복! HP: {currentHP}/{maxHP} (회복량: {amount})");
        UpdateHealthUI();
    }

    // UI 업데이트
    void UpdateHealthUI()
    {
        PlayerHealthUI ui = FindFirstObjectByType<PlayerHealthUI>();
        if (ui != null)
        {
            ui.FlashHearts();
        }
    }

    // 피격 사운드 재생
    void PlayHitSound()
    {
        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position, hitSoundVolume);
        }
    }

    // 사망 처리
    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("플레이어 사망!");

        // 사망 효과음 재생
        PlayDeathSound();

        DisablePlayerControls();
        StartCoroutine(DeathSequence());
    }

    // 사망 효과음 재생
    void PlayDeathSound()
    {
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position, hitSoundVolume);
            Debug.Log("사망 효과음 재생!");
        }
    }

    // 플레이어 조작 비활성화
    void DisablePlayerControls()
    {
        if (playerMovement != null)
        {
            playerMovement.FreezePlayer();
            Debug.Log("PlayerMovement.FreezePlayer() 호출");
        }
    }

    // 사망 연출 코루틴
    IEnumerator DeathSequence()
    {
        if (useFadeOut)
        {
            yield return StartCoroutine(FadeOutEffect());
        }
        else
        {
            yield return new WaitForSeconds(deathDelay);
        }

        LoadBadEndScene();
    }

    // 페이드 아웃 효과
    IEnumerator FadeOutEffect()
    {
        GameObject fadePanel = GameObject.Find("FadePanel");
        CanvasGroup canvasGroup = null;

        if (fadePanel == null)
        {
            fadePanel = CreateFadePanel();
            canvasGroup = fadePanel.GetComponent<CanvasGroup>();
        }
        else
        {
            canvasGroup = fadePanel.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = fadePanel.AddComponent<CanvasGroup>();
            }
        }

        fadePanel.SetActive(true);
        canvasGroup.alpha = 0f;

        float elapsed = 0f;
        while (elapsed < deathDelay)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / deathDelay);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    // 페이드 패널 동적 생성
    GameObject CreateFadePanel()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        }

        GameObject fadePanel = new GameObject("FadePanel");
        fadePanel.transform.SetParent(canvas.transform, false);

        RectTransform rectTransform = fadePanel.AddComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.anchoredPosition = Vector2.zero;

        UnityEngine.UI.Image image = fadePanel.AddComponent<UnityEngine.UI.Image>();
        image.color = fadeColor;

        CanvasGroup canvasGroup = fadePanel.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = true;

        fadePanel.transform.SetAsLastSibling();

        return fadePanel;
    }

    // BadEnd 씬 로드
    void LoadBadEndScene()
    {
        Debug.Log("씬 전환: BadEnd");
        SceneManager.LoadScene("BadEnd");
    }
}