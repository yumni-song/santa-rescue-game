using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    public float rotateSpeed = 50f; // 회전 속도
    public Vector3 rotateAxis = Vector3.up; // 회전 축 (Y축 기본)

    void Update()
    {
        transform.Rotate(rotateAxis * rotateSpeed * Time.deltaTime);
    }
}
