using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("플레이어 HP 설정")]
    public int maxHP = 10;
    public int currentHP;

    [Header("피격 무적 시간 설정")]
    public float invincibleDuration = 0.8f;  // 한 번 맞고 0.8초 동안은 추가 데미지 없음
    private float lastDamageTime = -999f;    // 마지막으로 데미지 받은 시간

    public bool IsDead => currentHP <= 0;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) return;

        // 최근에 맞은 뒤로 invincibleDuration이 지나지 않았으면 무시
        if (Time.time < lastDamageTime + invincibleDuration)
        {
            return;
        }

        lastDamageTime = Time.time;  // 이번에 맞은 시간 기록

        currentHP -= damage;

        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }

        Debug.Log($"Player HP: {currentHP}/{maxHP}");
        // TODO: 여기서 피격 이펙트, 깜빡임, 사운드 등 넣어도 됨
    }

    public void Heal(int amount)
    {
        if (IsDead) return;

        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;

        Debug.Log($"Player Heal → HP: {currentHP}/{maxHP}");
    }

    private void Die()
    {
        Debug.Log("플레이어 사망!");
        // TODO: 조작 막기, 게임오버 UI, 씬 재시작 등
    }
}


