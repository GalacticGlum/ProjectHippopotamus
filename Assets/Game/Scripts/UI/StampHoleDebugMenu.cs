using UnityEngine;
using UnityEngine.UI;

public class StampHoleDebugMenu : MonoBehaviour
{
    [SerializeField]
    private InputField inputField;

    public void StampHole()
    {
        if (inputField == null || string.IsNullOrEmpty(inputField.text)) return;
        string[] coords = inputField.text.Split(',');
        int x = int.Parse(coords[0]);
        int y = int.Parse(coords[1]);

        TerrainUtilities.GenerateCircle(10, World.Current.WorldData, new Vector2i(x, y), TileType.Empty);
    }
}