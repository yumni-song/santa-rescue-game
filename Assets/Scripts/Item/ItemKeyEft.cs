using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/Key")]
public class ItemKeyEft : ItemEffect
{
    [Header("사용 거리 설정")]
    [Tooltip("감옥과 이 거리 안에 있어야 열쇠 사용 가능")]
    public float useDistance = 10f; // 5f → 10f로 변경

    [Header("레이캐스트 설정")]
    [Tooltip("플레이어가 감옥을 바라보는 각도 범위")]
    public float maxAngle = 60f; // 45f → 60f로 변경 (더 넓은 시야)

    [Tooltip("레이캐스트 최대 거리")]
    public float raycastDistance = 15f; // 10f → 15f로 변경

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

        // 카메라 가져오기
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogWarning("메인 카메라를 찾을 수 없습니다!");
            return false;
        }

        // 1단계: 플레이어 주변에 감옥이 있는지 확인
        Prison nearestPrison = FindNearestPrison(player.transform.position);
        if (nearestPrison == null)
        {
            Debug.Log("근처에 감옥이 없습니다!");
            return false;
        }

        float distanceToPrison = Vector3.Distance(player.transform.position, nearestPrison.transform.position);

        // 2단계: 거리 체크
        if (distanceToPrison > useDistance)
        {
            Debug.Log($"감옥이 너무 멀리 있습니다! (거리: {distanceToPrison:F1}m, 필요: {useDistance}m 이내)");
            return false;
        }

        // 3단계: 플레이어가 감옥을 바라보고 있는지 확인
        Vector3 directionToPrison = (nearestPrison.transform.position - mainCamera.transform.position).normalized;
        float angle = Vector3.Angle(mainCamera.transform.forward, directionToPrison);

        if (angle > maxAngle)
        {
            Debug.Log($"감옥을 바라봐야 합니다! (현재 각도: {angle:F1}도, 필요: {maxAngle}도 이내)");
            return false;
        }

        // 4단계: 레이캐스트로 감옥이 시야에 있는지 확인 (장애물 체크)
        Ray ray = new Ray(mainCamera.transform.position, directionToPrison);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // 레이캐스트가 감옥에 맞았는지 확인
            Prison hitPrison = hit.collider.GetComponent<Prison>();
            if (hitPrison != nearestPrison)
            {
                Debug.Log("감옥과 플레이어 사이에 장애물이 있습니다!");
                return false;
            }
        }
        else
        {
            Debug.Log("감옥을 직접 바라봐야 합니다!");
            return false;
        }

        // 5단계: 모든 조건 만족 - 감옥 파괴
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