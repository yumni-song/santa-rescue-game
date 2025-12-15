using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("UI 설정")]
    public Transform heartContainer;
    public GameObject heartPrefab;
    public Sprite heartFull;
    public Sprite heartEmpty;

    private PlayerHealth playerHealth;
    private List<Image> heartImages = new List<Image>();
    private int lastKnownHP = -1;  // 마지막으로 알려진 HP (최적화용)

    void Start()
    {
        // PlayerHealth 자동 검색
        playerHealth = FindFirstObjectByType<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth를 찾을 수 없습니다!");
            return;
        }

        InitializeHearts();
        UpdateHearts();
    }

    void Update()
    {
        // HP가 실제로 변경되었을 때만 업데이트 (최적화)
        if (playerHealth != null && playerHealth.CurrentHP != lastKnownHP)
        {
            UpdateHearts();
            lastKnownHP = playerHealth.CurrentHP;
        }
    }

    /// <summary>
    /// 최대 HP만큼 하트 아이콘 생성
    /// </summary>
    private void InitializeHearts()
    {
        if (heartContainer == null || heartPrefab == null)
        {
            Debug.LogWarning("HP UI가 설정되지 않았습니다. Inspector에서 설정해주세요.");
            return;
        }

        // 기존 하트 제거
        foreach (Transform child in heartContainer)
        {
            Destroy(child.gameObject);
        }
        heartImages.Clear();

        // maxHP만큼 하트 생성
        for (int i = 0; i < playerHealth.maxHP; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            Image heartImage = heart.GetComponent<Image>();

            if (heartImage != null)
            {
                heartImages.Add(heartImage);
            }
            else
            {
                Debug.LogError("heartPrefab에 Image 컴포넌트가 없습니다!");
            }
        }
    }

    /// <summary>
    /// 현재 HP에 따라 하트 표시 업데이트
    /// </summary>
    private void UpdateHearts()
    {
        if (heartImages.Count == 0 || playerHealth == null) return;

        for (int i = 0; i < heartImages.Count; i++)
        {
            if (i < playerHealth.CurrentHP)
            {
                // 현재 HP 이하: 풀 하트
                heartImages[i].sprite = heartFull;
                heartImages[i].enabled = true;
            }
            else
            {
                // 현재 HP 초과: 빈 하트
                heartImages[i].sprite = heartEmpty;
                heartImages[i].enabled = true;
            }
        }
    }

    /// <summary>
    /// 피격 시 하트 깜빡임 효과 (외부에서 호출 가능)
    /// </summary>
    public void FlashHearts()
    {
        if (heartImages.Count == 0) return;
        StartCoroutine(FlashEffect());
    }

    private IEnumerator FlashEffect()
    {
        for (int flash = 0; flash < 3; flash++)
        {
            // 반투명하게
            foreach (var heart in heartImages)
            {
                heart.color = new Color(1, 1, 1, 0.3f);
            }
            yield return new WaitForSeconds(0.1f);

            // 원래대로
            foreach (var heart in heartImages)
            {
                heart.color = Color.white;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}