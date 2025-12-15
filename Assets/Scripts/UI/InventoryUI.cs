using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;

    public GameObject inventoryPanel;
    public Slot[] slots;
    public Transform slotHolder;

    private int selectedSlotIndex = 0; // 현재 선택된 슬롯

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitializeInventoryUI());
    }

    IEnumerator InitializeInventoryUI()
    {
        // Inventory가 초기화될 때까지 대기
        while (Inventory.instance == null)
        {
            yield return null;
        }

        inven = Inventory.instance;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        Debug.Log($"찾은 슬롯 개수: {slots.Length}");

        inven.onChangeItem -= RedrawSlotUI;
        inven.onChangeItem += RedrawSlotUI;

        // 씬 로드 직후, 현재 인벤토리 데이터를 UI에 바로 반영
        if (slots.Length > 0)
        {
            RedrawSlotUI();
        }

        Debug.Log("InventoryUI 초기화 완료 및 UI 갱신됨");
    }


    // Update is called once per frame
    void Update()
    {
        // 마우스 스크롤로 슬롯 선택
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f) // 스크롤 위로
        {
            SelectPreviousSlot();
        }
        else if (scroll < 0f) // 스크롤 아래로
        {
            SelectNextSlot();
        }

        // 좌클릭으로 아이템 사용
        if (Input.GetMouseButtonDown(0)) // 0 = 좌클릭
        {
            UseSelectedItem();
        }
    }

    void SelectSlot(int index)
    {
        // 유효하지 않은 인덱스 체크
        if (index < 0 || index >= slots.Length)
        {
            Debug.LogWarning($"잘못된 슬롯 인덱스: {index}");
            return;
        }

        Debug.Log($"SelectSlot 호출: 이전={selectedSlotIndex}, 새로운={index}");

        // 모든 슬롯 선택 해제
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
            {
                slots[i].Deselect();
            }
        }

        // 새 슬롯 선택
        selectedSlotIndex = index;
        
        if (slots[selectedSlotIndex] != null)
        {
            slots[selectedSlotIndex].Select();
            Debug.Log($"슬롯 {selectedSlotIndex} 선택됨");
        }
        else
        {
            Debug.LogError($"슬롯 {selectedSlotIndex}이 null입니다!");
        }
    }

    void SelectNextSlot()
    {
        int newIndex = selectedSlotIndex + 1;
        if (newIndex >= slots.Length)
        {
            newIndex = 0; // 마지막에서 처음으로
        }
        Debug.Log($"다음 슬롯으로: {selectedSlotIndex} → {newIndex}");
        SelectSlot(newIndex);
    }

    void SelectPreviousSlot()
    {
        int newIndex = selectedSlotIndex - 1;
        if (newIndex < 0)
        {
            newIndex = slots.Length - 1; // 처음에서 마지막으로
        }
        Debug.Log($"이전 슬롯으로: {selectedSlotIndex} → {newIndex}");
        SelectSlot(newIndex);
    }

    void UseSelectedItem()
    {
        if (selectedSlotIndex >= 0 && selectedSlotIndex < slots.Length)
        {
            Slot selectedSlot = slots[selectedSlotIndex];

            if (selectedSlot != null && selectedSlot.item != null)
            {
                bool isUsed = selectedSlot.UseItem();

                if (isUsed)
                {
                    // 아이템 사용 성공 시 인벤토리에서 제거
                    inven.RemoveItem(selectedSlotIndex);
                }
            }
            else
            {
                Debug.Log("선택된 슬롯에 아이템이 없습니다.");
            }
        }
    }


    void RedrawSlotUI()
    {
        Debug.Log("RedrawSlotUI 호출됨!");

        // 모든 슬롯 초기화
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveSlot();
        }

        Debug.Log($"인벤토리 아이템 개수: {inven.GetItems().Count}");

        // 아이템이 있는 슬롯만 업데이트
        for (int i = 0; i < inven.GetItems().Count; i++)
        {
            Debug.Log($"슬롯 {i}에 아이템 추가: {inven.GetItems()[i].itemName}");
            slots[i].item = inven.GetItems()[i];
            slots[i].slotnum = i;
            slots[i].UpdateSlotUI();
        }

        // 선택된 슬롯이 범위를 벗어나면 조정
        if (selectedSlotIndex >= slots.Length)
        {
            selectedSlotIndex = slots.Length - 1;
        }

        if (selectedSlotIndex < 0)
        {
            selectedSlotIndex = 0;
        }

        // 슬롯 선택 다시 적용
        SelectSlot(selectedSlotIndex);
    }
}