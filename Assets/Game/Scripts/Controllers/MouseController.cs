using UnityEngine;

public class MouseController
{
	public void Update ()
    {
        if (!Input.GetMouseButtonDown(1)) return;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int x = Mathf.FloorToInt(mousePosition.x + 0.5f);
        int y = Mathf.FloorToInt(mousePosition.y + 0.5f);

    }
}
