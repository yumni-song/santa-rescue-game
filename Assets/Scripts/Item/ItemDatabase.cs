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

    private bool hasSpawnedItems = false; // 이미 생성했는지 체크

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // 이미 생성했으면 리턴
        if (hasSpawnedItems) return;

        SpawnFieldItems();
        hasSpawnedItems = true;
    }

    private void SpawnFieldItems()
    {
        for (int i = 0; i < pos.Length; i++)
        {
            GameObject go = Instantiate(fieldItemPrefab, pos[i], Quaternion.identity);
            Item randomItem = itemDB[Random.Range(2, 4)];
            Debug.Log($"위치 {i}에 생성된 아이템: {randomItem.itemName}");
            go.GetComponent<FieldItems>().SetItem(randomItem);
        }
    }
}