using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float speed = 100;

	private void Update ()
	{
	    float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
	    float vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;

	    Camera.main.transform.position = Camera.main.transform.position + new Vector3(horizontal, vertical);
	}
}
