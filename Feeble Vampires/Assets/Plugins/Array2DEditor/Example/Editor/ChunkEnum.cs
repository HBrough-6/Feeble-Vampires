using UnityEngine;

namespace Array2DEditor
{
    [System.Serializable]
    public class ChunkEnum : Array2D<TileType>
    {
        [SerializeField]
        CellRowTileType[] cells = new CellRowTileType[Consts.defaultGridSize];

        protected override CellRow<TileType> GetCellRow(int idx)
        {
            return cells[idx];
        }
    }

    [System.Serializable]
    public class CellRowTileType : CellRow<TileType> { }
}