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
public class PlayerHealthManager : MonoBehaviour 
{
	
	private float 						m_PlayerSavedStateHealth = 99999999;  // DO NOT MODIFY
	private float						m_PlayerSavedStateMana   = 99999999;  //  DO NOT MODIFY
	private float                       m_PlayerSavedMaxMana     = 0;         //  DO NOT MODIFY
	private float                       m_PlayerSavedMaxHP       = 0;         //  DO NOT MODIFY
	
	private static PlayerHealthManager  Instance;  //Since we use dontdestroyonload <-- we MUST have ONLY ONE instance
	
	
	public void SetPlayerMaxHealth(float pi_MaxHP)
	{
		m_PlayerSavedMaxHP = pi_MaxHP;
	}
	
	public float GetPlayerMaxHealth()
	{
		return m_PlayerSavedMaxHP;
	}
	
	
	public void SetPlayerMaxSpirit(float pi_MaxMP)
	{
		m_PlayerSavedMaxMana = pi_MaxMP;
	}
	
	public float GetPlayerMaxSpirit()
	{
		return m_PlayerSavedMaxMana;
	}
	
	
	
	/// <summary>
	/// Sets the player health.
	/// </summary>
	/// <param name='pi_Health'>
	/// Pi_ health.
	/// </param>
	public void 			SetPlayerHealth		(float pi_Health)
	{
		if(pi_Health <= 0)
		{
			Debug.LogError("ASSERT FAILURE: Player is dead, it should do the gameOver script!");
		}
		
		m_PlayerSavedStateHealth = pi_Health;
	}
	
	/// <summary>
	/// Gets the player health.
	/// </summary>
	/// <returns>
	/// The player health.
	/// </returns>
	public float 			GetPlayerHealth		()
	{
		return m_PlayerSavedStateHealth;
	}
	


	/// <summary>
	/// Sets the player mana.
	/// </summary>
	/// <param name='pi_mana'>
	/// Pi_mana.
	/// </param>
	public void 			SetPlayerMana		(float pi_mana)
	{
		if(pi_mana < 0)
		{
			Debug.LogError("ASSERT FAILURE: Mana cannot be negative!!!");
		}
		
		m_PlayerSavedStateMana = pi_mana;
	}
	
	/// <summary>
	/// Gets the player mana.
	/// </summary>
	/// <returns>
	/// The player mana.
	/// </returns>
	public float 			GetPlayerMana		()
	{
		return m_PlayerSavedStateMana;
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
