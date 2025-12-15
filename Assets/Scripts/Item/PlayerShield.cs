using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [Header("보호막 상태")]
    public bool isShieldActive = false;
    private float shieldRadius;

    public float ShieldRadius => shieldRadius;

    private float shieldEndTime;
    private GameObject currentShieldEffect;
    private HashSet<EnemyBase> freezedEnemies = new HashSet<EnemyBase>();

    [Header("밀어내기 설정")]
    [Tooltip("몬스터를 밀어내는 속도 (초당 이동 거리)")]
    public float repelSpeed = 3f;

    [Tooltip("밀어낼 목표 거리 (보호막 반경 + 이 값)")]
    public float repelExtraDistance = 0.5f;

    // 효과음 관련
    private AudioSource loopAudioSource;

    void OnDestroy()
    {
        if (isShieldActive)
        {
            DeactivateShield();
        }
    }

    void Update()
    {
        if (isShieldActive)
        {
            if (Time.time >= shieldEndTime)
            {
                DeactivateShield();
            }
            else
            {
                FreezeAndRepelNearbyEnemies();
            }
        }
    }

    public void ActivateShield(float duration, float radius, GameObject effectPrefab,
                              float effectOffset, float effectSize,
                              AudioClip activateSound, AudioClip loopSound, float volume)
    {
        if (isShieldActive && currentShieldEffect != null)
        {
            shieldEndTime = Time.time + duration;
            Debug.Log("보호막 시간 연장!");
            return;
        }

        isShieldActive = true;
        shieldRadius = radius;
        shieldEndTime = Time.time + duration;

        Debug.Log($"보호막 활성화! {duration}초 동안 지속");

        // 활성화 효과음 재생 (1회)
        if (activateSound != null)
        {
            AudioSource.PlayClipAtPoint(activateSound, transform.position, volume);
            Debug.Log("보호막 생성 효과음 재생!");
        }

        // 루프 효과음 시작
        if (loopSound != null)
        {
            StartLoopSound(loopSound, volume);
        }

        // 이펙트 생성
        if (effectPrefab != null)
        {
            Camera mainCamera = Camera.main;
            Vector3 effectPosition = transform.position;

            if (mainCamera != null)
            {
                Vector3 cameraForward = mainCamera.transform.forward;
                effectPosition += cameraForward * effectOffset;
            }

            currentShieldEffect = Instantiate(effectPrefab, effectPosition, Quaternion.identity);
            currentShieldEffect.transform.SetParent(transform);
            currentShieldEffect.transform.localScale = Vector3.one * (radius * effectSize);

            Debug.Log($"이펙트 생성 위치: {effectPosition}, 크기 배율: {effectSize}");
        }

        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            StartCoroutine(TemporaryInvincibility(duration));
        }
    }

    // 루프 효과음 시작
    void StartLoopSound(AudioClip loopSound, float volume)
    {
        // 기존 AudioSource가 있으면 정지
        StopLoopSound();

        // AudioSource 컴포넌트 추가
        loopAudioSource = gameObject.AddComponent<AudioSource>();
        loopAudioSource.clip = loopSound;
        loopAudioSource.volume = volume * 0.6f;  // 루프음은 좀 더 작게
        loopAudioSource.loop = true;  // 반복 재생!
        loopAudioSource.spatialBlend = 0f;  // 2D 사운드
        loopAudioSource.Play();

        Debug.Log("보호막 루프 효과음 시작!");
    }

    // 루프 효과음 정지
    void StopLoopSound()
    {
        if (loopAudioSource != null)
        {
            loopAudioSource.Stop();
            Destroy(loopAudioSource);
            loopAudioSource = null;
            Debug.Log("보호막 루프 효과음 정지!");
        }
    }

    void DeactivateShield()
    {
        isShieldActive = false;

        // 루프 효과음 정지
        StopLoopSound();

        UnfreezeAllEnemies();
        freezedEnemies.Clear();

        Debug.Log("보호막 해제!");

        if (currentShieldEffect != null)
        {
            Destroy(currentShieldEffect);
            currentShieldEffect = null;
        }
    }

    void FreezeAndRepelNearbyEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, shieldRadius);

        foreach (Collider col in colliders)
        {
            EnemyBase enemy = col.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                // 동결 처리
                if (!freezedEnemies.Contains(enemy))
                {
                    FreezeEnemy(enemy);
                    freezedEnemies.Add(enemy);
                    Debug.Log($"{col.name}을(를) 동결!");
                }

                float distanceToPlayer = Vector3.Distance(col.transform.position, transform.position);
                float targetDistance = shieldRadius + repelExtraDistance;

                // 보호막 범위 내에 있으면 서서히 밀어냄
                if (distanceToPlayer < targetDistance)
                {
                    Vector3 direction = (col.transform.position - transform.position).normalized;

                    // 방향이 0에 가까우면 랜덤 방향 생성
                    if (direction.magnitude < 0.01f)
                    {
                        direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
                    }

                    // 목표 위치 계산
                    Vector3 targetPosition = transform.position + direction * targetDistance;
                    targetPosition.y = col.transform.position.y;

                    // 현재 위치에서 목표 위치로 부드럽게 이동
                    Vector3 newPosition = Vector3.MoveTowards(
                        col.transform.position,
                        targetPosition,
                        repelSpeed * Time.deltaTime
                    );

                    col.transform.position = newPosition;
                }
            }
        }

        // 보호막 범위를 벗어난 적들 동결 해제
        List<EnemyBase> toUnfreeze = new List<EnemyBase>();
        foreach (EnemyBase enemy in freezedEnemies)
        {
            if (enemy == null)
            {
                toUnfreeze.Add(enemy);
                continue;
            }

            float dist = Vector3.Distance(transform.position, enemy.transform.position);

            // 목표 거리보다 충분히 멀어지면 동결 해제
            if (dist > shieldRadius + repelExtraDistance + 0.5f)
            {
                toUnfreeze.Add(enemy);
            }
        }

        foreach (EnemyBase enemy in toUnfreeze)
        {
            if (enemy != null)
            {
                UnfreezeEnemy(enemy);
            }
            freezedEnemies.Remove(enemy);
        }
    }

    void FreezeEnemy(EnemyBase enemy)
    {
        NormalEnemy normalEnemy = enemy as NormalEnemy;
        if (normalEnemy != null)
        {
            normalEnemy.enabled = false;
            return;
        }

        BossEnemyBase bossEnemy = enemy as BossEnemyBase;
        if (bossEnemy != null)
        {
            bossEnemy.enabled = false;
            return;
        }
    }

    void UnfreezeEnemy(EnemyBase enemy)
    {
        if (enemy == null) return;

        NormalEnemy normalEnemy = enemy as NormalEnemy;
        if (normalEnemy != null)
        {
            normalEnemy.enabled = true;
            return;
        }

        BossEnemyBase bossEnemy = enemy as BossEnemyBase;
        if (bossEnemy != null)
        {
            bossEnemy.enabled = true;
            return;
        }
    }

    void UnfreezeAllEnemies()
    {
        if (freezedEnemies == null) return;

        foreach (EnemyBase enemy in freezedEnemies)
        {
            UnfreezeEnemy(enemy);
        }

        freezedEnemies.Clear();
    }

    IEnumerator TemporaryInvincibility(float duration)
    {
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth == null) yield break;

        float originalInvincibleDuration = playerHealth.invincibleDuration;
        playerHealth.invincibleDuration = 9999f;

        yield return new WaitForSeconds(duration);

        playerHealth.invincibleDuration = originalInvincibleDuration;
    }

    void OnDrawGizmosSelected()
    {
        if (isShieldActive)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, shieldRadius);

            // 밀어내기 목표 거리 표시
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, shieldRadius + repelExtraDistance);
        }
    }
}