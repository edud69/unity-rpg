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

public class ManagersTable : MonoBehaviour 
{
	public   enum WINDOWS_IDS   { INVENTORY_WINDOW = 1,
						          QUEST_WINDOW     = 2,
								  MARKET_WINDOW    = 3,
						         };
	
	
	private static PositionManager   		m_PlayerPositionManager; 
	private static DontDestroyEnnemies		m_EnnemiesManager;
	private static PlayerHealthManager      m_PlayerHealthManager;	
	private static PlayerXPManager			m_PlayerXPManager;
	private static PlayerMsgSystemManager   m_PlayerMsgSystemManager;
	private static PlayerTargetSysManager   m_PlayerTargetSysManager;
	private static PlayerItemsManager       m_PlayerItemsManager;
	private static PlayerAttributeManager   m_PlayerAttributeManager;
	private static DontDestroyTreasures     m_TreasuresManager;
	private static FenetreQuetes					m_Quest;
	private static MAJ_Quetes				m_MAJ_Quete;
	private static GameObject				m_SpellSystem;
	
	private static bool						m_TablesSet = false;
		
	private static ManagersTable  Instance;  //Since we use dontdestroyonload <-- we MUST have ONLY ONE instance
	
	/// <summary>
	/// Sets the ennemies manager.
	/// </summary>
	/// <param name='pi_EnnemiesManager'>
	/// Pi_ ennemies manager.
	/// </param>
	public static void SetEnnemiesManager(DontDestroyEnnemies  pi_EnnemiesManager )
	{
		m_EnnemiesManager = pi_EnnemiesManager;		
	}
	
	
	/// <summary>
	/// Sets the spell system.
	/// </summary>
	/// <param name='pi_SpellSys'>
	/// Pi_ spell sys.
	/// </param>
	public static void SetSpellSystem(GameObject   pi_SpellSys)
	{
		m_SpellSystem = pi_SpellSys;
	}
	
	/// <summary>
	/// Gets the spell system.
	/// </summary>
	/// <returns>
	/// The spell system.
	/// </returns>
	public static GameObject GetSpellSystem()
	{
		if(Application.loadedLevelName != "Level_Dungeon")
		{
			Debug.LogError("ASSERTION FAILURE: Should never call this if not in dungeon!!!!");
			return null;
		}
		
		return m_SpellSystem;
	}
	
	/// <summary>
	/// Gets the attribute manager.
	/// </summary>
	/// <returns>
	/// The attribute manager.
	/// </returns>
	public static PlayerAttributeManager s_GetPlayerAttributeManager()
	{		
		return m_PlayerAttributeManager;
	}
	
	/// <summary>
	/// Sets the treasures manager.
	/// </summary>
	/// <param name='pi_TreasuresManager'>
	/// Pi_ treasures manager.
	/// </param>
	public static void SetTreasuresManager(DontDestroyTreasures  pi_TreasuresManager)
	{
		m_TreasuresManager = pi_TreasuresManager;
	}
	
	/// <summary>
	/// Gets the player position manager.
	/// </summary>
	/// <returns>
	/// The player position manager.
	/// </returns>
	public static PositionManager s_GetPlayerPositionManager()
	{
		return m_PlayerPositionManager;
	}
	
	/// <summary>
	/// S_s the get player message sys manager.
	/// </summary>
	/// <returns>
	/// The get player message sys manager.
	/// </returns>
	public static PlayerMsgSystemManager s_GetPlayerMsgSysManager()
	{
		return m_PlayerMsgSystemManager;
	}
	
	/// <summary>
	/// S_s the get ennemies manager.
	/// </summary>
	/// <returns>
	/// The get ennemies manager.
	/// </returns>
	public static DontDestroyEnnemies s_GetEnnemiesManager()
	{
		return m_EnnemiesManager;
	}
	
	/// <summary>
	/// S_s the get treasures manager.
	/// </summary>
	/// <returns>
	/// The get treasures manager.
	/// </returns>
	public static DontDestroyTreasures s_GetTreasuresManager()
	{
		return m_TreasuresManager;
	}
	
	/// <summary>
	/// S_s the get player XP manager.
	/// </summary>
	/// <returns>
	/// The get player XP manager.
	/// </returns>
	public static PlayerXPManager s_GetPlayerXPManager()
	{
		return m_PlayerXPManager;
	}
	
	/// <summary>
	/// S_s the get player health manager.
	/// </summary>
	/// <returns>
	/// The get player health manager.
	/// </returns>
	public static PlayerHealthManager  s_GetPlayerHealthManager()
	{
		return m_PlayerHealthManager;
	}
	
	/// <summary>
	/// S_s the get player items manager.
	/// </summary>
	/// <returns>
	/// The get player items manager.
	/// </returns>
	public static PlayerItemsManager  s_GetPlayerItemsManager()
	{
		return m_PlayerItemsManager;
	}
	
	public static FenetreQuetes  s_GetQuest()
	{
		return m_Quest;
	}
	
	public static MAJ_Quetes  s_MAJ_Quete()
	{
		return m_MAJ_Quete;
	}
	
	/// <summary>
	/// S_s the get player target sys manager.
	/// </summary>
	/// <returns>
	/// The get player target sys manager.
	/// </returns>
	public static PlayerTargetSysManager  s_GetPlayerTargetSysManager()
	{
		return m_PlayerTargetSysManager;
	}
	
	/// <summary>
	/// Gets the tables state ready.
	/// </summary>
	/// <returns>
	/// The tables state ready.
	/// </returns>
	public static bool GetTablesStateReady()
	{
		return m_TablesSet;
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
			
			m_PlayerAttributeManager = GameObject.Find("PlayerAttributeManager").GetComponent<PlayerAttributeManager>();
			m_PlayerPositionManager  = GameObject.Find("PlayerPositionManager").GetComponent<PositionManager>();
			m_PlayerHealthManager    = GameObject.Find("PlayerHealthManager").GetComponent<PlayerHealthManager>();			
			m_PlayerTargetSysManager = GameObject.Find("PlayerTargetSysManager").GetComponent<PlayerTargetSysManager>();
			m_PlayerMsgSystemManager = GameObject.Find("PlayerMsgSystemManager").GetComponent<PlayerMsgSystemManager>();
			m_PlayerXPManager        = GameObject.Find("PlayerXPManager").GetComponent<PlayerXPManager>();
			m_PlayerItemsManager     = GameObject.Find("PlayerItemsManager").GetComponent<PlayerItemsManager>();
			m_Quest     = GameObject.Find("Quests").GetComponent<FenetreQuetes>();
			m_MAJ_Quete = GameObject.Find("Quests").GetComponent<MAJ_Quetes>();
			
        }
	}
}
