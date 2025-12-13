using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballProjectile : MonoBehaviour
{
    [Header("눈덩이 설정")]
    public int damage = 1;              // 데미지
    public float lifeTime = 5f;         // 최대 생존 시간
    public GameObject hitEffectPrefab;  // 충돌 이펙트 (선택)

    [Header("충돌 무시 설정")]
    public float ignorePlayerTime = 0.2f; // 생성 후 이 시간동안 플레이어와 충돌 무시

    private bool hasHit = false;
    private float spawnTime;

    void Start()
    {
        spawnTime = Time.time;

        // 일정 시간 후 자동 삭제
        Destroy(gameObject, lifeTime);

        // 플레이어와 물리적 충돌 무시
        IgnorePlayerCollision();
    }

    void IgnorePlayerCollision()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Collider snowballCollider = GetComponent<Collider>();
            Collider[] playerColliders = player.GetComponentsInChildren<Collider>();

            foreach (Collider playerCol in playerColliders)
            {
                if (snowballCollider != null && playerCol != null)
                {
                    Physics.IgnoreCollision(snowballCollider, playerCol);
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;

        // 생성 직후 짧은 시간동안은 플레이어와 충돌 무시 (추가 안전장치)
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time - spawnTime < ignorePlayerTime)
            {
                Debug.Log("생성 직후 - 플레이어와 충돌 무시");
                return;
            }
        }

        hasHit = true;

        Debug.Log($"눈덩이가 {collision.gameObject.name}에 충돌!");

        // 적에게 데미지
        EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log($" {collision.gameObject.name}에게 {damage} 데미지!");
        }
        else
        {
            Debug.Log($" {collision.gameObject.name}에는 EnemyBase가 없습니다");
        }

        // 충돌 이펙트 생성
        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
            Debug.Log("충돌 이펙트 생성!");
        }
        else
        {
            Debug.Log("hitEffectPrefab이 설정되지 않았습니다");
        }

        // 눈덩이 파괴
        Destroy(gameObject);
    }
}