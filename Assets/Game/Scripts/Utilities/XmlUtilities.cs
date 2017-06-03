using System;
using System.IO;
using System.Xml;

public static class XmlUtilities
{
    /// <summary>
    /// Executes <paramref name="childReadAction"/> on every child of <paramref name="rootElementName"/> that has a name of <paramref name="elementName"/>.
    /// </summary>
    /// <param name="rootElementName"></param>
    /// <param name="elementName"></param>
    /// <param name="filePath"></param>
    /// <param name="childReadAction"></param>
    /// <param name="rootReadAction"></param>
    public static void Read(string rootElementName, string elementName, string filePath, Action<XmlReader> childReadAction, Action<XmlReader> rootReadAction = null)
    {
        using (XmlReader reader = new XmlTextReader(new StreamReader(filePath)))
        {
            if (!reader.ReadToDescendant(rootElementName)) return;
            if (rootReadAction != null)
            {
                rootReadAction.Invoke(reader);
            }

            if (childReadAction == null) return;

            if (!reader.ReadToDescendant(elementName)) return;
            do
            {
                childReadAction(reader);
            }
            while (reader.ReadToNextSibling(elementName));
        }
    }
}

