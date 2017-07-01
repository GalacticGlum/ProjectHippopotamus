using System.Xml;

public class BuildableObject : IPrototypable
{
    private string name;
    public string Name
    {
        get { return string.IsNullOrEmpty(name) ? Type : name; }
        set { name = value; }
    }

    public string Type { get; private set; }
    public Tile Tile { get; private set; }

    public void ReadXmlPrototype(XmlReader reader)
    {
        throw new System.NotImplementedException();
    }
}
