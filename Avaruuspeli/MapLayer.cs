namespace Avaruuspeli;

/// <summary>
/// MapLayer that has the layers name and tile index numbers
/// </summary>
public class MapLayer
{
    public string name;
    public int[] mapTiles;
    public MapLayer(int mapSize)
    {
        name = "";
        mapTiles = new int[mapSize];
    }
}
