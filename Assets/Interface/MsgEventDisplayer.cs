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
using System.Collections.Generic;



public class MsgEventDisplayer : MonoBehaviour 
{
	const int _MAX_MSG_LOG_LINE = 8;
	
	private int		       m_NbEvents = 0;
    PlayerMsgSystemManager m_pPlayerMsgSystemManager;
	
	void 		Start			()
	{
		m_pPlayerMsgSystemManager = ManagersTable.s_GetPlayerMsgSysManager();
	}
		
	/// <summary>
	/// Raises the GU event.
	/// </summary>
	void 		OnGUI			()
	{	
		GUI.Box(new Rect(0, Screen.height - Screen.height/4, 350, Screen.height/4), "");
		m_NbEvents = m_pPlayerMsgSystemManager.GetNbMsgEvent();
		if(_MAX_MSG_LOG_LINE + 1 <= m_NbEvents)
		{
			m_pPlayerMsgSystemManager.ClearPartialMsgLog();
			m_NbEvents = m_pPlayerMsgSystemManager.GetNbMsgEvent();
		}
		
		if(0 != m_NbEvents)
		{
			string l_string;
			for(int idx = 1; idx <= m_NbEvents; ++idx)
			{
				l_string = m_pPlayerMsgSystemManager.GetMsgObject(idx - 1).GetMsg();
			    GUI.Label(new Rect(5,
							       Screen.height - (m_NbEvents - idx)*15 - 20,
					               Screen.width,
								   l_string.Length), 
								   l_string, 
					               CreateGUIStyle(idx - 1));
			}
		}
	}
	
	/// <summary>
	/// Creates the GUI style.
	/// </summary>
	/// <returns>
	/// The GUI style.
	/// </returns>
	GUIStyle 	CreateGUIStyle	(int pi_idx)
	{
		GUIStyle pStyle  = new GUIStyle();
		PlayerMsgSystemManager.MsgType pMsgType = m_pPlayerMsgSystemManager.GetMsgObject(pi_idx).GetMsgType();
		
		
		if(PlayerMsgSystemManager.MsgType.MSG_DIALOG == pMsgType)
		{
			pStyle.normal.textColor = Color.white;
			pStyle.fontStyle = FontStyle.BoldAndItalic;
		}
		else if(PlayerMsgSystemManager.MsgType.MSG_DMG_FROM_PLAYER == pMsgType)
		{
			pStyle.normal.textColor = new Color(1f, 0.48f, 0f, 1f);
			pStyle.fontStyle = FontStyle.Italic;
		}
		else if(PlayerMsgSystemManager.MsgType.MSG_DMG_TO_PLAYER == pMsgType)
		{
			pStyle.normal.textColor = Color.red;
			pStyle.fontStyle = FontStyle.Italic;
		}
		else if(PlayerMsgSystemManager.MsgType.MSG_HEAL_OR_MANA_REGEN == pMsgType)
		{
			pStyle.normal.textColor = Color.green;
			pStyle.fontStyle = FontStyle.Italic;
		}
		else if(PlayerMsgSystemManager.MsgType.MSG_ITEMS == pMsgType)
		{
			pStyle.normal.textColor = Color.yellow;
			pStyle.fontStyle = FontStyle.Italic;
		}
		else if(PlayerMsgSystemManager.MsgType.MSG_BONUS_STATS == pMsgType)
		{
			pStyle.normal.textColor = new Color(1f, 0f, 1f, 1f);
			pStyle.fontStyle = FontStyle.BoldAndItalic;
		}
		
		return pStyle;
	}
}