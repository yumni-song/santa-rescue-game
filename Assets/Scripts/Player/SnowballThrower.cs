using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballThrower : MonoBehaviour
{
    // 눈덩이 프리팹
    public GameObject snowballPrefab;

    // 발사 설정
    [Range(10f, 50f)]
    public float shootSpeed = 25f;  // 발사 속도

    // 생성 위치
    public float spawnDistance = 1f;  // 카메라 앞 생성 거리

    // 쿨다운 설정
    public float shootCooldown = 0.3f;
    private float lastShootTime = -999f;

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // 우클릭
        {
            TryShootSnowball();
        }
    }

    void TryShootSnowball()
    {
        // 쿨다운 체크
        if (Time.time < lastShootTime + shootCooldown)
        {
            return;
        }

        if (snowballPrefab == null)
        {
            Debug.LogError("눈덩이 프리팹이 설정되지 않았습니다!");
            return;
        }

        lastShootTime = Time.time;
        ShootSnowball();
    }

    void ShootSnowball()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogWarning("메인 카메라를 찾을 수 없습니다!");
            return;
        }

        // 화면 중앙에서 발사 (FPS 방식)
        Vector3 spawnPosition = mainCamera.transform.position + mainCamera.transform.forward * spawnDistance;
        Vector3 shootDirection = mainCamera.transform.forward;

        // 눈덩이 생성
        GameObject snowball = Instantiate(snowballPrefab, spawnPosition, Quaternion.identity);

        // Rigidbody 설정
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

        // 직선 발사 (FPS 방식)
        rb.velocity = shootDirection * shootSpeed;

        Debug.Log($"눈덩이 발사! 속도: {shootSpeed}");
    }

    void IgnorePlayerCollision(GameObject snowball)
    {
        Collider snowballCollider = snowball.GetComponent<Collider>();
        if (snowballCollider == null) return;

        Collider[] playerColliders = GetComponentsInChildren<Collider>();
        foreach (Collider playerCol in playerColliders)
        {
            if (playerCol != null)
            {
                Physics.IgnoreCollision(snowballCollider, playerCol);
            }
        }
    }
}