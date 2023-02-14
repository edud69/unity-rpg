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

/// <summary>
/// To dungeon loading.
/// </summary>
public class ToDungeonLoading : MonoBehaviour 
{

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		Application.LoadLevelAsync ("Level_Dungeon");
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () 
	{
	
	}
}
