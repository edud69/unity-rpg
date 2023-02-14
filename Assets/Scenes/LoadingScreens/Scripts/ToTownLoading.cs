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

public class ToTownLoading : MonoBehaviour 
{

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		Application.LoadLevelAsync ("Level_Town");
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () 
	{
	
	}
}
