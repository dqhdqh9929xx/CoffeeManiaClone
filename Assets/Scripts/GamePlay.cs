using UnityEngine;
using System.Collections.Generic;

public class GamePlay : MonoBehaviour
{
    [System.Serializable]
    public class FoodEntry
    {
        public int foodType;
        public Tile sourceTile;
    }

    public static GamePlay Instance;
    public ConveyorManager conveyorManager;

    [HideInInspector]
    public List<FoodEntry> currentTilesGamePlay = new List<FoodEntry>();

    [Header("Match Settings")]
    public float matchCooldown = 0.3f;
    private float matchTimer = 0f;

    void Awake()
    {
        Instance = this;
    }

    public void AddFoodFromTile(string color, int tileType, Tile tile)
    {
        int foodType = ColorToFoodType(color);
        int count = (tileType == 0) ? 4 : 8;

        for (int i = 0; i < count; i++)
        {
            currentTilesGamePlay.Add(new FoodEntry { foodType = foodType, sourceTile = tile });
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
    

    void Update()
    {
        if (currentTilesGamePlay.Count == 0) return;
        if (conveyorManager.MatchIndex >= conveyorManager.ListItemsLevel.Count) return;

        matchTimer -= Time.deltaTime;
        if (matchTimer > 0) return;

        int requiredType = conveyorManager.ListItemsLevel[conveyorManager.MatchIndex];

        for (int i = 0; i < currentTilesGamePlay.Count; i++)
        {
            // Safety: skip entries for destroyed tiles
            if (currentTilesGamePlay[i].sourceTile == null)
            {
                currentTilesGamePlay.RemoveAt(i);
                i--;
                continue;
            }

            if (currentTilesGamePlay[i].foodType == requiredType)
            {
                Tile targetTile = currentTilesGamePlay[i].sourceTile;
                currentTilesGamePlay.RemoveAt(i);
                TryMatch(requiredType, targetTile);
                matchTimer = matchCooldown;
                break;
            }
        }
    }

    void TryMatch(int requiredType, Tile targetTile)
    {
        conveyorManager.MatchIndex++;

        if (conveyorManager.CurrentItemsList.Count > 0)
        {
            ConveyorItem firstItem = conveyorManager.CurrentItemsList[0];
            conveyorManager.CurrentItemsList.RemoveAt(0);
            firstItem.MoveToTile(targetTile);
            conveyorManager.ShiftItemsForward();
        }
    }
}