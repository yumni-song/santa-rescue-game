using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FixedItemSpawn
{
    [Tooltip("스폰할 위치 인덱스 (pos 배열의 인덱스)")]
    public int positionIndex;

    [Tooltip("스폰할 아이템 인덱스 (itemDB 배열의 인덱스)")]
    public int itemIndex;
}

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;

    [Header("아이템 데이터베이스")]
    public List<Item> itemDB = new List<Item>();

    [Header("스폰 설정")]
    public GameObject fieldItemPrefab;
    public Vector3[] pos;

    [Header("랜덤 스폰 설정")]
    [Tooltip("랜덤으로 스폰할 아이템의 인덱스 범위 (Min 포함, Max 미포함)")]
    public int randomItemMinIndex = 2;
    public int randomItemMaxIndex = 4;

    [Header("랜덤 위치 특별 아이템 설정")]
    [Tooltip("랜덤 위치 한 곳에만 생성할 특별 아이템의 인덱스 (-1이면 사용 안 함)")]
    public int randomSpecialItemIndex = 0;

    [Tooltip("랜덤 특별 아이템을 생성할지 여부")]
    public bool spawnRandomSpecialItem = true;

    [Header("고정 위치 아이템 설정")]
    [Tooltip("특정 위치에 고정으로 스폰할 아이템 목록")]
    public List<FixedItemSpawn> fixedSpawns = new List<FixedItemSpawn>();

    private bool hasSpawnedItems = false;

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
        if (hasSpawnedItems) return;

        SpawnFieldItems();
        hasSpawnedItems = true;
    }

    private void SpawnFieldItems()
    {
        if (itemDB.Count == 0)
        {
            Debug.LogWarning("ItemDatabase: itemDB가 비어있습니다!");
            return;
        }

        if (fieldItemPrefab == null)
        {
            Debug.LogWarning("ItemDatabase: fieldItemPrefab이 설정되지 않았습니다!");
            return;
        }

        if (pos.Length == 0)
        {
            Debug.LogWarning("ItemDatabase: 스폰 위치가 설정되지 않았습니다!");
            return;
        }

        // 사용 가능한 위치 인덱스 리스트 생성
        List<int> availablePositions = new List<int>();
        for (int i = 0; i < pos.Length; i++)
        {
            availablePositions.Add(i);
        }

        // 1단계: 고정 위치 아이템 먼저 스폰
        SpawnFixedItems(availablePositions);

        // 2단계: 랜덤 위치 특별 아이템 스폰
        if (spawnRandomSpecialItem && randomSpecialItemIndex >= 0 && randomSpecialItemIndex < itemDB.Count)
        {
            SpawnRandomSpecialItem(availablePositions);
        }

        // 3단계: 나머지 위치에 랜덤 아이템 스폰
        SpawnRandomItems(availablePositions);
    }

    private void SpawnFixedItems(List<int> availablePositions)
    {
        foreach (FixedItemSpawn fixedSpawn in fixedSpawns)
        {
            // 유효성 검사
            if (fixedSpawn.positionIndex < 0 || fixedSpawn.positionIndex >= pos.Length)
            {
                Debug.LogWarning($"잘못된 위치 인덱스: {fixedSpawn.positionIndex}");
                continue;
            }

            if (fixedSpawn.itemIndex < 0 || fixedSpawn.itemIndex >= itemDB.Count)
            {
                Debug.LogWarning($"잘못된 아이템 인덱스: {fixedSpawn.itemIndex}");
                continue;
            }

            // 이미 사용된 위치인지 확인
            if (!availablePositions.Contains(fixedSpawn.positionIndex))
            {
                Debug.LogWarning($"위치 {fixedSpawn.positionIndex}는 이미 사용되었습니다!");
                continue;
            }

            // 해당 위치에 고정 아이템 스폰
            Item itemToSpawn = itemDB[fixedSpawn.itemIndex];
            Vector3 spawnPos = pos[fixedSpawn.positionIndex];

            GameObject go = Instantiate(fieldItemPrefab, spawnPos, Quaternion.identity);
            FieldItems fieldItem = go.GetComponent<FieldItems>();

            if (fieldItem != null)
            {
                fieldItem.SetItem(itemToSpawn);
                availablePositions.Remove(fixedSpawn.positionIndex);
                Debug.Log($"[고정] 위치 {fixedSpawn.positionIndex}에 생성된 아이템: {itemToSpawn.itemName}");
            }
            else
            {
                Debug.LogWarning($"FieldItems 컴포넌트를 찾을 수 없습니다: {go.name}");
            }
        }
    }

    private void SpawnRandomSpecialItem(List<int> availablePositions)
    {
        if (availablePositions.Count == 0)
        {
            Debug.LogWarning("랜덤 특별 아이템을 스폰할 위치가 없습니다!");
            return;
        }

        // 랜덤하게 위치 하나 선택
        int randomIndex = Random.Range(0, availablePositions.Count);
        int selectedPosition = availablePositions[randomIndex];

        // 해당 위치에 특별 아이템 스폰
        Item specialItem = itemDB[randomSpecialItemIndex];
        Vector3 spawnPos = pos[selectedPosition];

        GameObject go = Instantiate(fieldItemPrefab, spawnPos, Quaternion.identity);
        FieldItems fieldItem = go.GetComponent<FieldItems>();

        if (fieldItem != null)
        {
            fieldItem.SetItem(specialItem);
            Debug.Log($"[랜덤 특별] 위치 {selectedPosition}에 생성된 아이템: {specialItem.itemName}");
        }
        else
        {
            Debug.LogWarning($"FieldItems 컴포넌트를 찾을 수 없습니다: {go.name}");
        }

        // 사용한 위치는 리스트에서 제거
        availablePositions.RemoveAt(randomIndex);
    }

    private void SpawnRandomItems(List<int> availablePositions)
    {
        // 남은 모든 위치에 랜덤 아이템 생성
        foreach (int posIndex in availablePositions)
        {
            // 랜덤 아이템 선택
            Item randomItem = itemDB[Random.Range(randomItemMinIndex, randomItemMaxIndex)];

            GameObject go = Instantiate(fieldItemPrefab, pos[posIndex], Quaternion.identity);
            FieldItems fieldItem = go.GetComponent<FieldItems>();

            if (fieldItem != null)
            {
                fieldItem.SetItem(randomItem);
                Debug.Log($"[랜덤] 위치 {posIndex}에 생성된 아이템: {randomItem.itemName}");
            }
            else
            {
                Debug.LogWarning($"FieldItems 컴포넌트를 찾을 수 없습니다: {go.name}");
            }
        }
    }

    /// <summary>
    /// 런타임에 특정 위치에 랜덤 아이템 스폰 (외부 호출용)
    /// </summary>
    public void SpawnRandomItem(Vector3 position)
    {
        if (itemDB.Count == 0)
        {
            Debug.LogWarning("ItemDatabase: itemDB가 비어있습니다!");
            return;
        }

        Item randomItem = itemDB[Random.Range(randomItemMinIndex, randomItemMaxIndex)];
        SpawnItem(randomItem, position);
    }

    /// <summary>
    /// 특정 아이템을 특정 위치에 스폰 (외부 호출용)
    /// </summary>
    public void SpawnItem(Item item, Vector3 position)
    {
        if (fieldItemPrefab == null)
        {
            Debug.LogWarning("ItemDatabase: fieldItemPrefab이 설정되지 않았습니다!");
            return;
        }

        GameObject go = Instantiate(fieldItemPrefab, position, Quaternion.identity);
        FieldItems fieldItem = go.GetComponent<FieldItems>();

        if (fieldItem != null)
        {
            fieldItem.SetItem(item);
        }
    }

    /// <summary>
    /// 특정 아이템 인덱스를 특정 위치에 스폰 (외부 호출용)
    /// </summary>
    public void SpawnItemByIndex(int itemIndex, Vector3 position)
    {
        if (itemIndex < 0 || itemIndex >= itemDB.Count)
        {
            Debug.LogWarning($"잘못된 아이템 인덱스: {itemIndex}");
            return;
        }

        SpawnItem(itemDB[itemIndex], position);
    }
}