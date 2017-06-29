using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class PrototypeContainer<T> : IEnumerable<T> where T : IPrototypable, new()
{
    public Dictionary<string, T>.KeyCollection Keys { get { return prototypes.Keys; } }
    public Dictionary<string, T>.ValueCollection Values { get { return prototypes.Values; } }
    public T this[string type] { get { return Get(type); } }

    public int Count { get { return prototypes.Count; } }

    private readonly Dictionary<string, T> prototypes;
    private readonly string prototypeXmlListTag;
    private readonly string prototypeXmlElementTag;

    public PrototypeContainer()
    {
        prototypes = new Dictionary<string, T>();
    }

    public PrototypeContainer(string prototypeXmlListTag, string prototypeXmlElementTag) : this()
    {
        this.prototypeXmlListTag = prototypeXmlListTag;
        this.prototypeXmlElementTag = prototypeXmlElementTag;
    }

    public void Load(string xmlSourceText)
    {
        if (string.IsNullOrEmpty(prototypeXmlListTag) || string.IsNullOrEmpty(prototypeXmlElementTag)) return;

        XmlReader reader = new XmlTextReader(new StringReader(xmlSourceText));
        if (reader.ReadToDescendant(prototypeXmlListTag))
        {
            if (reader.ReadToDescendant(prototypeXmlElementTag))
            {
                do
                {
                    T prototype = new T();
                    try
                    {
                        prototype.ReadXmlPrototype(reader);
                    }
                    catch (Exception e)
                    {
                        Logger.Log("Engine", string.Format("PrototypeContainer<{0}>::Load: Error loading \"{1}\" prototype.\n{2}", 
                            prototypeXmlListTag, prototypeXmlElementTag, e.Message), LoggerVerbosity.Error);   
                    }

                    Set(prototype);
                }
                while (reader.ReadToNextSibling(prototypeXmlElementTag));
            }
            else
            {
                Logger.Log("Engine", string.Format("PrototypeContainer<{0}>::Load: Could not find any element of name \"{1}\" in the XML definition file.",
                    prototypeXmlListTag, prototypeXmlElementTag), LoggerVerbosity.Warning);
            }
        }
        else
        {
            Logger.Log("Engine", string.Format("PrototypeContainer<{0}>::Load: Could not find element of name \"{0}\" in the XML definition file.",
                prototypeXmlListTag), LoggerVerbosity.Warning);
        }
    }

    public bool Contains(string type)
    {
        return prototypes.ContainsKey(type);
    }

    public void Add(T prototype)
    {
        if (Contains(prototype.Type)) return;
        Set(prototype);
    }

    public T Get(string type)
    {
        return Contains(type) ? prototypes[type] : default(T);
    }

    public void Set(T prototype)
    {
        prototypes[prototype.Type] = prototype;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return prototypes.GetEnumerator();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return prototypes.Values.GetEnumerator();
    }
}
