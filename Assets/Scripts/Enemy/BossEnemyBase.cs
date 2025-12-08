using UnityEngine;

public class BossEnemyBase : EnemyBase
{
    [Header("보스 이동/어그로 설정")]
    public bool chasePlayer = true;      // 보스가 플레이어를 따라갈지 여부
    public float moveSpeed = 2.5f;
    public float stopDistance = 4f;      // 이 거리까지만 접근하고 더는 안 다가감

    [Header("어그로 설정")]
    public float aggroRange = 15f;       // 이 거리 안으로 들어오면 어그로 ON
    public float loseAggroRange = 25f;   // 이 거리보다 멀어지면 어그로 OFF
    protected bool isAggro = false;

    [Header("공격 설정")]
    public float attackRange = 12f;      // 이 거리 안에 있으면 공격 시도
    public float attackCooldown = 2f;    // 공격 쿨타임
    protected float lastAttackTime = -999f;

    [Header("투사체 설정")]
    public GameObject projectilePrefab;      // Inspector에서 이펙트 프리팹 넣어줄 슬롯
    public Transform projectileSpawnPoint;   // 손/지팡이 끝 같은 위치

    public float rotateSpeed = 5f;

    protected override void Start()
    {
        base.Start(); // EnemyBase에서 player 찾고 currentHP 초기화
    }

    protected virtual void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        // 어그로 ON/OFF
        if (!isAggro && dist <= aggroRange)
            isAggro = true;
        else if (isAggro && dist > loseAggroRange)
            isAggro = false;

        if (!isAggro) return; // 어그로 안 끌렸으면 아무것도 안 함

        LookAtPlayer();

        if (chasePlayer)
        {
            ChasePlayer(dist);
        }

        if (dist <= attackRange)
        {
            TryAttack();
        }
    }

    protected void LookAtPlayer()
    {
        Vector3 dir = (player.position - transform.position);
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.001f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
    }

    protected void ChasePlayer(float distanceToPlayer)
    {
        if (distanceToPlayer <= stopDistance) return;

        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        Vector3 dir = (targetPos - transform.position).normalized;

        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    protected void TryAttack()
    {
        if (projectilePrefab == null || projectileSpawnPoint == null)
        {
            Debug.LogWarning("BossEnemyBase: projectilePrefab 또는 projectileSpawnPoint가 비어 있음");
            return;
        }

        if (Time.time < lastAttackTime + attackCooldown) return;

        lastAttackTime = Time.time;
        Shoot();
    }

    protected virtual void Shoot()
    {
        // 실제 발사
        GameObject proj = Instantiate(
            projectilePrefab,
            projectileSpawnPoint.position,
            projectileSpawnPoint.rotation);

        // 투사체에 보스 공격력 전달
        EnemyProjectile ep = proj.GetComponent<EnemyProjectile>();
        if (ep != null)
        {
            ep.damage = attackDamage;
        }
    }
}

