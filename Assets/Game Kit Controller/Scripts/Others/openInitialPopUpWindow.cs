﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class openInitialPopUpWindow : MonoBehaviour
{
	public bool showInitialPopWindowEnabled = true;

	void Start ()
	{
		checkOpenInitialPopUpWindow ();
	}

	void checkOpenInitialPopUpWindow ()
	{
		#if UNITY_EDITOR

		if (showInitialPopWindowEnabled) {
			string relativePath = "Assets/Game Kit Controller/Pop Up Window Folder";

			if (!Directory.Exists (relativePath)) {

				Directory.CreateDirectory (relativePath);

				initialPopUpWindow mainWindow = (initialPopUpWindow)EditorWindow.GetWindow (typeof(initialPopUpWindow));

				mainWindow.Init ();

				UnityEditor.EditorApplication.isPlaying = false;
			}
		}
		#endif
	}

	public void setShowInitialPopWindowEnabledState (bool state)
	{
		showInitialPopWindowEnabled = state;
	}
}
