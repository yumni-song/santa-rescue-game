using UnityEngine;

public class Prison : MonoBehaviour
{
    [Header("파괴 설정")]
    [Tooltip("파괴 시 추가 이펙트 (선택사항)")]
    public GameObject destroyEffectPrefab;

    [Tooltip("파괴 지연 시간 (이펙트 재생 시간 확보)")]
    public float destroyDelay = 0.5f;

    [Header("사운드 (선택사항)")]
    public AudioClip unlockSound;

    private bool isUnlocked = false;

    /// <summary>
    /// 열쇠로 감옥을 열 때 호출
    /// </summary>
    public void Unlock()
    {
        if (isUnlocked) return;
        isUnlocked = true;

        Debug.Log($"{gameObject.name} 감옥이 열렸습니다!");

        // 사운드 재생
        if (unlockSound != null)
        {
            AudioSource.PlayClipAtPoint(unlockSound, transform.position);
        }

        // 추가 이펙트 생성
        if (destroyEffectPrefab != null)
        {
            GameObject effect = Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 3f);
        }

        // 감옥 파괴
        Destroy(gameObject, destroyDelay);
    }

    /// 디버그용: 씬 뷰에서 사용 가능 범위 표시
    void OnDrawGizmosSelected()
    {
        // ItemKeyEft의 useDistance와 동일한 값 (10m로 변경)
        float debugRadius = 15f;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, debugRadius);

        // 시야각 표시 (선택사항 - 더 상세한 디버깅)
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 2f);
    }
}