using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class CharacterAttributeBarDisplay : MonoBehaviour
{
    [SerializeField]
    private string attributeName;
    [SerializeField]
    private Text text;
    [SerializeField]
    private string textFormat = "{0} / {1}";
    private Slider slider;

	// Use this for initialization
	private void Start ()
	{
	    slider = GetComponent<Slider>();
	    CharacterAttribute attribute = Player.Current.Attributes.Get(attributeName);
        if (attribute == null) return;
        attribute.ValueChanged += OnValueChanged;
	    OnValueChanged(this, new CharacterAttributeEventArgs(attribute));
	}

    private void OnValueChanged(object sender, CharacterAttributeEventArgs args)
    {
        if (text != null)
        {
            text.text = string.Format(textFormat, args.CharacterAttribute.Value, args.CharacterAttribute.MaximumValue);
        }

        slider.value = args.CharacterAttribute.Value / (float)args.CharacterAttribute.MaximumValue;
    }
}
