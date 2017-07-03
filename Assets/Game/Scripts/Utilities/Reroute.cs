using System.Collections.Generic;
using System.IO;
using System.Xml;

public class Reroute
{
    public bool Exists { get; private set; }

    private readonly Dictionary<string, string> routes;

    public Reroute(string directory)
    {
        routes = new Dictionary<string, string>();
        string filePath = Path.Combine(directory, "Reroute.xml");
        if (!File.Exists(filePath)) return;

        Exists = true;
        XmlUtilities.Read("Reroutes", "Reroute", filePath, ReadRerouteEntry);
    }

    private void ReadRerouteEntry(XmlReader xmlReader)
    {
        string from = xmlReader.GetAttribute("From");
        string to = xmlReader.GetAttribute("To");
        routes.Add(from, to);
    }

    public string Get(string route)
    {
        return routes.ContainsKey(route) ? routes[route] : route;
    }
}
