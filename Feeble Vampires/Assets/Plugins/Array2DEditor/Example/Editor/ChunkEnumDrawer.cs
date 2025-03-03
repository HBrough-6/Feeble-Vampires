using Array2DEditor;
using UnityEditor;

[CustomPropertyDrawer(typeof(Array2DExampleEnum))]
public class ChunkEnumDrawer : Array2DEnumDrawer<TileType> { }
