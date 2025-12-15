using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems : MonoBehaviour
{
    public Item item;
    public GameObject spawnedModel;

    [Header("아이템 이펙트 설정")]
    [Tooltip("아이템 주변에 표시될 빛나는 이펙트 프리팹")]
    public GameObject itemEffectPrefab;

    [Tooltip("이펙트를 아이템보다 얼마나 크게 할지")]
    public float effectScale = 1.5f;

    [Tooltip("이펙트의 Y축 오프셋 (아이템 위/아래로 조정)")]
    public float effectYOffset = 0f;

    private GameObject spawnedEffect;

    [Header("회전 설정")]
    [Tooltip("아이템을 회전시킬지 여부")]
    public bool rotateItem = true;
    public float rotationSpeed = 50f;
    public Vector3 rotationAxis = Vector3.up;

    [Header("상하 움직임 설정")]
    [Tooltip("아이템을 위아래로 움직일지 여부")]
    public bool floatItem = true;
    public float floatAmplitude = 0.3f;  // 상하 움직임 범위
    public float floatSpeed = 2f;        // 상하 움직임 속도

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // 회전 효과
        if (rotateItem && spawnedModel != null)
        {
            spawnedModel.transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
        }

        // 상하 움직임 효과
        if (floatItem)
        {
            float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

    public void SetItem(Item _item)
    {
        // 기존 모델이 있다면 제거
        if (spawnedModel != null)
        {
            Destroy(spawnedModel);
        }

        // 기존 이펙트가 있다면 제거
        if (spawnedEffect != null)
        {
            Destroy(spawnedEffect);
        }

        item = _item;
        item.itemName = _item.itemName;
        item.itemIcon = _item.itemIcon;
        item.itemModel = _item.itemModel;
        item.itemType = _item.itemType;
        item.efts = _item.efts;

        // 아이템 모델 생성
        spawnedModel = Instantiate(item.itemModel, transform);
        spawnedModel.transform.localPosition = Vector3.zero;
        spawnedModel.transform.localRotation = Quaternion.identity;

        // 이펙트 생성
        CreateEffect();
    }

    void CreateEffect()
    {
        if (itemEffectPrefab == null) return;

        // 이펙트 생성 (아이템의 자식으로)
        Vector3 effectPosition = new Vector3(0, effectYOffset, 0);
        spawnedEffect = Instantiate(itemEffectPrefab, transform);
        spawnedEffect.transform.localPosition = effectPosition;
        spawnedEffect.transform.localRotation = Quaternion.identity;
        spawnedEffect.transform.localScale = Vector3.one * effectScale;

        Debug.Log($"아이템 이펙트 생성: {item.itemName}");
    }

    public Item GetItem()
    {
        return item;
    }

    public void DestroyItem()
    {
        // 아이템과 이펙트 모두 제거
        Destroy(gameObject);
    }
}