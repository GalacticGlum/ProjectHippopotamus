using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using MoonSharp.Interpreter;
using UnityEngine;

[LuaExposeType]
[MoonSharpUserData]
public struct TileType
{
    public static readonly TileType Empty;
    public static readonly TileType NonEmpty;
    private static readonly Dictionary<byte, TileType> tileTypeIdMap;
    private static readonly Dictionary<string, TileType> tileTypeNameMap;

    public readonly byte Id;
    public readonly string Name;
    public readonly LinkType LinkType;

    private readonly Closure destroyedFunction;

    static TileType()
    {
        tileTypeIdMap = new Dictionary<byte, TileType>();
        tileTypeNameMap = new Dictionary<string, TileType>();

        // Initialize defaults
        Empty = new TileType(0, "Empty", LinkType.None);
        NonEmpty = new TileType(255, "NonEmpty", LinkType.None);
        Add(Empty);
        Add(NonEmpty);

        string filePath = Path.Combine(Path.Combine(Application.streamingAssetsPath, "Data"), "TileTypes.xml");
        XmlUtilities.Read("TileTypes", "TileType", filePath, ReadTileType);
    }

    public TileType(byte id, string name, LinkType linkType, Closure destroyedFunction = null) : this()
    {
        Id = id;
        Name = name;
        LinkType = linkType;
        this.destroyedFunction = destroyedFunction;
    }

    private static void ReadTileType(XmlReader xmlReader)
    {
        byte id = byte.Parse(xmlReader.GetAttribute("Id"));
        string name = xmlReader.GetAttribute("Name");

        LinkType linkType = LinkType.None;
        string linkTypeAttribute = xmlReader.GetAttribute("LinkType");
        if (!string.IsNullOrEmpty(linkTypeAttribute))
        {
            linkType = (LinkType)Enum.Parse(typeof(LinkType), linkTypeAttribute);
        }

        XmlReader subReader = xmlReader.ReadSubtree();
        Closure destroyedFunction = null;
        while (subReader.Read())
        {
            switch (subReader.Name)
            {
                case "OnDestroy":
                    Lua.Parse(subReader.GetAttribute("FilePath"));
                    destroyedFunction = Lua.GetFunction(subReader.GetAttribute("FunctionName"));
                    break;
            }
        }

        Add(new TileType(id, name, linkType, destroyedFunction));
    }

    public static void Add(TileType tileType)
    {
        if (tileTypeIdMap.ContainsKey(tileType.Id) || tileTypeNameMap.ContainsKey(tileType.Name))
        {
            Logger.Log("Engine", string.Format("TileType::Add: Tile Type \"{0}\" (Id={1}) already exists!", tileType.Name, tileType.Id), LoggerVerbosity.Warning);
            return;
        }

        tileTypeIdMap.Add(tileType.Id, tileType);
        tileTypeNameMap.Add(tileType.Name, tileType);
    }

    public static TileType Parse(string name)
    {
        if (tileTypeNameMap.ContainsKey(name)) return tileTypeNameMap[name];

        Logger.Log("Engine", string.Format("TileType::Parse: Tile Type \"{0}\" could not be found!", name), LoggerVerbosity.Warning);
        return Empty;
    }

    public static TileType Parse(byte id)
    {
        if (tileTypeIdMap.ContainsKey(id)) return tileTypeIdMap[id];
        Logger.Log("Engine", string.Format("TileType::Parse: Tile Type with Id={0} could not be found!", id), LoggerVerbosity.Warning);
        return Empty;
    }

    public void OnDestroyed(Tile tile)
    {
        if (destroyedFunction == null) return; 
        Lua.Call(destroyedFunction, tile);
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
        return Id == other.Id && string.Equals(Name, other.Name) && LinkType == other.LinkType;
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
            int hashCode = Id.GetHashCode();
            hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (int) LinkType;
            return hashCode;
        }
    }
}