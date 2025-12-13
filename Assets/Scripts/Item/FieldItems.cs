using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class FieldItems : MonoBehaviour
{
    public Item item;
    public GameObject spawnedModel;

    public void SetItem(Item _item)
    {

        // 기존 모델이 있다면 제거
        if (spawnedModel != null)
        {
            Destroy(spawnedModel);
        }

        item = _item;

        item.itemName = _item.itemName;
        item.itemIcon = _item.itemIcon;
        item.itemModel = _item.itemModel;
        item.itemType = _item.itemType;
        item.efts = _item.efts;

        spawnedModel = Instantiate(item.itemModel, transform);
        spawnedModel.transform.localPosition = Vector3.zero;
        spawnedModel.transform.localRotation = Quaternion.identity;

    }
    public Item GetItem()
    {
        return item;
    }
    public void DestroyItem()
    {
        Destroy(gameObject);
    }

}
