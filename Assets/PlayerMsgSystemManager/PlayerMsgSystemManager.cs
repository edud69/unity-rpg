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

public class PlayerMsgSystemManager : MonoBehaviour 
{
	public class MsgPair
	{
		public MsgPair(string    pi_Msg, 
			           MsgType   pi_MsgType)
		{
			m_Msg 		  = pi_Msg;
			m_MessageType = pi_MsgType;
		}
		
		public string  GetMsg    () 		{ return m_Msg; }
		public MsgType GetMsgType()			{ return m_MessageType; }
		
		private string  m_Msg;
		private MsgType m_MessageType;
	}
	
	private List<MsgPair>	m_MsgList;	
	
	public enum MsgType
	{
		MSG_DMG_FROM_PLAYER,
		MSG_DMG_TO_PLAYER,
		MSG_HEAL_OR_MANA_REGEN,
		MSG_ITEMS,
		MSG_BONUS_STATS,
		MSG_DIALOG,
	};
	
		
	private static PlayerMsgSystemManager  Instance; //Since we use dontdestroyonload <-- we MUST have ONLY ONE instance
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	public void Start()
	{
		m_MsgList = new List<MsgPair>();
	}
	
	

	/// <summary>
	/// Clears the message log.
	/// </summary>
	public void 			ClearMsgLog				()
	{
		m_MsgList.Clear();
	}
	
	/// <summary>
	/// Clears the partial message log.
	/// </summary>
	/// <param name='pi_NbEventsToKeep'>
	/// Pi_ nb events to keep.
	/// </param>
	public void 			ClearPartialMsgLog		()
	{
		for(int idx = 0; idx < m_MsgList.Count - 1; ++idx)
		{
			m_MsgList[idx] = m_MsgList[idx + 1];
		}
		
		m_MsgList.RemoveAt(m_MsgList.Count - 1);
	}
	
	/// <summary>
	/// Adds the message to log.
	/// </summary>
	/// <param name='pi_Msg'>
	/// Pi_ message.
	/// </param>
	/// <param name='pi_MsgType'>
	/// Pi_ message type.
	/// </param>
	public void 			AddMsgToLog				(string    pi_Msg, 
			                						 MsgType   pi_MsgType)
	{
		MsgPair l_NewMsg = new MsgPair(pi_Msg, pi_MsgType);
		m_MsgList.Add(l_NewMsg);	
	}
	
	/// <summary>
	/// Gets the last message object.
	/// </summary>
	public MsgPair 			GetLastMsgObject		()
	{
		if(0 == m_MsgList.Count)
		{
			return null;
		}
		
		return m_MsgList[m_MsgList.Count - 1];
	}
	
	/// <summary>
	/// Gets the message object.
	/// </summary>
	/// <returns>
	/// The message object.
	/// </returns>
	/// <param name='pi_Idx'>
	/// Pi_ index.
	/// </param>
	public MsgPair 			GetMsgObject			(int 	   pi_Idx)
	{
		if(pi_Idx < 0 || pi_Idx >= m_MsgList.Count)
		{
			Debug.LogError("ASSERT FAILURE: Index Invalide!");
		}
		
		return m_MsgList[pi_Idx];
	}
	
	/// <summary>
	/// Gets the nb message event.
	/// </summary>
	/// <returns>
	/// The nb message event.
	/// </returns>
	public int 				GetNbMsgEvent			()
	{
		return m_MsgList.Count;
	}
	
	
	
	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
        if(Instance)
            DestroyImmediate(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
	}
}
