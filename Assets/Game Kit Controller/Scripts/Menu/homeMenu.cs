using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class homeMenu : MonoBehaviour
{
	public bool useInitialUIElementSelected;
	public GameObject initialUIElementSelected;

	void Start ()
	{
		if (useInitialUIElementSelected) {
			EventSystem.current.SetSelectedGameObject (null);
			EventSystem.current.SetSelectedGameObject (initialUIElementSelected);
		}

		AudioListener.pause = false;

		Time.timeScale = 1;
	}

	public void confirmExit ()
	{
		Application.Quit ();
	}

	public void loadScene (int sceneNumber)
	{
		SceneManager.LoadScene (sceneNumber);
	}

}