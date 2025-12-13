using UnityEngine;

/// <summary>
/// 필드에 있는 아이템을 회전시키고 띄우는 스크립트
/// 폭탄 모델 프리팹에 이 스크립트가 붙어있으면
/// 투척 시 자동으로 제거됩니다
/// </summary>
public class RotateItem : MonoBehaviour
{
    [Header("회전 설정")]
    public float rotationSpeed = 50f;
    public Vector3 rotationAxis = Vector3.up;

    [Header("상하 움직임 설정")]
    public float floatAmplitude = 0.3f;  // 상하 움직임 범위
    public float floatSpeed = 2f;        // 상하 움직임 속도

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // 회전
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);

        // 상하 움직임 (사인파 사용)
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}