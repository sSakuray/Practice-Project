using System.Collections.Generic;
using UnityEngine;

public static class GridRegistry
{
    private static Dictionary<Vector3, bool> occupiedCells = new Dictionary<Vector3, bool>();

    public static bool IsOccupied(Vector3 position)
    {
        return occupiedCells.ContainsKey(position) && occupiedCells[position];
    }

    public static void SetOccupied(Vector3 position, bool occupied)
    {
        if (occupiedCells.ContainsKey(position))
        {
            occupiedCells[position] = occupied;
        }
        else
        {
            occupiedCells.Add(position, occupied);
        }
    }
}
