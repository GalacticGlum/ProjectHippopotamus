using UnityEngine;

public class LoadingScreen : MonoBehaviour
{	
	// Update is called once per frame
	private void Update ()
    {
        if (WorldController.Instance.HasLoaded)
        {
            gameObject.SetActive(false);
        }
	}
}
