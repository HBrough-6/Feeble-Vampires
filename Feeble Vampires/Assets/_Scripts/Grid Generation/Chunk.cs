using Array2DEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "GridChunk", menuName = "ScriptableObjects/GridChunk", order = 1)]
public class Chunk : ScriptableObject
{
    public Array2DBool gridChunk = new Array2DBool();

    [Header("0: empty, 1: wall, 2: small object, 3: sigil")]
    public Array2DInt chunkData = new Array2DInt();
}
