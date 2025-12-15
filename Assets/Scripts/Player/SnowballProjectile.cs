using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballProjectile : MonoBehaviour
{
    // 눈덩이 설정
    public int damage = 1;
    public float lifeTime = 5f;
    public GameObject hitEffectPrefab;

    // 효과음 설정
    public AudioClip hitSound;
    [Range(0f, 1f)]
    public float hitSoundVolume = 0.7f;

    // 충돌 무시 설정
    public float ignorePlayerTime = 0.2f;

    private bool hasHit = false;
    private float spawnTime;

    void Start()
    {
        spawnTime = Time.time;
        Destroy(gameObject, lifeTime);
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

        // 생성 직후 플레이어와 충돌 무시
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time - spawnTime < ignorePlayerTime)
            {
                return;
            }
        }

        hasHit = true;
        Debug.Log($"눈덩이가 {collision.gameObject.name}에 충돌!");

        // 충돌 위치 계산 (정확한 충돌 지점)
        Vector3 hitPosition = collision.contacts[0].point;

        // 효과음 재생
        PlayHitSound(hitPosition);

        // 적에게 데미지
        EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log($"{collision.gameObject.name}에게 {damage} 데미지!");
        }

        // 충돌 이펙트 생성
        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, hitPosition, Quaternion.identity);
            Destroy(effect, 2f);
        }

        // 눈덩이 파괴
        Destroy(gameObject);
    }

    // 효과음 재생
    void PlayHitSound(Vector3 position)
    {
        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, position, hitSoundVolume);
            Debug.Log("충돌 효과음 재생!");
        }
        else
        {
            Debug.LogWarning("hitSound가 설정되지 않았습니다!");
        }
    }
}