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
/// Dont destroy ennemies.
/// </summary>
public class DontDestroyEnnemies : MonoBehaviour
{
	private bool m_PlayerIsInDungeon;
	
	public static DontDestroyEnnemies  Instance; //Since we use dontdestroyonload <-- we MUST have ONLY ONE instance
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		m_PlayerIsInDungeon = true; //true because it is instantiate from dungeon level
	}
	
	/// <summary>
	/// Gets the player is in dungeon.
	/// </summary>
	/// <returns>
	/// The player is in dungeon.
	/// </returns>
	public bool GetPlayerIsInDungeon()
	{
		return m_PlayerIsInDungeon;
	}
	
	/// <summary>
	/// Sets the player is in dungeon.
	/// </summary>
	/// <param name='pi_bool'>
	/// Pi_bool.
	/// </param>
	public void SetPlayerIsInDungeon(bool pi_bool)
	{
		m_PlayerIsInDungeon = pi_bool;
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
			
			ManagersTable.SetEnnemiesManager(this.gameObject.GetComponent<DontDestroyEnnemies>());
        }
	}
}