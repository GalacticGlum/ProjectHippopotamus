using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class EnergyBar : MonoBehaviour
{
    private Slider slider;
    private Text sliderText;

	// Use this for initialization
	private void Start ()
	{
	    slider = GetComponent<Slider>();
	    sliderText = transform.Find("Text").GetComponent<Text>();
	}
	
	// Update is called once per frame
	private void Update ()
	{
	    if (World.Current == null || World.Current.Player == null) return;
	    slider.value = World.Current.Player.Power / World.Current.Player.MaximumPower;
	    sliderText.text = string.Format("{0} / {1}", Mathf.RoundToInt(World.Current.Player.Power), Mathf.RoundToInt(World.Current.Player.MaximumPower));
	}
}
