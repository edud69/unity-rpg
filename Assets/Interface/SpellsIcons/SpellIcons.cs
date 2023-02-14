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

public class SpellIcons : MonoBehaviour 
{
	
	public int 							m_SpellNumber = 0;
	public Texture					    m_InactiveSpell;
	private Texture						m_ActiveSpell;
	
	void Start () 
	{
		guiTexture.pixelInset  = new Rect(Screen.width - 70, Screen.height - 50 - 55 * m_SpellNumber, 50, 50);
		m_ActiveSpell = guiTexture.texture;
	}
	
	void OnGUI()
	{
		if(Application.loadedLevelName == "Level_Town")
		{
			guiTexture.texture = m_InactiveSpell;	
		}
		else
		{
			switch(m_SpellNumber)
			{
				case 1: //burning spell
					if(!ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<ControllerCharacter>().GetBurningSpellReady())
					{
						guiTexture.texture = m_InactiveSpell;
					}
					else
					{
						guiTexture.texture = m_ActiveSpell;	
					}
					break;
				case 2: //ice spell
					if(!ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<ControllerCharacter>().GetIceSpellReady())
					{
						guiTexture.texture = m_InactiveSpell;
					}
					else
					{
						guiTexture.texture = m_ActiveSpell;	
					}
					break;		
			    case 5: //WarpGate spell
					if(!ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<ControllerCharacter>().GetWarpGateReady())
					{
						guiTexture.texture = m_InactiveSpell;
					}
					else
					{
						guiTexture.texture = m_ActiveSpell;	
					}
					break;	
					
				default:
					break;
			}
		}
	}
}
