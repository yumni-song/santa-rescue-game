using UnityEngine;

public class SideFollower : MonoBehaviour
{
    public Transform player;

    [Header("Follow Settings")]
    public float followSpeed = 3f;
    public float rotateSpeed = 10f;
    public float followDistance = 1.5f;

    [Header("Offset")]
    public Vector3 localOffset;

    [Header("Start Mode")]
    public bool startFollowingOnStart = false;   // Scene2에서는 true로 체크

    private bool isFollowing = false;

    void Start()
    {
        if (startFollowingOnStart)
            isFollowing = true;
    }

    void Update()
    {
        if (!isFollowing || player == null) return;

        Vector3 targetPos = player.TransformPoint(localOffset);

        Vector3 dir = targetPos - transform.position;
        dir.y = 0f;

        if (dir.magnitude < followDistance) return;

        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir.normalized);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotateSpeed * Time.deltaTime
            );
        }

        transform.position += dir.normalized * followSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // startFollowingOnStart가 true여도 상관 없게 그냥 둬도 됨
        if (other.CompareTag("Player"))
            isFollowing = true;
    }
}

