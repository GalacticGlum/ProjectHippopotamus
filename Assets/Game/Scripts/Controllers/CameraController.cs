using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    public Camera Camera { get; private set; }

    private bool isFrozen;

    private const float dampTime = 0.2f;
    private bool lerpToPosition;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        Instance = this;
        Camera = GetComponent<Camera>();
    }

	private void LateUpdate ()
	{
	    if (isFrozen) return;

	    Vector3 targetPosition = World.Current.Player.transform.position + new Vector3(0, 0, -10);
        if (lerpToPosition)
        {
            targetPosition.z = transform.position.z;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, dampTime);

            if (MathHelper.Approximately(transform.position, targetPosition))
            {
                lerpToPosition = false;
            }

	        return;
	    }

	    Camera.main.transform.position = targetPosition;
	}

    public void Freeze(float duration = 0)
    {
        isFrozen = true;
        if (duration > 0)
        {
            Invoke("Unfreeze", duration);
        }
    }

    public void Unfreeze()
    {
        isFrozen = false;
        lerpToPosition = true;
    }
}
