using System.Collections.Generic;
using UnityEngine;

public class SlotsManager : MonoBehaviour
{
    public static SlotsManager Instance;

    [Header("Slot Settings")]
    public int maxSlots = 4;
    public Transform[] slotPositions;
    private Tile[] currentSlots; // fixed-size array, null = empty slot

    private void Awake()
    {
        Instance = this;
        currentSlots = new Tile[maxSlots];
    }

    public bool HasEmptySlot()
    {
        for (int i = 0; i < currentSlots.Length; i++)
        {
            if (currentSlots[i] == null) return true;
        }
        return false;
    }

    public void AddTile(Tile tile)
    {
        int emptyIndex = -1;
        for (int i = 0; i < currentSlots.Length; i++)
        {
            if (currentSlots[i] == null)
            {
                emptyIndex = i;
                break;
            }
        }

        if (emptyIndex < 0)
        {
            Debug.Log("Slot đã đầy!");
            return;
        }

        currentSlots[emptyIndex] = tile;

        if (emptyIndex < slotPositions.Length)
        {
            tile.MoveToSlot(slotPositions[emptyIndex].position);
        }
    }

    public void RemoveTile(Tile tile)
    {
        for (int i = 0; i < currentSlots.Length; i++)
        {
            if (currentSlots[i] == tile)
            {
                currentSlots[i] = null; // chỉ xoá slot này, giữ nguyên index các tile khác
                break;
            }
        }
    }
}