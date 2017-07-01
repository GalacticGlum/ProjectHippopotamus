using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SeedReporter : MonoBehaviour
{
    private Text textComponent;

	// Use this for initialization
	private void Start ()
	{
	    textComponent = GetComponent<Text>();
	}
	
	// Update is called once per frame
	private void Update ()
	{
	    textComponent.text = string.Format("Seed: {0}", World.Current.Seed);
	}
}
