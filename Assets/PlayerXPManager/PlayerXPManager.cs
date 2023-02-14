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
/// Player health manager.
/// </summary>
public class PlayerXPManager : MonoBehaviour 
{
	
	private int 	 				m_CurrentXP 		= 0;
	private int  					m_CurrentLevel      = 1;
	private int  					m_MaxXPForLvl;
	private PlayerMsgSystemManager 	m_pPlayerMsgSystemManager;

	public static PlayerXPManager   Instance;  //Since we use dontdestroyonload <-- we MUST have ONLY ONE instance
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		m_pPlayerMsgSystemManager = ManagersTable.s_GetPlayerMsgSysManager();
		m_MaxXPForLvl             = 50 * ((m_CurrentLevel + 1) * (m_CurrentLevel + 1));
	}
	
	
	
	/// <summary>
	/// Incs the player X.
	/// </summary>
	/// <param name='pi_XP'>
	/// Pi_ X.
	/// </param>
	public void 	IncPlayerXP			(int 	pi_XP)
	{
		if(pi_XP < 0)
		{
			Debug.LogError("ASSERT FAILURE: Valeur negative impossible!");
		}
		
		m_CurrentXP   += pi_XP;
		m_MaxXPForLvl = 50 * ((m_CurrentLevel + 1) * (m_CurrentLevel + 1));
		while(m_CurrentXP > m_MaxXPForLvl)
		{
			m_CurrentXP = m_CurrentXP - m_MaxXPForLvl; 
			++m_CurrentLevel;
			if(!audio.isPlaying) //lvlup sound
			{
				audio.Play();
			}
			
			m_pPlayerMsgSystemManager.AddMsgToLog("NIVEAU AUGMENTE!", PlayerMsgSystemManager.MsgType.MSG_DIALOG);
			
			m_MaxXPForLvl = 50 * ((m_CurrentLevel + 1) * (m_CurrentLevel + 1));
			
			//Change health according to new level
			Health pPlayerHealth 		= ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<Health>();
			pPlayerHealth.MaxHealth	   += 50;
			pPlayerHealth.CurrentHealth = pPlayerHealth.MaxHealth;
			
			ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<Attribute>().addBonusMana(3);
			pPlayerHealth.CurrentSpirit	 = pPlayerHealth.MaxSpirit;
		}
	}
	
	/// <summary>
	/// Gets the player X.
	/// </summary>
	/// <returns>
	/// The player X.
	/// </returns>
	public int 		GetPlayerXP			()
	{
		return m_CurrentXP;
	}
	
	/// <summary>
	/// Gets the player XP max for lvl.
	/// </summary>
	/// <returns>
	/// The player XP max for lvl.
	/// </returns>
	public int 		GetPlayerXPMaxForLvl()
	{
		return m_MaxXPForLvl;
	}
	
	/// <summary>
	/// Gets the player level.
	/// </summary>
	/// <returns>
	/// The player level.
	/// </returns>
	public int 		GetPlayerLevel		()
	{
		return m_CurrentLevel;
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

