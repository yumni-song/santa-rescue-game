using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    /* ======================
     * 이동 관련 설정
     * ====================== */

    public float walkSpeed = 5f;          // 기본 걷기 속도
    public float sprintMultiplier = 2f;   // Shift 누를 때 속도 배수

    /* ======================
     * 점프 / 중력 설정
     * ====================== */

    public float gravity = -9.81f;         // 중력 값 (음수)
    public float jumpHeight = 2f;           // 점프 높이
    public int maxJumps = 2;                // 최대 연속 점프 횟수 (2 = 더블점프)

    /* ======================
     * 내부 변수
     * ====================== */

    private CharacterController controller; // 캐릭터 컨트롤러
    private Vector3 velocity;               // y축 속도 관리용
    private int jumpsRemaining;              // 남은 점프 횟수

    void Start()
    {
        // CharacterController 컴포넌트 가져오기
        controller = GetComponent<CharacterController>();

        // 시작 시 점프 횟수 초기화
        jumpsRemaining = maxJumps;
    }

    void Update()
    {
        /* ======================
         *  WASD 이동 입력
         * ====================== */

        float x = Input.GetAxis("Horizontal"); // A(-1) / D(+1)
        float z = Input.GetAxis("Vertical");   // S(-1) / W(+1)

        // Shift 키가 눌려 있으면 달리기 속도 적용
        float currentSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            currentSpeed *= sprintMultiplier;

        // 로컬 기준 이동 방향 계산
        Vector3 move = (transform.right * x + transform.forward * z) * currentSpeed;

        /* ======================
         * 점프 처리 (Space)
         * ====================== */

        // 남은 점프 횟수가 있을 때만 점프 가능
        if (Input.GetKeyDown(KeyCode.Space) && jumpsRemaining > 0)
        {
            // 점프 속도 계산 (물리 공식)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // 점프 횟수 차감
            jumpsRemaining--;
        }

        /* ======================
         * 중력 적용
         * ====================== */

        // 매 프레임 중력 누적
        velocity.y += gravity * Time.deltaTime;

        /* ======================
         * 이동 + 충돌 처리
         * ====================== */

        // 수평 이동(move) + 수직 이동(velocity)을 합쳐 한 번에 Move
        CollisionFlags flags = controller.Move((move + velocity) * Time.deltaTime);

        /* ======================
         * 바닥 착지 판정
         * ====================== */

        // 아래쪽(Below) 충돌이 발생했으면 "바닥에 닿음"
        if ((flags & CollisionFlags.Below) != 0)
        {
            // 바닥에 밀착시키기 위한 작은 음수값
            velocity.y = -2f;

            // 점프 횟수 리셋 (착지했으므로 다시 2번 가능)
            jumpsRemaining = maxJumps;
        }
    }
}
