using UnityEngine;

public class SideFollowerOnTouch : MonoBehaviour
{
    public Transform player;

    [Header("Follow Settings")]
    public float followSpeed = 3f;
    public float rotateSpeed = 10f;
    public float followDistance = 1.5f;

    [Header("Offset")]
    public Vector3 localOffset;
    // 산타: (-1.2, 0, -1.5)
    // 루돌프: ( 1.2, 0, -1.5)

    private bool isFollowing = false;

    void Update()
    {
        if (!isFollowing || player == null) return;

        // 플레이어 기준 좌/우/뒤 위치 계산
        Vector3 targetPos = player.TransformPoint(localOffset);

        Vector3 dir = targetPos - transform.position;
        dir.y = 0f;

        if (dir.magnitude < followDistance) return;

        // 회전
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir.normalized);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotateSpeed * Time.deltaTime
            );
        }

        // 이동 (관통 이동)
        transform.position += dir.normalized * followSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isFollowing = true;
        }
    }
}
