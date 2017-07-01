using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public static class SpriteManager 
{
    public static Texture2D MissingTexture { get; private set; }
    private static Dictionary<string, Sprite> sprites;
    private static Dictionary<string, Reroute> reroutes;

    public static void Initialize()
    {
        MissingTexture = new Texture2D(32, 32, TextureFormat.ARGB32, false);
        Color32[] pixels = MissingTexture.GetPixels32();
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = new Color32(255, 0, 255, 255);
        }

        MissingTexture.SetPixels32(pixels);
        MissingTexture.Apply();

        reroutes = new Dictionary<string, Reroute>();
        LoadSprites();
    }

    private static void LoadSprites()
    {
        sprites = new Dictionary<string, Sprite>();
        string filePath = Path.Combine(Application.streamingAssetsPath, "Images");

        LoadSprites(filePath);
    }

    private static void LoadSprites(string filePath)
    {
        string[] directories = Directory.GetDirectories(filePath);
        foreach (string directory in directories)
        {
            LoadSprites(directory);
        }

        string[] files = Directory.GetFiles(filePath);
        string spriteCategory = new DirectoryInfo(filePath).Name;
        reroutes.Add(spriteCategory, new Reroute(filePath));

        foreach (string file in files)
        {
            LoadImage(spriteCategory, file);

        }
    }

    private static void LoadImage(string spriteCategory, string filePath)
    {
        if (filePath.Contains(".xml") || filePath.Contains(".meta") || filePath.Contains(".db"))
        {
            return;
        }

        // Load the file into a texture
        byte[] imageBytes = File.ReadAllBytes(filePath);
        Texture2D imageTexture = new Texture2D(2, 2);

        if (!imageTexture.LoadImage(imageBytes)) return;

        string baseSpriteName = Path.GetFileNameWithoutExtension(filePath);
        string xmlPath = Path.Combine(Path.GetDirectoryName(filePath), baseSpriteName + ".xml");

        if (File.Exists(xmlPath))
        {
            XmlUtilities.Read("Sprites", "Sprite", xmlPath, xmlReader => ReadSpriteFromXml(spriteCategory, xmlReader, imageTexture));
        }
        else
        {
            LoadSprite(spriteCategory, baseSpriteName, imageTexture, new Rect(0, 0, imageTexture.width, imageTexture.height), 32, new Vector2(0.5f, 0.5f));
        }
    }

    private static void ReadSpriteFromXml(string spriteCategory, XmlReader reader, Texture2D imageTexture)
    {
        string spriteName = reader.GetAttribute("Name");
        int x = int.Parse(reader.GetAttribute("X"));
        int y = int.Parse(reader.GetAttribute("Y"));
        int width = int.Parse(reader.GetAttribute("Width"));
        int height = int.Parse(reader.GetAttribute("Height"));

        string pivotXAttribute = reader.GetAttribute("PivotX");
        float pivotX;

        if (float.TryParse(pivotXAttribute, out pivotX) == false)
        {
            pivotX = 0.5f;
        }

        string pivotYAttribute = reader.GetAttribute("PivotY");
        float pivotY;

        if (float.TryParse(pivotYAttribute, out pivotY) == false)
        {
            pivotY = 0.5f;
        }

        pivotX = Mathf.Clamp01(pivotX);
        pivotY = Mathf.Clamp01(pivotY);

        y = imageTexture.height - y - height;

        LoadSprite(spriteCategory, spriteName, imageTexture, new Rect(x, y, width, height), int.Parse(reader.GetAttribute("PixelPerUnit")), new Vector2(pivotX, pivotY));
    }

    private static void LoadSprite(string spriteCategory, string spriteName, Texture2D imageTexture, Rect spriteCoordinates, int pixelsPerUnit, Vector2 pivotPoint)
    {
        spriteName = spriteCategory + "/" + spriteName;

        Sprite sprite = Sprite.Create(imageTexture, spriteCoordinates, pivotPoint, pixelsPerUnit);
        sprite.texture.filterMode = FilterMode.Point;
        sprites[spriteName] = sprite;
    }

    public static Sprite Get(string categoryName, string spriteName)
    {
        if (reroutes.ContainsKey(categoryName))
        {
            string absoluteName = spriteName.Split('_')[0];
            string suffix = spriteName.Split('_')[1];
            spriteName = string.Concat(reroutes[categoryName].Get(absoluteName), "_", suffix);
        }

        spriteName = categoryName + "/" + spriteName;
        return sprites.ContainsKey(spriteName) == false ? Sprite.Create(MissingTexture, 
            new Rect(Vector2.zero, new Vector3(32, 32)), 
            new Vector2(0.5f, 0.5f), 32) : sprites[spriteName];
    }
}