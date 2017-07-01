using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField]
    private int rows;
    [SerializeField]
    private int slots;
    [SerializeField]
    private int paddingLeft;
    [SerializeField]
    private int paddingTop;
    [SerializeField]
    private int slotSize;
    [SerializeField]
    private GameObject slotPrefab;

    private int width;
    private int height;

    private void Start()
    {
        width = slots / rows * (slotSize + paddingLeft) + paddingLeft;
        height = rows * (slotSize + paddingTop) + paddingTop;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

        int columns = slots / rows;
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                GameObject slotInstance = Instantiate(slotPrefab);
                RectTransform slotRectTransform = slotInstance.GetComponent<RectTransform>();
                slotInstance.name = string.Format("Slot_{0}_{1}", x, y);
                slotInstance.transform.SetParent(transform);
                slotRectTransform.localPosition = rectTransform.localPosition + new Vector3(paddingLeft * (x + 1) + slotSize * x, -paddingTop * (y + 1) - slotSize * y);

                slotRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                slotRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);
            }
        }
    }
}
