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

public class PlayerTargetSysManager : MonoBehaviour 
{
	private GameObject					  m_TargetedMob;
	private GameObject					  m_LastTargetedMob;
	private Health						  m_pTargetHealth;
	private bool						  m_TargetedInit;
	private GameObject					  m_Player;
	private PlayerMsgSystemManager		  m_pPlayerMsgSystemManager;
	
	public static PlayerTargetSysManager  Instance;  //Since we use dontdestroyonload <-- we MUST have ONLY ONE instance
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		m_pTargetHealth = null;
		m_TargetedMob   = null;
		m_TargetedInit  = false;
		
		m_pPlayerMsgSystemManager = ManagersTable.s_GetPlayerMsgSysManager();
	}
	
	
	
	/// <summary>
	/// Gets the name of the target.
	/// </summary>
	/// <returns>
	/// The target name.
	/// </returns>
	public string 		GetTargetName	()
	{
		if(null != m_TargetedMob)
		{
			return m_TargetedMob.GetComponent<InstanceTargeted>().m_SelfTargetName;
		}
		else
		{
			return " ";
		}
	}
	
	/// <summary>
	/// Sets the target.
	/// </summary>
	/// <param name='pi_GameObject'>
	/// Pi_ game object.
	/// </param>
	public void 		SetTarget		(GameObject pi_GameObject)
	{
		if(null == pi_GameObject)
		{
			if(null != m_LastTargetedMob)
			{
				m_LastTargetedMob.GetComponent<InstanceTargeted>().RestoreContour();
			}
			
			m_LastTargetedMob = null;
		}
		else
		{
			string l_MobName = pi_GameObject.GetComponent<InstanceTargeted>().m_SelfTargetName;
			m_pPlayerMsgSystemManager.AddMsgToLog("Cible choisie: " + l_MobName, PlayerMsgSystemManager.MsgType.MSG_DMG_FROM_PLAYER);
		}
		
		m_TargetedMob  = pi_GameObject;
	}
	
	/// <summary>
	/// Gets the target.
	/// </summary>
	/// <returns>
	/// The target.
	/// </returns>
	public GameObject 	GetTarget		()
	{
		return m_TargetedMob;
	}
	
	
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void 	Update()
	{
		if(null != m_TargetedMob)
		{
			if(!m_TargetedInit)
			{
				m_pTargetHealth   = (Health)m_TargetedMob.gameObject.GetComponent<Health>();
				m_LastTargetedMob = m_TargetedMob;
				m_TargetedMob.GetComponent<InstanceTargeted>().CreateTargetContour();
				m_TargetedInit    = true;
			}
			else if(m_TargetedInit && m_TargetedMob != m_LastTargetedMob && null != m_LastTargetedMob)
			{
				m_LastTargetedMob.GetComponent<InstanceTargeted>().RestoreContour();
				m_TargetedInit = false;
			}
			else if(m_TargetedInit && m_TargetedMob != m_LastTargetedMob && null == m_LastTargetedMob)
			{
				m_TargetedInit = false;
			}
		}
	}
	
	
	
	
	
	/// <summary>
	/// Gets the target current health.
	/// </summary>
	/// <returns>
	/// The target current health.
	/// </returns>
	public float 		GetTargetCurrentHealth			()
	{
		if(null != m_TargetedMob && m_TargetedInit)
		{
			return m_pTargetHealth.CurrentHealth;
		}
		
		return 0f; //No target was assigned
	}
	
	/// <summary>
	/// Substracts the target current health.
	/// </summary>	
	public void 		SubstractTargetCurrentHealth	(float dommage)
	{
		if(null != m_TargetedMob && m_TargetedInit)
		{
			m_pTargetHealth.CurrentHealth = m_pTargetHealth.CurrentHealth - dommage;
		}		
	}
	
	/// <summary>
	/// Gets the target max health.
	/// </summary>
	/// <returns>
	/// The target max health.
	/// </returns>
	public float 		GetTargetMaxHealth				()
	{
		if(null != m_TargetedMob && m_TargetedInit)
		{
			return m_pTargetHealth.MaxHealth;
		}
		
		return 0f; //No target was assigned
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