using Raylib_cs;
using System.Numerics;

namespace Avaruuspeli;

public class Map
{
    public int mapWidth;
    public int mapHeight;
    public MapLayer[] layers;
    int tilesPerRow = 12;
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

    public void Draw( Texture2D mapImage)
    {
        MapLayer groundLayer = GetLayer("Tile Layer 1");
        int[]mapTiles = groundLayer.mapTiles;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                int tileId = mapTiles[x + y * mapWidth];
                int spriteId = tileId - 1;

                // Laske suorakulmio
                Rectangle imageRect = new Rectangle(GetSpritePosition(spriteId, tilesPerRow), tileSize, tileSize);

                Vector2 spawnPosition = new Vector2(x * 16, y * 16 - 100);

                Raylib.DrawTextureRec(mapImage, imageRect, spawnPosition, Color.White);
            }
        }
    }

    public Vector2 GetSpritePosition(int spriteIndex, int spritesPerRow)
    {
        float spritePixelX = (spriteIndex % spritesPerRow) * mapWidth;
        float spritePixelY = (int)(spriteIndex / spritesPerRow) * mapHeight;
        return new Vector2(spritePixelX, spritePixelY);
    }
}
