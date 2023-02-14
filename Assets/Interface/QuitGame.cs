/*
 * Owner	: Mathieu Antonuk-L'Esperance
 *
 * Modification History 
 * --------------------
 *
 * Dated	Version		Who		Description
 * ----------------------------------------------------------
 * 
 */

using UnityEngine;
using System.Collections;

public class QuitGame : MonoBehaviour 
{
	
	private bool     m_LeaveOrStay = false;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
	
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () {
	
	}
	
	/// <summary>
	/// Raises the GU event.
	/// </summary>
	void OnGUI()
	{
		GUIStyle pStyle  = new GUIStyle("Button");
		pStyle.alignment = TextAnchor.MiddleLeft;
		
		if (GUI.Button (new Rect (Screen.width - 30, 15, 22, 20), "X", pStyle)) 
		{
	   		m_LeaveOrStay = true;
		}
		
		
		if(m_LeaveOrStay)
		{
			string l_QuitStr = "Voulez-vous vraiment quitter la partie?";
			GUI.Label(new Rect (Screen.width/2 - l_QuitStr.Length*2, Screen.height/2 - 30, Screen.width,  Screen.height), l_QuitStr);
			
			if (GUI.Button (new Rect (Screen.width/2 + 80, Screen.height/2, 80, 20), "Quitter")) 
			{
		   		Application.Quit();
			}
			
			if (GUI.Button (new Rect (Screen.width/2 - 80, Screen.height/2, 80, 20), "Rester")) 
			{
		   		m_LeaveOrStay = false;
			}
		}
	}
}
