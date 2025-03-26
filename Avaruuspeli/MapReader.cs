using Raylib_cs;
using System.Numerics;
using TurboMapReader;

namespace Avaruuspeli;

public class MapReader
{
    public Map ConvertTiledMapToMap(TiledMap turboMap)
    {
        // Luo tyhjä kenttä
        Map gameMap = new Map();

        // Muunna tason "ground" tiedot
        TurboMapReader.MapLayer groundLayer = turboMap.GetLayerByName("Tile Layer 1");

        // Palauta null jos ground layeriä ei löydy
        if (groundLayer == null) { return null; }

        // aseta kentän leveys ja korkeus
        gameMap.mapWidth = groundLayer.width;
        gameMap.mapHeight = groundLayer.data.Length / groundLayer.width;

        // Kuinka monta kenttäpalaa tässä tasossa on?
        int howManyTiles = groundLayer.data.Length;
        // Taulukko jossa palat ovat
        int[] groundTiles = groundLayer.data;

        // Luo uusi taso tietojen perusteella
        MapLayer myGroundLayer = new MapLayer(howManyTiles);
        myGroundLayer.name = "Tile Layer 1";

        myGroundLayer.mapTiles = groundTiles;

        // Tallenna taso kenttään
        gameMap.layers[0] = myGroundLayer;

        return gameMap;
    }

    public Map? ReadMapFromFile(string filename)
    {
        // Lataa tiedosto käyttäen TurboMapReaderia
        TurboMapReader.TiledMap mapMadeInTiled = TurboMapReader.MapReader.LoadMapFromFile(filename);

        // Tarkista onnistuiko lataaminen
        if (mapMadeInTiled != null)
        {
            // Muuta Map olioksi ja palauta
            return ConvertTiledMapToMap(mapMadeInTiled);
        }
        else
        {
            // OH NO!
            return null;
        }
    }
}
