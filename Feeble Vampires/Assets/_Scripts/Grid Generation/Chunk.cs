using Array2DEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "GridChunk", menuName = "ScriptableObjects/GridChunk", order = 1)]
public class Chunk : ScriptableObject
{
    public Array2DBool gridChunk = new Array2DBool();
}
