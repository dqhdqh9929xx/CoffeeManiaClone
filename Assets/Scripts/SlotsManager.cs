using System.Collections.Generic;
using UnityEngine;

public class SlotsManager : MonoBehaviour
{
    public static SlotsManager Instance;

    [Header("Slot Settings")]
    public int maxSlots = 4;
    public Transform[] slotPositions;
    private List<Tile> currentTiles = new List<Tile>();
    private void Awake()
    {
        Instance = this;
    }
    public bool HasEmptySlot()
    {
        return currentTiles.Count < maxSlots;
    }
    public void AddTile(Tile tile)
    {
        if (!HasEmptySlot())
        {
            Debug.Log("Slot đã đầy!");
            return;
        }
        currentTiles.Add(tile);

        int index = currentTiles.Count - 1;

        if (index < slotPositions.Length)
        {
            tile.MoveToSlot(slotPositions[index].position);
        }
    }
    public void RemoveTile(Tile tile)
    {
        if (currentTiles.Contains(tile))
        {
            currentTiles.Remove(tile);
        }
    }
}