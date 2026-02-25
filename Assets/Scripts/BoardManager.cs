using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;
    public List<Tile> allTiles = new List<Tile>();
    private void Awake()
    {
        Instance = this;
    }
    public bool IsBlocked(Tile tile)
    {
        foreach (var other in allTiles)
        {
            if (other == tile) continue;
            if (other.layer > tile.layer)
            {
                if (other.Bounds.Intersects(tile.Bounds))
                {
                    return true;
                }
            }
        }
        return false;
    }
}