using Raylib_cs;
using System.Numerics;

namespace Avaruuspeli;

/// <summary>
/// Draws and handles Tiled maps
/// </summary>
public class Map
{
    public int mapWidth;
    public int mapHeight;
    public MapLayer[] layers;
    public int tilesPerRow = 12;
    int tileSize = 16;

    public Map()
    {
        mapWidth = 1;
        mapHeight = 1;
        layers = new MapLayer[1];
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i] = new MapLayer(mapWidth * mapHeight);
        }
    }

    /// <summary>
    /// Checks if given layername is in the layers list
    /// </summary>
    /// <param name="layerName"></param>
    /// <returns>A layer</returns>
    public MapLayer GetLayer(string layerName)
    {
        for (int i = 0; i < layers.Length; i++)
        {
            if (layers[i].name == layerName)
            {
                return layers[i];
            }
        }
        Console.WriteLine($"Error: No layer with name: {layerName}");
        return null;
    }

    /// <summary>
    /// Draws the tilemap
    /// </summary>
    /// <param name="mapImage"></param>
    public void Draw(Texture2D mapImage)
    {
        MapLayer groundLayer = GetLayer("Tile Layer 1");
        int[]mapTiles = groundLayer.mapTiles;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                // Count the index
                int tileId = mapTiles[x + y * mapWidth];
                int spriteId = tileId - 1;

                // Set a rectangle for the sprites position in the image
                Rectangle imageRect = new Rectangle(GetSpritePosition(spriteId, tilesPerRow), tileSize, tileSize);

                // Set the drawing position
                Vector2 spawnPosition = new Vector2(x * 16, y * 16 - 800);

                // Draw the image
                Raylib.DrawTextureRec(mapImage, imageRect, spawnPosition, Color.White);
            }
        }
    }

    /// <summary>
    /// Counts sprites position based on its index
    /// </summary>
    /// <param name="spriteIndex"></param>
    /// <param name="spritesPerRow"></param>
    /// <returns>A position as a Vector2</returns>
    public Vector2 GetSpritePosition(int spriteIndex, int spritesPerRow)
    {
        float spritePixelX = (spriteIndex % spritesPerRow) * tileSize;
        float spritePixelY = (int)(spriteIndex / spritesPerRow) * tileSize;
        return new Vector2(spritePixelX, spritePixelY);
    }
}
