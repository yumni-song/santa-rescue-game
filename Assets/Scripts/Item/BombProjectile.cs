using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombProjectile : MonoBehaviour
{
    [Header("폭발 설정")]
    public float explosionRadius = 5f;
    public int damage = 2;
    public GameObject explosionEffectPrefab;

    [Header("폭탄 설정")]
    public float lifeTime = 5f; // 최대 생존 시간

    private bool hasExploded = false;

    void Start()
    {
        // 투척된 폭탄만 자동 폭발 (lifeTime 후)
        // 이 스크립트는 투척 시에만 추가되므로 문제없음
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasExploded) return;

        Debug.Log($"폭탄이 {collision.gameObject.name}에 충돌!");
        Explode();
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        Vector3 explosionPosition = transform.position;

        // 폭발 이펙트 생성
        if (explosionEffectPrefab != null)
        {
            GameObject effect = Instantiate(explosionEffectPrefab, explosionPosition, Quaternion.identity);
            Destroy(effect, 2f);
        }

        // 폭발 범위 내 적들에게 데미지
        Collider[] hitColliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

        // 중복 방지를 위해 이미 데미지 받은 적 추적
        HashSet<EnemyBase> damagedEnemies = new HashSet<EnemyBase>();
        int enemyCount = 0;

        foreach (Collider col in hitColliders)
        {
            EnemyBase enemy = col.GetComponent<EnemyBase>();
            if (enemy != null && !damagedEnemies.Contains(enemy))
            {
                enemy.TakeDamage(damage);
                damagedEnemies.Add(enemy);  // 이미 데미지 받은 적으로 등록
                enemyCount++;
                Debug.Log($"{col.name}에게 {damage} 데미지!");
            }
        }

        Debug.Log($"총 {enemyCount}마리의 적에게 데미지를 입혔습니다!");

        // 폭탄 오브젝트 파괴
        Destroy(gameObject);
    }

    // 기즈모로 폭발 범위 표시 (에디터에서만)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}