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
/// Position manager.
/// </summary>
public class PositionManager : MonoBehaviour 
{
	
	private Vector3      m_PlayerTown;
	private Vector3      m_PlayerDungeon;
	
	private Vector3      m_PlayerTownPortal;
	private Vector3	     m_PlayerDungeonPortal;
	
	private bool		 m_PortalUsed = false;
	
	private GameObject m_Player;
	
	private static PositionManager  Instance; //Since we use dontdestroyonload <-- we MUST have ONLY ONE instance
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		m_PlayerTown       = new Vector3(82.67f,  1.39f, -22.89f);
		m_PlayerDungeon    = new Vector3(38.80f, 47.29f,   7.28f);
		m_PlayerTownPortal = new Vector3( 22.6f,   0.4f,    2.2f);
		
		SetPlayer();
	}
	
	/// <summary>
	/// Gets the poral usage.
	/// </summary>
	/// <returns>
	/// The poral usage.
	/// </returns>
	public bool GetPortalUsage()
	{
		return m_PortalUsed;
	}
	
	/// <summary>
	/// Sets the last position in town.
	/// </summary>
	/// <param name='pi_x'>
	/// Pi_x.
	/// </param>
	/// <param name='pi_y'>
	/// Pi_y.
	/// </param>
	/// <param name='pi_z'>
	/// Pi_z.
	/// </param>
	public void 		SetLastPositionInTown		(float pi_x, float pi_y, float pi_z)
	{
		m_PortalUsed = false;
		m_PlayerTown = new Vector3(pi_x, pi_y, pi_z);
	}
	
	/// <summary>
	/// Sets the last position in dungeon.
	/// </summary>
	/// <param name='pi_x'>
	/// Pi_x.
	/// </param>
	/// <param name='pi_y'>
	/// Pi_y.
	/// </param>
	/// <param name='pi_z'>
	/// Pi_z.
	/// </param>
	public void 		SetLastPositionInDungeon	(float pi_x, float pi_y, float pi_z)
	{
		m_PortalUsed = false;
		m_PlayerDungeon = new Vector3(pi_x, pi_y, pi_z);
	}
	
	/// <summary>
	/// Gets the position from town level portal.
	/// </summary>
	/// <returns>
	/// The position from town level portal.
	/// </returns>
	public Vector3 GetPositionFromTownLevelPortal()
	{
		return m_PlayerTownPortal;
	}
	
	/// <summary>
	/// Sets the position dungeon portal.
	/// </summary>
	/// <returns>
	/// The position dungeon portal.
	/// </returns>
	/// <param name='pi_Vector'>
	/// Pi_ vector.
	/// </param>
	public void SetPositionDungeonPortal(Vector3   pi_Vector)
	{
		m_PlayerDungeonPortal = pi_Vector;	
		m_PortalUsed          = true;
	}
	
	/// <summary>
	/// Gets the position from town level.
	/// </summary>
	/// <returns>
	/// The position from town level.
	/// </returns>
	public Vector3 		GetPositionFromTownLevel	()
	{
		return m_PlayerTown;
	}
	
	/// <summary>
	/// Gets the position from dungeon level portal.
	/// </summary>
	/// <returns>
	/// The position from dungeon level portal.
	/// </returns>
	public Vector3      GetPositionFromDungeonLevelPortal()
	{
		m_PortalUsed = false;
		return m_PlayerDungeonPortal;
	}
	
	
	/// <summary>
	/// Gets the position from dungeon level.
	/// </summary>
	/// <returns>
	/// The position from dungeon level.
	/// </returns>
	public Vector3 		GetPositionFromDungeonLevel	()
	{
		m_PortalUsed = false;
		return m_PlayerDungeon;
	}
	
	
	
	
	
	/// <summary>
	/// Sets the player.
	/// </summary>
	public void 		SetPlayer					()
	{
		m_Player = GameObject.Find("player");
	}
	
	/// <summary>
	/// Gets the player.
	/// </summary>
	/// <returns>
	/// The player.
	/// </returns>
	public GameObject 	GetPlayer					()
	{
		return m_Player;
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
