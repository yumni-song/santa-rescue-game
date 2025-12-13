using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballThrower : MonoBehaviour
{
    [Header("눈덩이 프리팹")]
    public GameObject snowballPrefab;

    [Header("투척 설정")]
    [Range(5f, 30f)]
    public float throwForce = 15f;
    [Range(15f, 75f)]
    public float throwAngle = 35f;

    [Header("생성 위치")]
    public float spawnDistance = 2f;         // 카메라 앞 생성 거리
    public float spawnHeightOffset = 0f;     // 카메라 기준 높이 오프셋 (0으로 변경!)
    public float fallbackSpawnHeight = 1.5f; // 카메라 없을 때 플레이어 기준 높이

    [Header("쿨다운 설정")]
    public float throwCooldown = 0f;
    private float lastThrowTime = -999f;

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // 우클릭
        {
            TryThrowSnowball();
        }
    }

    void TryThrowSnowball()
    {
        // 쿨다운 체크
        if (Time.time < lastThrowTime + throwCooldown)
        {
            Debug.Log("눈덩이 쿨다운 중...");
            return;
        }

        if (snowballPrefab == null)
        {
            Debug.LogError("눈덩이 프리팹이 설정되지 않았습니다!");
            return;
        }

        lastThrowTime = Time.time;
        ThrowSnowball();
    }

    void ThrowSnowball()
    {
        // 투척 방향 및 생성 위치 계산
        Camera mainCamera = Camera.main;
        Vector3 throwDirection;
        Vector3 spawnPosition;

        if (mainCamera != null)
        {
            // 카메라 중심에서 약간 앞쪽 + 오프셋 적용
            spawnPosition = mainCamera.transform.position +
                           mainCamera.transform.forward * spawnDistance +
                           Vector3.up * spawnHeightOffset;
            throwDirection = mainCamera.transform.forward;
        }
        else
        {
            // 카메라 없으면 플레이어 기준
            spawnPosition = transform.position +
                           Vector3.up * fallbackSpawnHeight +
                           transform.forward * spawnDistance;
            throwDirection = transform.forward;
        }

        // 눈덩이 생성
        GameObject snowball = Instantiate(snowballPrefab, spawnPosition, Quaternion.identity);

        // 강제로 위치 고정 (프리팹의 다른 스크립트가 위치 변경 못하게)
        snowball.transform.position = spawnPosition;

        // Rigidbody 확인 및 설정
        Rigidbody rb = snowball.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = snowball.AddComponent<Rigidbody>();
        }
        rb.useGravity = true;
        rb.isKinematic = false;

        // Collider 확인
        Collider col = snowball.GetComponent<Collider>();
        if (col == null)
        {
            SphereCollider sphereCol = snowball.AddComponent<SphereCollider>();
            sphereCol.radius = 0.15f;
        }

        // 플레이어와 충돌 무시
        IgnorePlayerCollision(snowball);

        // 포물선 투척
        Vector3 throwVelocity = CalculateThrowVelocity(throwDirection, throwAngle);
        rb.velocity = throwVelocity;

        Debug.Log($"눈덩이 투척! 위치: {spawnPosition}, 속도: {rb.velocity.magnitude}");
    }

    void IgnorePlayerCollision(GameObject snowball)
    {
        Collider snowballCollider = snowball.GetComponent<Collider>();
        if (snowballCollider == null) return;

        // 플레이어의 모든 Collider와 충돌 무시
        Collider[] playerColliders = GetComponentsInChildren<Collider>();
        foreach (Collider playerCol in playerColliders)
        {
            if (playerCol != null)
            {
                Physics.IgnoreCollision(snowballCollider, playerCol);
            }
        }
    }

    /// <summary>
    /// 포물선 투척을 위한 속도 벡터 계산
    /// </summary>
    Vector3 CalculateThrowVelocity(Vector3 direction, float angle)
    {
        // 수평 방향
        Vector3 horizontalDir = new Vector3(direction.x, 0, direction.z).normalized;

        // 각도를 라디안으로 변환
        float angleRad = angle * Mathf.Deg2Rad;

        // 수평 및 수직 속도 성분
        float horizontalSpeed = throwForce * Mathf.Cos(angleRad);
        float verticalSpeed = throwForce * Mathf.Sin(angleRad);

        // 최종 속도 벡터
        Vector3 velocity = horizontalDir * horizontalSpeed + Vector3.up * verticalSpeed;

        return velocity;
    }
}