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

public class WarpGateToTown : MonoBehaviour 
{

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		this.gameObject.renderer.enabled                                 = false;
		this.gameObject.transform.FindChild("Point light").light.enabled = false;
		audio.mute                                                       = true;
	}
	
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () 
	{
		Vector3 pPos = ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<ControllerCharacter>().transform.position;
		this.transform.position = new Vector3(pPos.x, pPos.y + 1f, pPos.z);
		if(ManagersTable.s_GetPlayerPositionManager().GetPortalUsage())
		{
			transform.position          								     = ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<ControllerCharacter>().transform.position;
			this.gameObject.transform.FindChild("Point light").light.enabled = false;
		 	audio.mute 		                                                 = false;
			gameObject.renderer.enabled                                      = true;
		}
	}
}
