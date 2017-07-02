using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public struct TileType
{
    public static readonly TileType Empty;
    public static readonly TileType NonEmpty;
    private static readonly Dictionary<byte, string> tileTypeIdMap;
    private static readonly Dictionary<string, byte> tileTypeNameMap;

    public readonly byte Id;
    public readonly string Name;

    static TileType()
    {
        tileTypeIdMap = new Dictionary<byte, string>();
        tileTypeNameMap = new Dictionary<string, byte>();

        // Initialize defaults
        Empty = new TileType(0, "Empty");
        NonEmpty = new TileType(255, "NonEmpty");
        Add(Empty);
        Add(NonEmpty);

        string filePath = Path.Combine(Path.Combine(Application.streamingAssetsPath, "Data"), "TileTypes.xml");
        XmlUtilities.Read("TileTypes", "TileType", filePath, ReadTileType);
    }

    public TileType(byte id, string name) : this()
    {
        Id = id;
        Name = name;
    }

    private static void ReadTileType(XmlReader xmlReader)
    {
        byte id = byte.Parse(xmlReader.GetAttribute("Id"));
        string name = xmlReader.GetAttribute("Name");

        Add(new TileType(id, name));
    }

    public static void Add(TileType tileType)
    {
        if (tileTypeIdMap.ContainsKey(tileType.Id) || tileTypeNameMap.ContainsKey(tileType.Name))
        {
            Logger.Log("Engine", string.Format("TileType::Add: Tile Type \"{0}\" (Id={1}) already exists!", tileType.Name, tileType.Id), LoggerVerbosity.Warning);
            return;
        }

        tileTypeIdMap.Add(tileType.Id, tileType.Name);
        tileTypeNameMap.Add(tileType.Name, tileType.Id);
    }

    public static TileType Get(string name)
    {
        if (!tileTypeNameMap.ContainsKey(name))
        {
            Logger.Log("Engine", string.Format("TileType::Get: Tile Type \"{0}\" could not be found!", name), LoggerVerbosity.Warning);
            return Empty;
        }

        byte id = tileTypeNameMap[name];
        return new TileType(id, name);
    }

    public static TileType Get(byte id)
    {
        if (!tileTypeIdMap.ContainsKey(id))
        {
            Logger.Log("Engine", string.Format("TileType::Get: Tile Type with Id={0} could not be found!", id), LoggerVerbosity.Warning);
            return Empty;
        }

        string name = tileTypeIdMap[id];
        return new TileType(id, name);
    }

    public static bool operator ==(TileType a, TileType b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(TileType a, TileType b)
    {
        return !a.Equals(b);
    }

    public bool Equals(TileType other)
    {
        return Id == other.Id && string.Equals(Name, other.Name);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is TileType && Equals((TileType) obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (Id.GetHashCode() * 397) ^ (Name != null ? Name.GetHashCode() : 0);
        }
    }
}