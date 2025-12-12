using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Key,
    Present,
    Bomb,
    Cross
}

[System.Serializable]
public class Item
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
    public GameObject itemModel;

    public bool Use()
    {
        return false;
    }
}
