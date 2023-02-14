/*
 * Owner	: BY FREE FANTASY AI 2.0
 *
 * Modification History 
 * --------------------
 *
 * Dated	Version		Who		Description
 * ----------------------------------------------------------
 * oct 2012  2.1		Mathieu Antonuk       Implementation for the player AND ennemies, + Code cleanup
 * cot 2012  2.2		Mathieu Antonuk		  Manage Player death and call gameover
 * 
 */
using UnityEngine;
using System.Collections;

/// <summary>
/// Health.
/// </summary>
public class Health : MonoBehaviour 
{
	public float MaxHealth = 100;
	public float CurrentHealth;
	public bool  Invincible;
	public bool  Dead;
	
	public float MaxSpirit = 100;
	public float CurrentSpirit;
	public bool  InfiniteSpirit = false;  
	
	private bool m_GameOver = false;
	private bool hasRegenerate = true;
	
	

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		//MAKE THE CURRENT HEALTH THE MAX HEALTH AT START	
		
		/* MODIFIED 
		---------------------------
		---------------------
		by Mathieu */
		
		if(gameObject.name == "player")
		{   
			if(ManagersTable.s_GetPlayerHealthManager().GetPlayerMaxHealth() != 0)
			{
				MaxHealth     = ManagersTable.s_GetPlayerHealthManager().GetPlayerMaxHealth();
			}
			
			if(ManagersTable.s_GetPlayerHealthManager().GetPlayerMaxHealth() != 0)
			{
				MaxSpirit     = ManagersTable.s_GetPlayerHealthManager().GetPlayerMaxSpirit();
			}
				
			CurrentHealth = ManagersTable.s_GetPlayerHealthManager().GetPlayerHealth(); //This is for player
			CurrentSpirit = ManagersTable.s_GetPlayerHealthManager().GetPlayerMana(); //This is for player
		}
		else if (gameObject.name == "Skeleton")
		{//This is for ennemies
			int lvl = gameObject.GetComponent<FreeAI>().m_MobLevel;
			CurrentHealth  = 50 + lvl*2; 
			MaxHealth      = 50 + lvl*2; 
			InfiniteSpirit = true;
			hasRegenerate = false; //we stop the regeneration for the ennemies
		}
		else 
		{//This is for ennemies
			int lvl = gameObject.GetComponent<FreeAI>().m_MobLevel;
			CurrentHealth  = 100 + lvl*30; 
			MaxHealth      = 100 + lvl*30; 
			InfiniteSpirit = true;
			hasRegenerate = false; //we stop the regeneration for the ennemies
		}
		
				/* MODIFIED 
		---------------------------
		---------------------
		by Mathieu */

		
	}

	
	/// <summary>
	/// Dos the on off.
	/// </summary>
	/// <returns>
	/// The on off.
	/// </returns>
	IEnumerator CallBackGameOver()
	{
		//THIS IS ONLY A METHOD FOR THE PLAYER, NOT FOR MOBS
		GameObject pBGM    		   = GameObject.Find ("BGM");
		GameObject pGameOverScreen = GameObject.Find ("GameOverScreen");
		
		pBGM.gameObject.transform.FindChild("BGM_DungeonF1").audio.mute = true;
		pBGM.gameObject.transform.FindChild("BGM_DungeonF2").audio.mute = true;
		pBGM.gameObject.transform.FindChild("BGM_DungeonF3").audio.mute = true;
		pBGM.gameObject.transform.FindChild("BGM_Volcano").audio.mute   = true;
		
		GetComponent<ControllerCharacter>().SetPlayerIsDead(true);
		
		float l_previousPitch = audio.pitch;
		audio.pitch = 1f;
		audio.PlayOneShot((AudioClip)Resources.Load("Sounds/Character/Dying", typeof(AudioClip)));
		
		animation["die"].speed = 1.6f;
		animation.Play("die");	
		
		yield return new WaitForSeconds(Random.Range(4f, 4f));
		audio.PlayOneShot((AudioClip)Resources.Load("Sounds/Character/HeartBeating", typeof(AudioClip)));
		
		animation["die"].speed = 0;
		pGameOverScreen.GetComponent<GameOverScreen>().enabled = true;

	 	Vector3 pInitPosition                = GameObject.Find ("PlayerPositionManager").GetComponent<PositionManager>().GetPositionFromDungeonLevel();
		transform.localPosition              = pInitPosition;
	 	CurrentHealth = MaxHealth; //save health at Max	
		CurrentSpirit = MaxSpirit;
		
		yield return new WaitForSeconds(Random.Range(5f, 5f));
		
		audio.Stop();
		pBGM.gameObject.transform.FindChild("BGM_DungeonF1").audio.mute = false;
		pBGM.gameObject.transform.FindChild("BGM_DungeonF2").audio.mute = false;
		pBGM.gameObject.transform.FindChild("BGM_DungeonF3").audio.mute = false;
		pBGM.gameObject.transform.FindChild("BGM_Volcano").audio.mute   = false;
		
		yield return new WaitForSeconds(Random.Range(1f, 1f));
		pGameOverScreen.GetComponent<GameOverScreen>().enabled = false;
		
		GetComponent<ControllerCharacter>().SetPlayerIsDead(false);
		
		audio.pitch = l_previousPitch;
		m_GameOver  = false;
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () 
	{
		regenerateLifeAndSpirit();
		//IF INVINCIBLE, HE CANNOT DIE..
		if(Invincible)
		{
			CurrentHealth = MaxHealth;	
		}
		else
		{
			if(CurrentSpirit <= 0)
			{
				CurrentSpirit = 0;
			}
			
			if(CurrentHealth <= 0)
			{
				CurrentHealth = 0;
				
				if("player" == this.gameObject.name)//Mod. by Mathieu
				{
					if(!m_GameOver)
					{
						m_GameOver = true;
						StartCoroutine(CallBackGameOver());
					}
				}
				else //Mod. by Mathieu
				{
					Dead=true;
				}
			}	
			
			//MAX HEALTH
			if(CurrentHealth >= MaxHealth)
			{
				CurrentHealth = MaxHealth;
			}
			
			if(CurrentSpirit >= MaxSpirit)
			{
				CurrentSpirit = MaxSpirit;
			}
			
			//WHEN DEATH IS UPON HIM
			if(Dead)
			{
				//TELL THE AI SCRIPT HE IS DEAD
				FreeAI AI = (FreeAI)GetComponent("FreeAI");
				if(AI)
				{
					if(AI.IsDead)
					{
						
					}
					else 
					{
						AI.IsDead=true;
						
					}
				}
			}
		}
	}
	
	private void regenerateLifeAndSpirit() {
		if(hasRegenerate) {
			StartCoroutine(regenerate ());
		}
	}
	
	IEnumerator regenerate() 
	{		
		hasRegenerate = false;
		yield return new WaitForSeconds(2);
		if(CurrentHealth > 0) 
		{
			if(CurrentSpirit < MaxSpirit) CurrentSpirit++;
			if(CurrentHealth < MaxHealth) CurrentHealth++;			
		}		
		hasRegenerate = true;
	}
}
