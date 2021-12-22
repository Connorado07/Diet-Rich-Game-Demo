using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputPanelUISystem : MonoBehaviour
{
	public playerInputPanelSystem mainPlayerInputPanelSystem;

	public GameObject inputPanelGameObject;

	public GameObject screenActionParent;

	public List<screenActionInfo> screenActionInfoList = new List<screenActionInfo> ();


	public void setInputPanelGameObjectActiveState (bool state)
	{
		inputPanelGameObject.SetActive (state);
	}

	public void setMainPlayerInputPanelSystem (playerInputPanelSystem newplayerInputPanelSystem)
	{
		mainPlayerInputPanelSystem = newplayerInputPanelSystem;

		updateComponent ();
	}

	public void searchPlayerInputPanelSystem (playerInputPanelSystem newPlayerInputPanelSystem)
	{
		mainPlayerInputPanelSystem = newPlayerInputPanelSystem;

		if (mainPlayerInputPanelSystem == null) {
			mainPlayerInputPanelSystem = FindObjectOfType<playerInputPanelSystem> ();
		}

		if (mainPlayerInputPanelSystem != null) {
			mainPlayerInputPanelSystem.setInputPanelUISystem (this);
		}

		updateComponent ();
	}

	void updateComponent ()
	{
		GKC_Utils.updateComponent (this);
	}
}
