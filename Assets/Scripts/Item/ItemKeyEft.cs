using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/Key")]
public class ItemKeyEft : ItemEffect
{
    [Header("사용 거리 설정")]
    [Tooltip("감옥과 이 거리 안에 있어야 열쇠 사용 가능")]
    public float useDistance = 10f;

    [Header("이펙트")]
    [Tooltip("감옥이 파괴될 때 생성할 이펙트")]
    public GameObject unlockEffectPrefab;

    [Tooltip("이펙트 지속 시간")]
    public float effectDuration = 2f;

    public override bool ExecuteRole()
    {
        Debug.Log("열쇠 사용 시도");

        // 플레이어 찾기
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("플레이어를 찾을 수 없습니다!");
            return false;
        }

        // 플레이어 주변에 감옥이 있는지 확인
        Prison nearestPrison = FindNearestPrison(player.transform.position);
        if (nearestPrison == null)
        {
            Debug.Log("근처에 감옥이 없습니다!");
            return false;
        }

        float distanceToPrison = Vector3.Distance(player.transform.position, nearestPrison.transform.position);

        // 거리 체크만 수행
        if (distanceToPrison > useDistance)
        {
            Debug.Log($"감옥이 너무 멀리 있습니다! (거리: {distanceToPrison:F1}m, 필요: {useDistance}m 이내)");
            return false;
        }

        // 거리 조건만 만족하면 감옥 파괴
        UnlockPrison(nearestPrison);

        Debug.Log("열쇠로 감옥을 열었습니다!");
        return true;
    }

    /// <summary>
    /// 플레이어 주변에서 가장 가까운 감옥 찾기
    /// </summary>
    Prison FindNearestPrison(Vector3 playerPosition)
    {
        GameObject[] prisonObjects = GameObject.FindGameObjectsWithTag("Prison");

        if (prisonObjects.Length == 0)
        {
            return null;
        }

        Prison nearest = null;
        float minDistance = float.MaxValue;

        foreach (GameObject obj in prisonObjects)
        {
            Prison prison = obj.GetComponent<Prison>();
            if (prison == null) continue;

            float distance = Vector3.Distance(playerPosition, obj.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = prison;
            }
        }

        return nearest;
    }

    /// <summary>
    /// 감옥 파괴 및 이펙트 생성
    /// </summary>
    void UnlockPrison(Prison prison)
    {
        // 이펙트 생성
        if (unlockEffectPrefab != null)
        {
            GameObject effect = Instantiate(unlockEffectPrefab, prison.transform.position, Quaternion.identity);
            GameObject.Destroy(effect, effectDuration);
        }

        // 감옥 파괴
        prison.Unlock();
    }
}