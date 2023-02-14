/*
 * Owner	: David Tremblay O'Neill
 *
 * Modification History 
 * --------------------
 *
 * Dated	Version		Who		Description
 * ----------------------------------------------------------
 * 
 */

using UnityEngine;
using System;

public class PlayerAttributeManager : MonoBehaviour 
{
	private float strenght = 0;
	private float agility  = 0;
	private float spirit   = 0;	
	private float defence  = 0;
	
	private PlayerAttributeManager 	Instance;
	
	/// Start this instance.	
	void Start () 
	{
		float level = (float)ManagersTable.s_GetPlayerXPManager().GetPlayerLevel();
		strenght  = level;
		agility   = level;
		spirit    = level;
		defence	  = level;
	}
	
	public void setAttribute(float myStrenght, float myAgility, float mySpirit, float myDefence) 
	{
		strenght = myStrenght;
		agility  = myAgility;
		spirit   = mySpirit;
		defence  = myDefence;
	}
	
	public float getStrenght() 
	{
		return strenght;
	}
	
	public float getAgility() 
	{
		return agility;
	}
	
	public float getSpirit() 
	{
		return spirit;
	}
	
	public float getDefence() 
	{
		return defence;
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
