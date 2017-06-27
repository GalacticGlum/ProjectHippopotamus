using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private void Start()
    {
        Follow(true);
    }

    private void LateUpdate()
    {
        Follow(false);
    }

    private void Follow(bool includeYAxis)
    {
        Vector3 position = target.position;
        if (!includeYAxis)
        {
            position.y = 0;
        }

        transform.position = position + offset;
    }
}
