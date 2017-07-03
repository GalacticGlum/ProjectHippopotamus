using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;

public class TerrainLayerProcessor : ITerrainProcessor
{
    private List<TerrainLayer> layers;
    private HashSet<byte>[] layersAtHeight;

    public TerrainLayerProcessor()
    {
        layers = new List<TerrainLayer>();

        string filePath = Path.Combine(Path.Combine(Application.streamingAssetsPath, "Data"), "TerrainLayers.xml");
        XmlUtilities.Read("TerrainLayers", "TerrainLayer", filePath, ReadLayer);

        SortLayers();
    }

    public void Generate(WorldData worldData)
    {
        if (layers.Count == 0) return;

        layersAtHeight = new HashSet<byte>[worldData.Height];
        CalculateLayersAtHeight();

        for (int y = 0; y < worldData.Height; y++)
        {
            for (int x = 0; x < worldData.Width; x++)
            {
                if (worldData.Tiles[x, y] != TileType.NonEmpty) continue;

                TerrainLayer terrainLayer = layers[layersAtHeight[y].ElementAt(Random.Range(0, layersAtHeight[y].Count))];
                TileType tileType = terrainLayer.TileType;
                worldData.Tiles[x, y] = tileType;
            }
        }
    }

    private void CalculateLayersAtHeight()
    {
        for (int i = 0; i < layersAtHeight.Length; ++i)
        {
            layersAtHeight[i] = new HashSet<byte>();
            float heightPercent = i / (float)layersAtHeight.Length;
            for (byte layer = 0; layer < layers.Count; ++layer)
            {
                TerrainLayer info = layers[layer];
                if (info.Top <= heightPercent && info.Bottom >= heightPercent)
                {
                    layersAtHeight[i].Add(layer);
                }
            }
        }
    }

    private void ReadLayer(XmlReader xmlReader)
    {
        TileType tileType = TileType.Parse(xmlReader.GetAttribute("TileType"));
        float top = float.Parse(xmlReader.GetAttribute("Top"));
        float bottom = float.Parse(xmlReader.GetAttribute("Bottom"));

        layers.Add(new TerrainLayer(tileType, top, bottom));
    }

    private void SortLayers()
    {
        layers = layers.OrderBy(layer => layer.Top).ToList();
    }
}