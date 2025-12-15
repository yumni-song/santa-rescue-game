using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/Bomb")]
public class ItemBombEft : ItemEffect
{
    [Header("투척 설정")]
    [Range(5f, 25f)]
    public float throwForce = 10f;      // 투척 힘 (기본값 낮춤)
    [Range(15f, 75f)]
    public float throwAngle = 35f;      // 투척 각도 (기본값 낮춤)

    [Header("생성 위치")]
    public float spawnDistance = 2f;         // 카메라 앞 생성 거리
    public float spawnHeightOffset = -0.5f;  // 카메라 기준 높이 오프셋 (음수=아래, 양수=위)

    [Header("카메라 없을 때 (폴백)")]
    public float fallbackSpawnHeight = 1.5f; // 플레이어 발 기준 높이

    [Header("폭탄 속성")]
    public float explosionRadius = 5f;
    public int damage = 2;
    public GameObject explosionEffectPrefab;

    public override bool ExecuteRole()
    {
        Debug.Log("폭탄 사용");

        // 플레이어 찾기
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("플레이어를 찾을 수 없습니다!");
            return false;
        }

        // 인벤토리에서 폭탄 아이템 찾기 (itemModel 사용)
        Item bombItem = GetBombItemFromInventory();
        if (bombItem == null || bombItem.itemModel == null)
        {
            Debug.LogError("폭탄 아이템 모델을 찾을 수 없습니다!");
            return false;
        }

        // 투척 방향 및 생성 위치 계산
        Camera mainCamera = Camera.main;
        Vector3 throwDirection;
        Vector3 spawnPosition;

        if (mainCamera != null)
        {
            // 카메라 중심에서 약간 앞쪽 + 아래쪽에 생성
            spawnPosition = mainCamera.transform.position +
                           mainCamera.transform.forward * spawnDistance +
                           Vector3.up * spawnHeightOffset;  // 높이 오프셋 적용
            throwDirection = mainCamera.transform.forward;
        }
        else
        {
            // 카메라 없으면 플레이어 기준
            spawnPosition = player.transform.position +
                           Vector3.up * fallbackSpawnHeight +
                           player.transform.forward * spawnDistance;
            throwDirection = player.transform.forward;
        }

        // 폭탄 모델 인스턴스화
        GameObject bomb = Instantiate(bombItem.itemModel, spawnPosition, Quaternion.identity);

        // Rigidbody 추가 (투척용)
        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = bomb.AddComponent<Rigidbody>();
        }
        rb.useGravity = true;  // 중력 활성화
        rb.isKinematic = false;

        // Collider 확인 및 추가
        Collider col = bomb.GetComponent<Collider>();
        if (col == null)
        {
            // 기본 Sphere Collider 추가
            SphereCollider sphereCol = bomb.AddComponent<SphereCollider>();
            sphereCol.radius = 0.3f;
        }

        // BombProjectile 컴포넌트 추가
        BombProjectile bombScript = bomb.AddComponent<BombProjectile>();
        bombScript.explosionRadius = explosionRadius;
        bombScript.damage = damage;
        bombScript.explosionEffectPrefab = explosionEffectPrefab;

        // 포물선 투척을 위한 속도 계산
        Vector3 throwVelocity = CalculateThrowVelocity(throwDirection, throwAngle);
        rb.velocity = throwVelocity;

        Debug.Log($"폭탄 투척! 힘: {throwForce}, 각도: {throwAngle}도, 속도: {throwVelocity}");

        return true;
    }

    /// <summary>
    /// 인벤토리에서 폭탄 아이템 찾기
    /// </summary>
    Item GetBombItemFromInventory()
    {
        Inventory inventory = Inventory.instance;
        if (inventory == null) return null;

        foreach (Item item in inventory.GetItems())
        {
            // 폭탄 아이템 타입으로 찾기
            if (item.itemType == ItemType.Bomb)
            {
                return item;
            }
        }
        return null;
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