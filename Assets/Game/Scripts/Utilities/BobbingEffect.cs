using UnityEngine;

public class BobbingEffect : MonoBehaviour
{
    private float strength = 1;
    private float startY;

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, startY + Mathf.Sin(Time.time) * strength, transform.position.z);
    }

    public static void Create(GameObject target, float strength, float startY)
    {
        if (target == null) return;
        BobbingEffect instance = target.AddComponent<BobbingEffect>();
        instance.strength = strength;
        instance.startY = startY;
    }
}