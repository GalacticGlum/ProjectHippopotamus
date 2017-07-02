using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;

internal struct LayerInfo
{
    public TileType tileType;
    public float top;
    public float bottom;

    public LayerInfo(TileType tileType, float top, float bottom)
    {
        this.tileType = tileType;
        this.top = top;
        this.bottom = bottom;
    }
}

public class TerrainLayerProcessor : ITerrainProcessor
{
    private List<LayerInfo> layers;
    private HashSet<byte>[] layersAtHeight;
    public TerrainLayerProcessor()
    {
        layers = new List<LayerInfo>();

        string filePath = Path.Combine(Path.Combine(Application.streamingAssetsPath, "Data"), "TerrainLayers.xml");
        XmlUtilities.Read("TerrainLayers", "TerrainLayer", filePath, ReadLayer);

        SortLayers();
    }

    public void Generate(WorldData worldData)
    {
        if (layers.Count == 0) return;

        layersAtHeight = new HashSet<byte>[worldData.Height];
        CalculateLayersAtHeight();
        //int 
        for (int y = 0; y < worldData.Height; y++)
        {
            for (int x = 0; x < worldData.Width; x++)
            {
                if (worldData.Tiles[x, y] == TileType.NonEmpty)
                {
                    LayerInfo layerInfo = layers[layersAtHeight[y].ElementAt(Random.Range(0, layersAtHeight[y].Count))];
                    TileType tileType = layerInfo.tileType;
                    worldData.Tiles[x, y] = tileType;
                }
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
                LayerInfo info = layers[layer];
                if (info.top <= heightPercent && info.bottom >= heightPercent)
                {
                    layersAtHeight[i].Add(layer);
                }
            }
        }
    }

    private void ReadLayer(XmlReader xmlReader)
    {
        TileType tileType = TileType.Get(xmlReader.GetAttribute("TileType"));
        float top = float.Parse(xmlReader.GetAttribute("Top"));
        float bottom = float.Parse(xmlReader.GetAttribute("Bottom"));

        layers.Add(new LayerInfo(tileType, top, bottom));
    }

    private void SortLayers()
    {
        layers = layers.OrderBy(layer => layer.top).ToList();
    }
}