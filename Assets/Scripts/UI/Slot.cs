using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item;
    public int itemCount;
    public Image itemIcon;

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
    }

    // 이미지 투명도 조절
    private void SetColor(float _alpha)
    {
        Color color = itemIcon.color;
        color.a = _alpha;
        itemIcon.color = color;
    }

    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.itemIcon;
        itemIcon.gameObject.SetActive(true);
        SetColor(1); // 아이템이 있으면 불투명하게

        Debug.Log($"아이콘 활성화 완료: {itemIcon.gameObject.name}");
    }
    public void RemoveSlot()
    {
        item = null;
        itemIcon.gameObject.SetActive(false);
        SetColor(0); // 비우면 투명하게
    }
}
