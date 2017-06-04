using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LoadingTextUpdater : MonoBehaviour
{
    private Text loadingText;

    private void Start()
    {
        loadingText = GetComponent<Text>();
    }

	// Update is called once per frame
	private void Update ()
	{
	    //loadingText.text = string.Format("{0}% Loaded...", WorldController.Instance.PreloadPercentage);
	}
}
