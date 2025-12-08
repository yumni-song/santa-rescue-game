using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("탄 기본 설정")]
    public float speed = 10f;
    public int damage = 3;      // 마녀/유령 공격력
    public float lifeTime = 100f; // 일정 시간 지나면 자동 삭제

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // 탄이 자신의 앞으로 계속 나아가게
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어 맞았을 때만 데미지
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            //Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            // 벽, 지형 등의 일반 콜라이더에 닿으면 소멸
            //Destroy(gameObject);
        }
    }
}

