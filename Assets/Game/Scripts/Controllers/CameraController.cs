using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float followDampening = 0.3f;
    private Vector3 currentVelocity;
	
	// Update is called once per frame
	private void LateUpdate ()
	{
	    Camera.main.transform.position = World.Current.Player.CharacterGameObject.transform.position + new Vector3(0, 0, -10);
	}
}
