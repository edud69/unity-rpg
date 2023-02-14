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

public class Attribute : MonoBehaviour 
{
	public float strenght = 0;
	public float agility  = 0;
	public float spirit   = 0;		
	public float defence = 0;
	
	/// Start this instance.	
	void Start () 
	{
		//build, strenght, agility and spirit in fonction on the ennemy lvl
		if(gameObject.name == "player")
		{
			strenght  = ManagersTable.s_GetPlayerAttributeManager().getStrenght();
			agility   = ManagersTable.s_GetPlayerAttributeManager().getAgility();
			spirit    = ManagersTable.s_GetPlayerAttributeManager().getSpirit();
			defence	  = ManagersTable.s_GetPlayerAttributeManager().getDefence();
		} 
		else if (gameObject.name == "demonBOSS")
		{
			float ennemyLevel = (float)gameObject.GetComponent<FreeAI>().m_MobLevel;
			strenght          = ennemyLevel+30;
			agility           = ennemyLevel+30;
			spirit            = ennemyLevel+30;	
			defence			  = ennemyLevel+30;	
		}
		else 
		{
			float ennemyLevel = (float)gameObject.GetComponent<FreeAI>().m_MobLevel;
			strenght          = ennemyLevel;
			agility           = ennemyLevel;
			spirit            = ennemyLevel;	
			defence			  = ennemyLevel;				
		}
	}
	
			
	public bool canHit(float ennemyAgility)
	{
		float chanceToHit = 0.8f;
		bool attack       = true;
		
		if(0 > ennemyAgility)
		{
			Debug.LogError("ASSERT FAILURE: EmmemyAgility ne peut etre < 0!!!");
		} else 
		{			
			if(agility > ennemyAgility) 
			{			
				chanceToHit = 1f - (1f/(3*(agility - ennemyAgility)));
			}
			else if(agility < ennemyAgility)
			{
				float value = ennemyAgility - agility-5;
				if(value <= 0) value = 0.8f;
				chanceToHit = 1f/value;
			}			
			
			float random = UnityEngine.Random.Range(0f,1f);
			if(random>chanceToHit) attack = false;			
		}
		return attack;
	}
	
	public float getBonusPhysicalDomage(float ennemyDefence)
	{
		float toReturn = 0;
		if(0> ennemyDefence) 
		{		
			Debug.LogError("ASSERT FAILURE: la defence ne peut etre < 0!!!");
		}else
		{			
			if(ennemyDefence < (strenght *2)) 
			{		
				toReturn = (strenght *2) - ennemyDefence;
			}
		}
		
		return toReturn;
	} 
	
	public void addBonusMana(float spirit) 
	{
		this.spirit += spirit;
		gameObject.GetComponent<Health>().MaxSpirit += spirit*2;
	}
	
	public void removeBonusMana(float spirit) 
	{
		this.spirit -= spirit;
		gameObject.GetComponent<Health>().MaxSpirit -= spirit*2;
	}
	
	
	
	public float getBonusSpellDomage(float ennemySpirit) 
	{
		float bonusDmg = 0f;
		if(0f > ennemySpirit)
		{
			Debug.LogError("ASSERT FAILURE: ennemySpirit ne peut etre < 0!!!");
		} else 
		{			
			if(spirit > ennemySpirit)
			{
				bonusDmg = (spirit-ennemySpirit);
			}				
		}
		return bonusDmg;
	}
	
	public void onLevelUp()
	{
		strenght = strenght+1;
		agility  = agility+1;
		spirit   = spirit+1;
	}	
}
