using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public void NewGame()
	{
		Debug.Log("A new game has been created.");
		SceneManager.LoadScene("Space Station");
	}
	public void LoadGame()
	{
		Debug.Log("The previous game has been loaded.");
		SceneManager.LoadScene("AI Showoff");
	}
	public void OptionsMenu()
	{
		Debug.Log("The options menu has been opened.");
		SceneManager.LoadScene("Options");
	}
    public void QuitGame()
	{
		Debug.Log("The game has quit.");
        Application.Quit();
	}
}

