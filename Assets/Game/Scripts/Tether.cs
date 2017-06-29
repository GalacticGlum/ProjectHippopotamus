using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Tether : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 3;
        lineRenderer.startWidth = 1;
        lineRenderer.endWidth = 1;
        lineRenderer.useWorldSpace = true;
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, (target.position - transform.position) / 2f);
        lineRenderer.SetPosition(2, target.position);
    }
}
