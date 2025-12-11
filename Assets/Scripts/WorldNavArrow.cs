using UnityEngine;

public class WorldNavArrow : MonoBehaviour
{
    [Header("참조 대상")]
    public Transform player;   // 플레이어 or 카메라
    public Transform target;   // 목적지

    [Header("플레이어 기준 위치 오프셋")]
    public Vector3 offsetFromPlayer = new Vector3(0f, 1.5f, 2.5f);
    // x: 좌우, y: 위아래, z: 앞뒤 (플레이어 로컬 기준)

    [Header("표시/숨김 거리 설정")]
    public float hideDistance = 2f;   // 이 거리 이내로 오면 화살표 숨김

    void Update()
    {
        if (player == null || target == null)
            return;

        // 1. 플레이어 → 목적지 방향 벡터 (수평면 기준)
        Vector3 dir = target.position - player.position;
        dir.y = 0f;   // 위아래는 무시

        float sqrDist = dir.sqrMagnitude;

        // 2. 너무 가까우면 화살표 숨기기
        if (sqrDist < hideDistance * hideDistance)
        {
            if (gameObject.activeSelf)
                gameObject.SetActive(false);
            return;
        }
        else
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
        }

        if (sqrDist < 0.0001f)
            return;

        dir.Normalize();

        // 3. 화살표 위치: 플레이어 앞에 붙여두기
        //    offsetFromPlayer는 플레이어 "로컬 좌표" 기준
        transform.position = player.position + player.TransformDirection(offsetFromPlayer);

        // 4. 화살표 회전: 목적지 방향 바라보게
        //    화살표 모델의 앞 방향이 +Z 라고 가정함
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }
}
