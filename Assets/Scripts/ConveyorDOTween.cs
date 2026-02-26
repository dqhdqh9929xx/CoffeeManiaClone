using UnityEngine;
using System.Collections.Generic;

public class ConveyorManager : MonoBehaviour
{
    [Header("Path")]
    public Transform[] pathPoints;

    [Header("Timing")]
    public float moveDuration = 1f;
    public float spawnInterval = 0.1f;

    [Header("Prefabs")]
    public GameObject foodPrefab1;
    public GameObject foodPrefab2;
    public GameObject foodPrefab3;
    public GameObject foodPrefab4;

    [HideInInspector]
    public List<ConveyorItem> CurrentItemsList = new List<ConveyorItem>();

    public List<int> ListItemsLevel = new List<int>();

    private int currentLevelIndex = 0;
    public int MatchIndex = 0;

    void Awake()
    {
        GenerateLevelData();
    }

    void Start()
    {
        InvokeRepeating(nameof(SpawnFood), 0f, spawnInterval);
    }

    void GenerateLevelData()
    {
        ListItemsLevel.Clear();

        for (int i = 0; i < 16; i++)
        {
            int randomValue = Random.Range(0, 4);
            ListItemsLevel.Add(randomValue);
        }
    }

    void SpawnFood()
    {
        if (currentLevelIndex >= ListItemsLevel.Count)
            return;

        if (CurrentItemsList.Count >= pathPoints.Length)
            return;

        int slotIndex = pathPoints.Length - 1 - CurrentItemsList.Count;

        int foodType = ListItemsLevel[currentLevelIndex];
        GameObject prefab = GetPrefabByType(foodType);

        GameObject food = Instantiate(prefab, pathPoints[0].position, Quaternion.identity);

        int sortingOrder = 20 - currentLevelIndex;
        foreach (SpriteRenderer sr in food.GetComponentsInChildren<SpriteRenderer>(true))
        {
            sr.sortingOrder = sortingOrder;
        }

        ConveyorItem item = food.GetComponent<ConveyorItem>();

        item.Init(
            slotIndex,
            pathPoints,
            moveDuration,
            foodType
        );

        CurrentItemsList.Add(item);

        currentLevelIndex++;
    }

    GameObject GetPrefabByType(int type)
    {
        switch (type)
        {
            case 0: return foodPrefab1;
            case 1: return foodPrefab2;
            case 2: return foodPrefab3;
            case 3: return foodPrefab4;
            default: return foodPrefab1;
        }
    }

    public void ShiftItemsForward()
    {
        for (int i = 0; i < CurrentItemsList.Count; i++)
        {
            int newSlotIndex = pathPoints.Length - 1 - i;

            ConveyorItem item = CurrentItemsList[i];

            item.MoveToSlot(newSlotIndex);
        }
    }
}