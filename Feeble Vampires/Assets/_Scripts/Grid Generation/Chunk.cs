using Array2DEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "GridChunk", menuName = "ScriptableObjects/GridChunk", order = 1)]
public class Chunk : ScriptableObject
{

    [SerializeField] private bool[] row0 = new bool[8];
    [SerializeField] private bool[] row1 = new bool[8];
    [SerializeField] private bool[] row2 = new bool[8];
    [SerializeField] private bool[] row3 = new bool[8];
    [SerializeField] private bool[] row4 = new bool[8];
    [SerializeField] private bool[] row5 = new bool[8];
    [SerializeField] private bool[] row6 = new bool[8];
    [SerializeField] private bool[] row7 = new bool[8];

    public Array2DBool gridChunk = new Array2DBool();

    /* public bool[,] AccessChunk
     {
         get
         {
             bool[,] chunk = new bool[8, 8];
             for (int i = 0; i < 8; i++)
             {
                 chunk[0]
             }

             return chunk;
         }
     }*/
}
