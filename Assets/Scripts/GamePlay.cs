using UnityEngine;
using System.Collections.Generic;

public class GamePlay : MonoBehaviour
{
    public static GamePlay Instance;
    public ConveyorManager conveyorManager;

    [HideInInspector]
    public List<int> currentTilesGamePlay = new List<int>();

    [Header("Match Settings")]
    public float matchCooldown = 0.3f;
    private float matchTimer = 0f;

    void Awake()
    {
        Instance = this;
    }

    public void AddFoodFromTile(string color, int tileType)
    {
        int foodType = ColorToFoodType(color);
        int count = (tileType == 0) ? 4 : 8;

        for (int i = 0; i < count; i++)
        {
            currentTilesGamePlay.Add(foodType);
        }
    }

    int ColorToFoodType(string color)
    {
        switch (color)
        {
            case "blue": return 0;
            case "pink": return 1;
            case "red": return 2;
            case "yellow": return 3;
            default: return 0;
        }
    }
    

    public void MatchFoodPrefab1() => TryMatch(0);
    public void MatchFoodPrefab2() => TryMatch(1);
    public void MatchFoodPrefab3() => TryMatch(2);
    public void MatchFoodPrefab4() => TryMatch(3);

    void Update()
    {
        if (currentTilesGamePlay.Count == 0) return;
        if (conveyorManager.MatchIndex >= conveyorManager.ListItemsLevel.Count) return;

        matchTimer -= Time.deltaTime;
        if (matchTimer > 0) return;

        int requiredType = conveyorManager.ListItemsLevel[conveyorManager.MatchIndex];

        for (int i = 0; i < currentTilesGamePlay.Count; i++)
        {
            if (currentTilesGamePlay[i] == requiredType)
            {
                currentTilesGamePlay.RemoveAt(i);
                TryMatch(requiredType);
                matchTimer = matchCooldown;
                break;
            }
        }
    }

    void TryMatch(int requiredType)
    {
        conveyorManager.MatchIndex++;

        if (conveyorManager.CurrentItemsList.Count > 0)
        {
            ConveyorItem firstItem = conveyorManager.CurrentItemsList[0];
            conveyorManager.CurrentItemsList.RemoveAt(0);
            Destroy(firstItem.gameObject);
            conveyorManager.ShiftItemsForward();
        }
    }
}