using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("공통 스탯")]
    public int maxHP = 2;
    protected int currentHP;

    [Tooltip("플레이어와 닿았을 때 줄 데미지")]
    public int touchDamage = 1;

    [Tooltip("원거리 공격(투사체 등)에 사용할 공격력")]
    public int attackDamage = 1;

    [Header("사망 이펙트")]
    public GameObject deathEffectPrefab;
    public float deathEffectDuration = 2f;
    [Tooltip("이펙트 생성 높이 오프셋 (몬스터 위치 기준)")]
    public float deathEffectHeightOffset = 1.5f;

    [Header("사망 사운드")]
    public AudioClip deathSound;
    [Range(0f, 1f)]
    public float deathSoundVolume = 0.7f;

    [Header("보호막 감지")]
    protected bool isNearShield = false;
    protected PlayerShield playerShield;
    protected Transform player;

    [Header("보호막 진동 방지 설정")]
    [Tooltip("보호막 경계에서 정지할 때 사용할 여유 거리")]
    public float shieldStopBuffer = 1.0f;

    [Tooltip("보호막에 밀렸을 때 잠시 대기할 시간")]
    public float repelCooldown = 0.5f;

    protected float lastRepelTime = -999f;
    protected bool wasRepelled = false;

    protected virtual void Start()
    {
        currentHP = maxHP;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerShield = playerObj.GetComponent<PlayerShield>();
        }
        else
        {
            Debug.LogWarning("Player 태그를 가진 오브젝트를 찾을 수 없습니다!");
        }
    }

    protected void UpdateShieldStatus()
    {
        if (player == null)
        {
            isNearShield = false;
            wasRepelled = false;
            return;
        }

        if (playerShield == null)
        {
            playerShield = player.GetComponent<PlayerShield>();
        }

        if (playerShield != null && playerShield.isShieldActive)
        {
            float distToPlayer = Vector3.Distance(transform.position, player.position);
            float stopDistance = playerShield.ShieldRadius + shieldStopBuffer;

            if (distToPlayer <= stopDistance)
            {
                isNearShield = true;
                CheckIfRepelled();
                return;
            }
        }

        isNearShield = false;
        wasRepelled = false;
    }

    protected virtual void CheckIfRepelled()
    {
        if (Time.time - lastRepelTime < repelCooldown)
        {
            wasRepelled = true;
            return;
        }

        if (playerShield != null && playerShield.isShieldActive)
        {
            float distToPlayer = Vector3.Distance(transform.position, player.position);

            if (distToPlayer >= playerShield.ShieldRadius - 0.5f &&
                distToPlayer <= playerShield.ShieldRadius + shieldStopBuffer)
            {
                if (!wasRepelled)
                {
                    lastRepelTime = Time.time;
                    wasRepelled = true;
                }
            }
        }
    }

    protected bool CanMove()
    {
        if (isNearShield)
            return false;

        if (wasRepelled && Time.time - lastRepelTime < repelCooldown)
            return false;

        return true;
    }

    protected bool CanAttack()
    {
        return !isNearShield;
    }

    protected bool CanDamagePlayer()
    {
        return playerShield == null || !playerShield.isShieldActive;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }
    }

    protected virtual void Die()
    {
        // 사망 이펙트 생성 (공중에 띄우기)
        if (deathEffectPrefab != null)
        {
            Vector3 effectPosition = transform.position + Vector3.up * deathEffectHeightOffset;
            GameObject effect = Instantiate(deathEffectPrefab, effectPosition, Quaternion.identity);
            Destroy(effect, deathEffectDuration);
            Debug.Log($"{gameObject.name} 사망 이펙트 생성! (높이: {deathEffectHeightOffset}m)");
        }

        // 사망 사운드 재생
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position, deathSoundVolume);
            Debug.Log($"{gameObject.name} 사망 사운드 재생!");
        }

        // 몬스터 제거
        Destroy(gameObject);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        if (isNearShield)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
        else if (wasRepelled && Time.time - lastRepelTime < repelCooldown)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}