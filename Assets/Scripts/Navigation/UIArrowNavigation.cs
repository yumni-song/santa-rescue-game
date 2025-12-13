using UnityEngine;
using UnityEngine.UI;

public class UIArrowNavigation : MonoBehaviour
{
    public Transform player; // 플레이어 또는 카메라
    public Transform target; // 목적지

    private RectTransform arrowRect;

    void Awake()
    {
        arrowRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (player == null || target == null) return;

        // 플레이어 → 목적지 방향 벡터 (수평 기준)
        Vector3 toTarget = target.position - player.position;
        toTarget.y = 0f;

        // 플레이어의 정면
        Vector3 forward = player.forward;
        forward.y = 0f;

        // 방향이 너무 작으면 회전 무시
        if (toTarget.sqrMagnitude < 0.0001f) return;

        // 각도 계산
        float angle = Vector3.SignedAngle(forward, toTarget, Vector3.up);

        // UI에서는 Z축 회전만 적용
        arrowRect.localEulerAngles = new Vector3(0, 0, -angle);
    }
}
