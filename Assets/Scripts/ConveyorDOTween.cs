using UnityEngine;
using System.Collections.Generic;

public class ConveyorManager : MonoBehaviour
{
    public Transform[] pathPoints;
    public float duration = 5f;
    public float spawnInterval = 2f;
    public GameObject foodPrefab;

    public List<ConveyorItem> CurrentItemsList = new List<ConveyorItem>();

    private Vector3[] cachedPath;

    void Awake()
    {
        cachedPath = new Vector3[pathPoints.Length];

        for (int i = 0; i < pathPoints.Length; i++)
        {
            cachedPath[i] = new Vector3(
                pathPoints[i].position.x,
                pathPoints[i].position.y,
                0f
            );
        }
    }
    void Start()
    {
        InvokeRepeating(nameof(SpawnFood), 0f, spawnInterval);
    }
    void SpawnFood()
    {
        if (CurrentItemsList.Count >= pathPoints.Length)
            return;
        int slotIndex = pathPoints.Length - 1 - CurrentItemsList.Count;
        Vector3[] subPath = new Vector3[slotIndex + 1];
        for (int i = 0; i <= slotIndex; i++)
        {
            subPath[i] = cachedPath[i];
        }
        GameObject food = Instantiate(foodPrefab, subPath[0], Quaternion.identity);
        ConveyorItem item = food.GetComponent<ConveyorItem>();
        item.Init(subPath, duration, this);
        CurrentItemsList.Add(item);
    }
}