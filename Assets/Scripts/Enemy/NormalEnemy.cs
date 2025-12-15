using UnityEngine;

public class NormalEnemy : EnemyBase
{
    [Header("이동 / 어그로 설정")]
    public float moveSpeed = 3f;        // 몬스터 이동 속도
    public float aggroRange = 8f;       // 이 거리 안으로 플레이어가 들어오면 어그로 ON
    public float loseAggroRange = 12f;  // 이 거리보다 멀어지면 어그로 OFF
    public float stopDistance = 1.5f;   // 플레이어에게 이 정도까지 가까이 가면 더 이상 다가가지 않음

    private bool isAggro = false;

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        // 어그로 ON 조건
        if (!isAggro && dist <= aggroRange)
        {
            isAggro = true;
        }
        // 어그로 OFF 조건
        else if (isAggro && dist > loseAggroRange)
        {
            isAggro = false;
        }

        // 보호막 상태 업데이트 (부모 클래스 메서드)
        UpdateShieldStatus();

        // 어그로 상태이고 이동 가능할 때만 추격
        if (isAggro && CanMove())
        {
            ChasePlayer(dist);
        }
    }

    void ChasePlayer(float distanceToPlayer)
    {
        // Y축은 고정해서 평면에서만 따라가게
        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        Vector3 dir = (targetPos - transform.position).normalized;

        // 너무 붙어 있지 않을 때만 이동
        if (distanceToPlayer > stopDistance)
        {
            transform.position += dir * moveSpeed * Time.deltaTime;
        }

        // 바라보는 방향 회전
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
        }
    }

    // 플레이어와 '닿았을 때' HP 1 감소
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"Enemy Trigger with: {other.name}");

        if (other.CompareTag("Player"))
        {
            // 보호막 체크 (부모 클래스 메서드)
            if (!CanDamagePlayer())
            {
                return;  // 보호막 활성화 중이면 데미지 없음
            }

            //Debug.Log("Player 감지! 데미지 시도");

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(touchDamage);
                //Debug.Log($"Player에게 {touchDamage} 데미지!");
            }
            else
            {
                //Debug.LogWarning("Player 오브젝트에서 PlayerHealth를 찾지 못했습니다.");
            }
        }
    }

}


