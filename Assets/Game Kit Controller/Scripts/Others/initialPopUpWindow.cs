using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;

public class initialPopUpWindow : EditorWindow
{
	GUISkin guiSkin;

	Texture2D GKCLogo = null;
	Vector2 rect = new Vector2 (550, 690);

	GUIStyle titleStyle = new GUIStyle ();
	GUIStyle style = new GUIStyle ();

	[MenuItem ("Game Kit Controller/Initial Pop Up", false, 100)]
	public static void AboutGKC ()
	{
		GetWindow<initialPopUpWindow> ();
	}

	string messageText = "IMPORTANT: You may notice that the player or AI is crossing " +
	                     "the ground or not playing some animations properly.\n\n" +

	                     "It is not a bug, make sure to import the animation " +
	                     "package from the public repository of the asset.\n\n" +

	                     "It is better explained on the doc, it is a group " +
	                     "of animations used on the action system" +
	                     " examples and the melee combat, just as placeholder, " +
	                     "so you can replace them at any moment, as any anymation" +
	                     "will work properly.\n\n" +

	                     "Import the package, close and open unity " +
	                     "and voila, all configured by it self.\n\n" +

	                     "You can use the alternative link for the animations package if you prefer.\n\n" +

	                     "Remove the object on the scene called 'INITIAL POP UP WINDOW' to disable this message.";

	void OnEnable ()
	{
		GKCLogo = (Texture2D)Resources.Load ("Logo_reworked", typeof(Texture2D));
	}

	public void Init ()
	{
		
	}

	void OnDisable ()
	{
		checkOnCloseWindow ();
	}

	void OnGUI ()
	{
		this.titleContent = new GUIContent ("GKC Initial Info");
		this.minSize = rect;

		EditorGUILayout.Space ();

		EditorGUILayout.Space ();

		EditorGUILayout.Space ();

		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		GUILayout.Label (GKCLogo, GUILayout.MaxHeight (100));
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();

		if (!guiSkin) {
			guiSkin = Resources.Load ("GUI") as GUISkin;
		}
		GUI.skin = guiSkin;

		GUILayout.BeginVertical ("window");

		GUILayout.BeginHorizontal ("box");
		GUILayout.FlexibleSpace ();

		titleStyle.normal.textColor = Color.white;
		titleStyle.fontStyle = FontStyle.Bold;
		titleStyle.fontSize = 15;
		titleStyle.alignment = TextAnchor.MiddleCenter;
		GUILayout.Label ("Game Kit Controller Initial Info", titleStyle);

		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();

		EditorGUILayout.Space ();

		EditorGUILayout.Space ();

		style = new GUIStyle (EditorStyles.helpBox);
		style.richText = true;

		style.fontSize = 14;
		style.fontStyle = FontStyle.Bold;
		GUILayout.BeginHorizontal ();
		EditorGUILayout.HelpBox ("", MessageType.Info);

		EditorGUILayout.LabelField (messageText, style);

		GUILayout.EndHorizontal ();

		EditorGUILayout.Space ();

		if (GUILayout.Button ("Public Repository")) {
			Application.OpenURL ("https://github.com/sr3888/GKC-Public-Repository");
		}

		EditorGUILayout.Space ();

		if (GUILayout.Button ("Alternative Package Google Drive Link")) {
			Application.OpenURL ("https://drive.google.com/file/d/144r3PD_O8wC7ydNl4TsXLBvBF6cbjmh2/view?usp=sharing");
		}

		EditorGUILayout.Space ();

		if (GUILayout.Button ("Tutorial To Use Generic Models")) {
			Application.OpenURL ("https://www.youtube.com/watch?v=XABt9LvzRaY");
		}

		EditorGUILayout.Space ();

		if (GUILayout.Button ("Close")) {
			checkOnCloseWindow ();
			this.Close ();
		}

		GUILayout.EndVertical ();
	}

	void checkOnCloseWindow ()
	{
		openInitialPopUpWindow mainOpenInitialPopUpWindow = FindObjectOfType<openInitialPopUpWindow> ();

		if (mainOpenInitialPopUpWindow != null) {
			mainOpenInitialPopUpWindow.setShowInitialPopWindowEnabledState (false);

			GKC_Utils.updateComponent (mainOpenInitialPopUpWindow);

			GKC_Utils.updateDirtyScene ("Update Initial Pop Up", mainOpenInitialPopUpWindow.gameObject);
		}
	}
}
#endif