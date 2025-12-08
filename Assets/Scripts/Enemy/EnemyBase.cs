using UnityEngine;

public class EnemyBase : MonoBehaviour
{

    [Header("공통 스탯")]
    public int maxHP = 2;           // 기본 HP (일반 몬스터는 2)
    protected int currentHP;

    [Tooltip("플레이어와 닿았을 때 줄 데미지")]
    public int touchDamage = 1;     // 일반 몬스터는 1

    [Tooltip("원거리 공격(투사체 등)에 사용할 공격력")]
    public int attackDamage = 1;

    protected Transform player;

    protected virtual void Start()
    {
        currentHP = maxHP;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Player 태그를 가진 오브젝트를 찾을 수 없습니다!");
        }
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
        // TODO: 죽는 애니메이션 있으면 여기서 재생 후 Destroy
        Destroy(gameObject);
    }
}

