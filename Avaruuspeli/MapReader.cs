using TurboMapReader;

namespace Avaruuspeli;

public class MapReader
{
    /// <summary>
    /// Converts a TurboReaders map into a Map
    /// </summary>
    /// <param name="turboMap"></param>
    /// <returns>The converted map</returns>
    public Map ConvertTiledMapToMap(TiledMap turboMap)
    {
        // Create an empty map
        Map gameMap = new Map();

        // Get the first default layer
        TurboMapReader.MapLayer groundLayer = turboMap.GetLayerByName("Tile Layer 1");

        // Return null if groundLayer is empty
        if (groundLayer == null) { return null; }

        // Set the levels width and height
        gameMap.mapWidth = groundLayer.width;
        gameMap.mapHeight = groundLayer.data.Length / groundLayer.width;

        // Get the amount of pieces
        int howManyTiles = groundLayer.data.Length;
        // Get all of the pieces values
        int[] groundTiles = groundLayer.data;

        // Create new MapLayer with the collected data
        MapLayer myGroundLayer = new MapLayer(howManyTiles);
        myGroundLayer.name = "Tile Layer 1";

        myGroundLayer.mapTiles = groundTiles;

        // Save the layer to the map
        gameMap.layers[0] = myGroundLayer;

        return gameMap;
    }

    /// <summary>
    /// Read the new Tiled file and converts it to a Map
    /// </summary>
    /// <param name="filename"></param>
    /// <returns>A new Map</returns>
    public Map? ReadMapFromFile(string filename)
    {
        // Download the new file using TurboMapReader
        TurboMapReader.TiledMap mapMadeInTiled = TurboMapReader.MapReader.LoadMapFromFile(filename);

        // Check that did loading succeed
        if (mapMadeInTiled != null)
        {
            // Convert Map to object and return it
            return ConvertTiledMapToMap(mapMadeInTiled);
        }
        else
        {
            // OH NO!
            return null;
        }
    }
}
