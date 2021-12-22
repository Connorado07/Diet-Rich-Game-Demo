using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showValuesOnScreenSystem : MonoBehaviour
{
	public bool showValueEnabled = true;

	public GameObject valueTextPanel;
	public Text valueText;

	public bool addExtraString;
	public string extraString;
	public bool addExtraStringAtStart;

	bool panelActive;

	void Start ()
	{
		if (!showValueEnabled) {
			if (valueTextPanel.activeSelf) {
				valueTextPanel.SetActive (false);
			}
		}
	}

	public void updateValueAmount (float newValue)
	{
		if (!showValueEnabled) {
			return;
		}

		if (!panelActive) {
			if (!valueTextPanel.activeSelf) {
				valueTextPanel.SetActive (true);
			}

			panelActive = true;
		}

		string valueString = newValue.ToString ();

		if (addExtraString) {
			if (addExtraStringAtStart) {
				valueString = extraString + valueString;
			} else {
				valueString += extraString;
			}
		}

		valueText.text = valueString;
	}
}
