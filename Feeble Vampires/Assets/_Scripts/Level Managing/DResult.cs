using System.Collections.Generic;
using UnityEngine;

public class DResult
{
    public Vector2Int startPoint;
    public List<Vector2Int> endPoints = new List<Vector2Int>();
    public List<Vector2Int> sigilPoints = new List<Vector2Int>();
    public int[] resultOfBFS;
    public int sigilCount;
}
