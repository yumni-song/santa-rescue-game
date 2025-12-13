using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    [SerializeField]
    private List<Item> items = new List<Item>();
    private int SlotCnt = 5;

    public bool AddItem(Item _item)
    {
        if(items.Count < SlotCnt)
        {
            items.Add(_item);
            Debug.Log($"아이템 추가: {_item.itemName}, 현재 개수: {items.Count}");

            if (onChangeItem!=null)
                onChangeItem.Invoke();
            return true;
        }
        Debug.Log("인벤토리가 가득 찼습니다!");
        return false;
    }

    // items에 접근할 수 있는 public 메서드
    public List<Item> GetItems()
    {
        return items;
    }

    // 아이템 제거 메서드
    public void RemoveItem(int _index)
    {
        if (_index >= 0 && _index < items.Count)
        {
            Debug.Log($"아이템 제거: {items[_index].itemName}");
            items.RemoveAt(_index);

            if (onChangeItem != null)
                onChangeItem.Invoke();
        }
        else
        {
            Debug.LogError($"잘못된 인덱스: {_index}");
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("FieldItem"))
        {
            FieldItems fieldItems = collision.GetComponent<FieldItems>();
            Item item = fieldItems.GetItem();

            // Present 타입인 경우 HP 회복
            if (item.itemType == ItemType.Present)
            {
                PlayerHealth playerHealth = GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.Heal(1);
                    Debug.Log($"Present 획득! HP +1");
                }
                else
                {
                    Debug.LogError("PlayerHealth 컴포넌트를 찾을 수 없습니다!");
                }

                // 아이템 파괴
                fieldItems.DestroyItem();
            }
            // 일반 아이템인 경우 인벤토리에 추가
            else
            {
                if (AddItem(item))
                {
                    fieldItems.DestroyItem();
                }
            }
        }
    }
}
