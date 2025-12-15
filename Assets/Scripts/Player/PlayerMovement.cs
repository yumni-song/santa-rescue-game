using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // 이동 관련 설정
    public float walkSpeed = 5f;
    public float sprintMultiplier = 2f;

    // 점프 / 중력 설정
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public int maxJumps = 2;

    // 내부 변수
    private CharacterController controller;
    private Vector3 velocity;
    private int jumpsRemaining;
    private bool isFrozen = false;  // 사망 시 움직임 정지 플래그

    void Start()
    {
        controller = GetComponent<CharacterController>();
        jumpsRemaining = maxJumps;
    }

    void Update()
    {
        // 얼어붙은 상태면 아무것도 하지 않음
        if (isFrozen) return;

        // WASD 이동 입력
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float currentSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            currentSpeed *= sprintMultiplier;

        Vector3 move = (transform.right * x + transform.forward * z) * currentSpeed;

        // 점프 처리
        if (Input.GetKeyDown(KeyCode.Space) && jumpsRemaining > 0)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpsRemaining--;
        }

        // 중력 적용
        velocity.y += gravity * Time.deltaTime;

        // 이동 + 충돌 처리
        CollisionFlags flags = controller.Move((move + velocity) * Time.deltaTime);

        // 바닥 착지 판정
        if ((flags & CollisionFlags.Below) != 0)
        {
            velocity.y = -2f;
            jumpsRemaining = maxJumps;
        }
    }

    // 플레이어 움직임 완전 정지 (사망 시 호출)
    public void FreezePlayer()
    {
        isFrozen = true;
        velocity = Vector3.zero;  // 속도 0으로

        Debug.Log("플레이어 움직임 정지");
    }
}