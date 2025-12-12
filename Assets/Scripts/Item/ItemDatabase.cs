using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;

    public List<Item> itemDB = new List<Item>();
    public GameObject fieldItemPrefab;
    public Vector3[] pos;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < pos.Length; i++)
        {
            GameObject go = Instantiate(fieldItemPrefab, pos[i], Quaternion.identity);
            Item randomItem = itemDB[Random.Range(2, 4)];
            Debug.Log($"위치 {i}에 생성된 아이템: {randomItem.itemName}"); // 디버그 로그
            go.GetComponent<FieldItems>().SetItem(randomItem);
        }
    }
}
