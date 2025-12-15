using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public int slotnum;
    public Item item;
    public int itemCount;
    public Image itemIcon;

    [Header("선택 표시")]
    public GameObject selectionBorder;

    private void Awake()
    {
        // itemIcon이 비어있으면 자동으로 찾기
        if (itemIcon == null)
        {
            itemIcon = transform.Find("Item_Image")?.GetComponent<Image>();
            if (itemIcon == null)
            {
                Debug.LogError($"{gameObject.name}: Item_Image를 찾을 수 없습니다!");
            }
        }

        // 초기에는 투명하게 설정
        SetColor(0);

        // 선택 테두리 확인 및 비활성화
        if (selectionBorder != null)
        {
            selectionBorder.SetActive(false);
            Debug.Log($"{gameObject.name}: SelectionBorder 초기화 완료");
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: SelectionBorder가 할당되지 않았습니다!");
        }
    }

    // 이미지 투명도 조절
    private void SetColor(float _alpha)
    {
        if (itemIcon != null)
        {
            Color color = itemIcon.color;
            color.a = _alpha;
            itemIcon.color = color;
        }
    }

    public void UpdateSlotUI()
    {
        if (item != null && itemIcon != null)
        {
            itemIcon.sprite = item.itemIcon;
            itemIcon.gameObject.SetActive(true);
            SetColor(1); // 아이템이 있으면 불투명하게
            Debug.Log($"슬롯 {slotnum} 업데이트: {item.itemName}");
        }
    }

    public void RemoveSlot()
    {
        item = null;
        itemCount = 0;

        if (itemIcon != null)
        {
            itemIcon.gameObject.SetActive(false);
            SetColor(0); // 비우면 투명하게
        }
    }

    // 슬롯 선택 표시
    public void Select()
    {
        if (selectionBorder != null)
        {
            selectionBorder.SetActive(true);
            Debug.Log($"슬롯 {slotnum} 선택됨 - Border 활성화");
        }
        else
        {
            Debug.LogWarning($"슬롯 {slotnum}: SelectionBorder가 null입니다!");
        }
    }

    // 슬롯 선택 해제
    public void Deselect()
    {
        if (selectionBorder != null)
        {
            selectionBorder.SetActive(false);
            Debug.Log($"슬롯 {slotnum} 선택 해제 - Border 비활성화");
        }
    }

    // 아이템 사용
    public bool UseItem()
    {
        if (item == null)
        {
            Debug.LogWarning("사용할 아이템이 없습니다.");
            return false;
        }

        Debug.Log($"{item.itemName} 사용 시도");
        bool isUsed = item.Use();

        if (isUsed)
        {
            Debug.Log($"{item.itemName} 사용 완료!");
        }
        else
        {
            Debug.Log($"{item.itemName} 사용 실패");
        }

        return isUsed;
    }
}