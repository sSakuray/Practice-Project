using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public bool isOccupied = false;
    
    public void SetOccupied(bool value)
    {
        isOccupied = value;
        GridRegistry.SetOccupied(transform.position, value);
    }
}
