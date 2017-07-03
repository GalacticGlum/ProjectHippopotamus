public struct TerrainLayer
{
    public readonly TileType TileType;
    public readonly float Top;
    public readonly float Bottom;

    public TerrainLayer(TileType tileType, float top, float bottom)
    {
        TileType = tileType;
        Top = top;
        Bottom = bottom;
    }
}
