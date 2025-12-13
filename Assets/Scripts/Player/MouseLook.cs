using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 80f;  // 처음엔 50~100 사이로 시작 추천
    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // 커서 화면 중앙 고정
    }

    void Update()
    {
        if (playerBody == null) return;  // 혹시라도 연결 안 되어 있을 때 안전장치

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 위아래 회전 (카메라만)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);  // 목 꺾이지 않게 제한

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 좌우 회전 (플레이어 몸통)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}

