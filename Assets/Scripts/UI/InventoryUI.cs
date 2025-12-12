using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;

    public GameObject inventoryPanel;
    public Slot[] slots;
    public Transform slotHolder;

    // Start is called before the first frame update
    void Start()
    {
        inven = Inventory.instance;

        if (inven == null)
        {
            Debug.LogError("Inventory instance가 null입니다!");
            return;
        }

        slots = slotHolder.GetComponentsInChildren<Slot>();
        Debug.Log($"찾은 슬롯 개수: {slots.Length}");

        inven.onChangeItem += RedrawSlotUI;
        Debug.Log("InventoryUI 초기화 완료");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RedrawSlotUI()
    {
        Debug.Log("RedrawSlotUI 호출됨!");

        // 인벤토리 패널 활성화 확인
        if (!inventoryPanel.activeSelf)
        {
            Debug.LogWarning("InventoryPanel이 비활성화되어 있습니다!");
        }

        for (int i =0; i< slots.Length; i++)
        {
            slots[i].RemoveSlot();
        }
        Debug.Log($"인벤토리 아이템 개수: {inven.GetItems().Count}");

        for (int i=0; i < inven.GetItems().Count; i++)
        {
            Debug.Log($"슬롯 {i}에 아이템 추가: {inven.GetItems()[i].itemName}");
            slots[i].item = inven.GetItems()[i];
            slots[i].UpdateSlotUI();
        }
    }
}
