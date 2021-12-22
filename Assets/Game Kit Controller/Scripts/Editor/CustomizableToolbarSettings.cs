using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class CustomizableToolbarSettingsData
{
	[SerializeField] private string m_commandName	= null;
	[SerializeField] private string m_buttonName	= null;
	[SerializeField] private Texture	m_image = null;
	[SerializeField] private int m_width = 0;
	[SerializeField] private string m_hint	= null;

	public string	CommandName	{ get { return m_commandName; } }

	public string	ButtonName	{ get { return m_buttonName; } }

	public Texture	Image		{ get { return m_image; } }

	public int		Width		{ get { return m_width; } }

	public string		hint		{ get { return m_hint; } }

	public bool IsValid { get { return !string.IsNullOrEmpty (m_commandName); } }
}

public sealed class CustomizableToolbarSettings : ScriptableObject
{
	[SerializeField] private CustomizableToolbarSettingsData[] m_list = null;

	public int numberOfRows;

	public float windowsHeight = 40;

	public IList<CustomizableToolbarSettingsData> List { get { return m_list; } }
}