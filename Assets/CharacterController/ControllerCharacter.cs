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
/// Controller character.
/// </summary>
public class ControllerCharacter : MonoBehaviour 
{
	//Private members
	private 	Transform 			m_MyTransform;
	private		float	            m_MinDistanceToPlayer = 2.8f;
	private 	Vector3 			m_Destination;	
	public 		int 				m_MobCollisionLayer	  = 20;
	//spells
	private 	Transform 	burnSpell;
	private		Transform 	burnSpellSound;
	private 	Transform 	iceSpell;
	private		Transform 	iceSpellSound;
	//physical attacks
	private     AudioClip	runSound;
	private 	AudioClip	swordAttackSound;
	private		AudioClip   shieldAttackSound;
	private     AudioClip	ArmorRunSound;
	
	
	private     bool        m_CanMove          = true;
	
	private     bool		m_WarnedShield     = false;
	private     bool		m_WarnedSword      = false;
	private		bool		m_PlayerIsDead     = false;
	
	private     bool        m_BurningSpellCast = false;
	private     bool        m_IceSpellCast     = false;
	
	private		bool		m_FireSpellRdy	   = true;
	private     bool		m_IceSpellRdy      = true;
	private     bool		m_WarpGateReady    = true;
	
	private		GameObject  m_BurningSpellTarget = null;
	private		GameObject  m_IceSpellTarget     = null;
	

	
	//public members
	public 		float 		m_MoveSpeed;	
	
	
	/// <summary>
	/// Gets the ice spell ready.
	/// </summary>
	/// <returns>
	/// The ice spell ready.
	/// </returns>
	public bool		GetIceSpellReady()
	{
		return m_IceSpellRdy;
	}
	
	/// <summary>
	/// Gets the burning spell ready.
	/// </summary>
	/// <returns>
	/// The burning spell ready.
	/// </returns>
	public bool     GetBurningSpellReady()
	{
		return m_FireSpellRdy;
	}
	
	/// <summary>
	/// Gets the warp gate ready.
	/// </summary>
	/// <returns>
	/// The warp gate ready.
	/// </returns>
	public bool GetWarpGateReady()
	{
		return m_WarpGateReady;
	}
	
	
	/// <summary>
	/// Sets the player is dead.
	/// </summary>
	/// <param name='pi_Bool'>
	/// Pi_ bool.
	/// </param>
	public void 			SetPlayerIsDead			(bool  pi_Bool) 		{ m_PlayerIsDead = pi_Bool; }
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{		
		//Sets the initial position
		if(Application.loadedLevelName == "Level_Dungeon")
		{
			ManagersTable.SetSpellSystem(GameObject.Find("PlayerSpells"));
			
			GameObject spellSystem = ManagersTable.GetSpellSystem();
			burnSpell              = spellSystem.transform.FindChild("BurnSpell");
			burnSpellSound         = burnSpell;
			iceSpell               = spellSystem.transform.FindChild("IceSpell");
			iceSpellSound          = iceSpell;
			
			swordAttackSound	   = (AudioClip)Resources.Load("Sounds/Character/swordPlayer", typeof(AudioClip));
			runSound               = (AudioClip)Resources.Load("Sounds/Character/run", typeof(AudioClip));
			shieldAttackSound	   = (AudioClip)Resources.Load("Sounds/Character/shieldSE", typeof(AudioClip));
			ArmorRunSound          = (AudioClip)Resources.Load("Sounds/Character/ArmorRun", typeof(AudioClip));
			
			if(ManagersTable.s_GetPlayerPositionManager().GetPortalUsage())
			{
				transform.position  = ManagersTable.s_GetPlayerPositionManager().GetPositionFromDungeonLevelPortal();
			}
			else
			{
				transform.position  = ManagersTable.s_GetPlayerPositionManager().GetPositionFromDungeonLevel();
			}
			
			ManagersTable.s_GetPlayerPositionManager().SetPlayer();
			ManagersTable.s_GetEnnemiesManager().SetPlayerIsInDungeon(true);
		}
		else
		{
			if(ManagersTable.s_GetPlayerPositionManager().GetPortalUsage())
			{
				transform.position  = ManagersTable.s_GetPlayerPositionManager().GetPositionFromTownLevelPortal();
			}
			else
			{
				transform.position  = ManagersTable.s_GetPlayerPositionManager().GetPositionFromTownLevel();
			}
			
			ManagersTable.s_GetPlayerPositionManager().SetPlayer();
		}
		
		m_MyTransform = transform;
		m_Destination = m_MyTransform.position;

        ManagersTable.s_GetPlayerItemsManager().GetComponent<FenetreInventaire>().SetInterfaceCamera(this.transform.FindChild("CameraAvatar").GetComponent<Camera>());
	}
 
	/// <summary>
	/// Checks the distance.
	/// </summary>
	/// <returns>
	/// The distance.
	/// </returns>
	private bool CheckDistance()
	{
		if(Vector3.Distance(transform.position, ManagersTable.s_GetPlayerTargetSysManager().GetTarget().transform.position) < m_MinDistanceToPlayer)
		{
        	return true;
    	}
    	else
		{
        	return false;
    	}
	}


	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () 
	{
		if(!m_PlayerIsDead) 
		{	
			CheckEquipment();
			CheckFireSpell(); //Act like a thread active when m_BurningSpellCast is true
			CheckIceSpell();  //Act like a thread active when m_IceSpellCast is true
			
			if(m_IceSpellCast || m_BurningSpellCast)
			{
				animation.Play("victory");
			}
			
			if(Input.GetKeyUp("left shift"))
			{
				m_WarnedSword  = false;
				m_WarnedShield = false;
			}
			
			
			if(Input.GetKeyDown ("left shift") && Application.loadedLevelName == "Level_Town")
			{
				ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Vous ne pouvez entrer en mode de bataille en ville.", PlayerMsgSystemManager.MsgType.MSG_DIALOG);	
			}
			
			if(Input.GetKey ("left shift") && Application.loadedLevelName == "Level_Dungeon" && !animation.IsPlaying("victory"))
			{	
				if(Input.GetMouseButton(0))
				{
					//////////////////////////
					//      SWORD ATTACK   //
					/////////////////////////
					if(!animation.IsPlaying("attack"))
					{
						onSwordAttack();					
					}	
				}
			
				
				//////////////////////////
				//      SHIELD ATTACK   //
				/////////////////////////
				else if(Input.GetMouseButton(1))
				{
					if(!animation.IsPlaying("resist"))
					{
						onShieldAttack();
					}
				}
				
				//////////////
				//IDLE
				//////////////
				else if((!animation.IsPlaying("attack")) && (!animation.IsPlaying("resist")) && (!animation.IsPlaying("victory")))				 
				{
					animation.Play ("idlebattle");	
				}
			}
			else 
			{
				//////////////
				//RUN
				//////////////
				if (Input.GetMouseButton (1) && GUIUtility.hotControl == 0 && m_CanMove && !animation.IsPlaying("victory")) 
				{	
					animation.Play ("run");
					if(!audio.isPlaying)
					{
						//Play different footstep sounds if certain armor pieces are equipped
						if(ManagersTable.s_GetPlayerItemsManager().GetBoots() != null ||
						   ManagersTable.s_GetPlayerItemsManager().GetChest() != null )
						{
							audio.clip  = ArmorRunSound;
							audio.Play();	
						}
						else
						{
							audio.clip  = runSound;
							audio.Play();							
						}
					}
					
					Plane pPlayerPlane = new Plane(Vector3.up,m_MyTransform.position);
					Ray   l_Ray        = Camera.main.ScreenPointToRay(Input.mousePosition);
					float l_Hitdist    = 0.0f;
					if (pPlayerPlane.Raycast(l_Ray, out l_Hitdist)) 
					{
						Vector3 l_TargetPoint 	    = l_Ray.GetPoint(l_Hitdist);
						m_Destination	  	 	    = l_Ray.GetPoint(l_Hitdist);
						Quaternion l_TargetRotation = Quaternion.LookRotation(l_TargetPoint - transform.position);
						m_MyTransform.rotation 	    = l_TargetRotation;
					}
					
					m_MyTransform.position = Vector3.MoveTowards(m_MyTransform.position, 
						                                         m_Destination, 
						                                         m_MoveSpeed * Time.deltaTime);
				}
				else
				{
					if(Application.loadedLevelName == "Level_Dungeon" && 
					   !animation.IsPlaying("attack") && 
					   !animation.IsPlaying("resist") && 
					   !animation.IsPlaying("victory"))
					{
						animation.Play ("idlebattle");
						audio.Stop();
					}
					else if(Application.loadedLevelName == "Level_Town")
					{
						animation.Play ("idle");
						audio.Stop();
					}					
				}
				
				if(Input.GetKeyDown(KeyCode.Alpha1) && !m_BurningSpellCast && Application.loadedLevelName == "Level_Dungeon")
				{	
					float l_SpellSpiritCost = 70f;

					////////////////////////////////////////////////
					//  FIRESPELL: VALIDATE ALL CONDITIONS TO CAST
					////////////////////////////////////////////////
					LayerMask lay  = ~(1 << m_MobCollisionLayer);
					RaycastHit hit = new RaycastHit();
					if(ManagersTable.s_GetPlayerTargetSysManager().GetTarget() == null)
					{
						ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Aucun cible selectionnee.", PlayerMsgSystemManager.MsgType.MSG_DIALOG);
					}
					else if(!CheckDistance() || Physics.Linecast(transform.position, ManagersTable.s_GetPlayerTargetSysManager().GetTarget().transform.position, out hit, lay))
					{
						ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Vous ne voyez pas le monstre!", PlayerMsgSystemManager.MsgType.MSG_DIALOG);
					}
					else if(gameObject.GetComponent<Health>().CurrentSpirit < l_SpellSpiritCost)
					{
						ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Vous n'avez pas assez de spirit!", PlayerMsgSystemManager.MsgType.MSG_DIALOG);
					}
					else
					{
						gameObject.GetComponent<Health>().CurrentSpirit -= l_SpellSpiritCost;
						m_BurningSpellTarget = ManagersTable.s_GetPlayerTargetSysManager().GetTarget();
						StartCoroutine(CastFireSpellForSeconds(burnSpellSound.audio.clip.length));
					}
				}
				else if (Input.GetKeyDown(KeyCode.Alpha2) && !m_IceSpellCast && Application.loadedLevelName == "Level_Dungeon")
				{
					float l_SpellSpiritCost = 40f;
					
					////////////////////////////////////////////////
					//  ICESPELL: VALIDATE ALL CONDITIONS TO CAST
					////////////////////////////////////////////////
					LayerMask lay  = ~(1 << m_MobCollisionLayer);
					RaycastHit hit = new RaycastHit();
					if(ManagersTable.s_GetPlayerTargetSysManager().GetTarget() == null)
					{
						ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Aucun cible selectionnee.", PlayerMsgSystemManager.MsgType.MSG_DIALOG);
					}
					else if(!CheckDistance() || Physics.Linecast(transform.position, ManagersTable.s_GetPlayerTargetSysManager().GetTarget().transform.position, out hit, lay))
					{
						ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Vous ne voyez pas le monstre!", PlayerMsgSystemManager.MsgType.MSG_DIALOG);
					}
					else if(gameObject.GetComponent<Health>().CurrentSpirit < l_SpellSpiritCost)
					{
						ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Vous n'avez pas assez de spirit!", PlayerMsgSystemManager.MsgType.MSG_DIALOG);
					}
					else
					{
						gameObject.GetComponent<Health>().CurrentSpirit -= l_SpellSpiritCost;
						m_IceSpellTarget = ManagersTable.s_GetPlayerTargetSysManager().GetTarget();
						StartCoroutine(CastIceSpellForSeconds(iceSpell.audio.clip.length)); // cast pour x secs
					}
                } 
				else if ((Input.GetKeyDown(KeyCode.Alpha1) && Application.loadedLevelName == "Level_Town") ||
                         (Input.GetKeyDown(KeyCode.Alpha2) && Application.loadedLevelName == "Level_Town") ||
					     (Input.GetKeyDown(KeyCode.Alpha0) && Application.loadedLevelName == "Level_Town"))
                {
                    ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Impossible de jeter des sorts en ville.", PlayerMsgSystemManager.MsgType.MSG_DIALOG);
                }
				////////////////////////////////////////////////
				//  PORTALSPELL
				////////////////////////////////////////////////
				else if (Input.GetKeyDown(KeyCode.Alpha0) && !m_IceSpellCast && Application.loadedLevelName == "Level_Dungeon")
				{
				 	ManagersTable.s_GetPlayerTargetSysManager().SetTarget(null); //Remove targeted mobs
					ManagersTable.s_GetEnnemiesManager().SetPlayerIsInDungeon(false); //break the thread for AI mobs
					
				 	ManagersTable.s_GetPlayerPositionManager().SetPositionDungeonPortal(ManagersTable.s_GetPlayerPositionManager().GetPlayer().transform.position); //Registers last position in scene;
					StartCoroutine(WaitForWarpGate(2f));				
				}

			}
		}
	}
	
	
	
	
	/// <summary>
	/// Ons the sword attack.
	/// </summary>
	private void onSwordAttack() 
	{
		PlayerItemsManager.ItemObject weapon = ManagersTable.s_GetPlayerItemsManager().GetRHandWeapon();
		if(weapon != null) 
		{	
			m_WarnedSword = false;
			GameObject ennemyTargeted = ManagersTable.s_GetPlayerTargetSysManager().GetTarget();
			animation.Play ("attack");
			audio.clip = swordAttackSound;
			audio.Play();
			
			if(ennemyTargeted != null) 
			{	
				transform.LookAt(ennemyTargeted.transform);
				
				Attribute ennemyAttributes = ennemyTargeted.GetComponent<Attribute>();
			
				bool canAttack = gameObject.GetComponent<Attribute>().canHit(ennemyAttributes.agility);		
				float l_SwordAtkRange = 1f;
				float l_TempRange     = m_MinDistanceToPlayer;
				m_MinDistanceToPlayer = l_SwordAtkRange;
				
				if(canAttack && CheckDistance()) 
				{
					float weaponDmg = weapon.GetAttackPower();
					float bonusDmg = gameObject.GetComponent<Attribute>().getBonusPhysicalDomage(ennemyAttributes.defence);
					float ennemyHealth = ennemyTargeted.GetComponent<Health>().CurrentHealth;
					ennemyTargeted.GetComponent<Health>().CurrentHealth = ennemyHealth - (weaponDmg+bonusDmg);
					string ennemyName = ManagersTable.s_GetPlayerTargetSysManager().GetTargetName();
					string msgToLog = "Vous infligez " + (weaponDmg+bonusDmg) + " PTS de dommage a "+ ennemyName;
					ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog(msgToLog, PlayerMsgSystemManager.MsgType.MSG_DMG_FROM_PLAYER);
				}
				else
				{
					ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Vous manquez l'attaque.", PlayerMsgSystemManager.MsgType.MSG_DMG_FROM_PLAYER);
				}
				
				m_MinDistanceToPlayer = l_TempRange;
			}						
		}
		else
		{
			if(!m_WarnedSword)
			{
				ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Vous n'avez pas d'arme.", PlayerMsgSystemManager.MsgType.MSG_DIALOG);
				m_WarnedSword = true;
			}
		}
	}
		
	/// <summary>
	/// Ons the shield attack.
	/// </summary>
	private void onShieldAttack() 
	{
		PlayerItemsManager.ItemObject weapon = ManagersTable.s_GetPlayerItemsManager().GetLHandShield();
		if(weapon != null) 
		{	
			m_WarnedShield = false;
			GameObject ennemyTargeted = ManagersTable.s_GetPlayerTargetSysManager().GetTarget();
			animation.Play ("resist");
			audio.clip = shieldAttackSound;
			audio.Play();
			
			if(ennemyTargeted != null) 
			{
				transform.LookAt(ennemyTargeted.transform);
				Attribute ennemyAttributes = ennemyTargeted.GetComponent<Attribute>();
			
				float l_ShieldAtkRange = 1f;
				float l_TempRange      = m_MinDistanceToPlayer;
				m_MinDistanceToPlayer  = l_ShieldAtkRange;
				bool canAttack = gameObject.GetComponent<Attribute>().canHit(ennemyAttributes.agility);				
				if(canAttack && CheckDistance()) 
				{		
					float weaponDmg = weapon.GetDefense()/6f;
					float bonusDmg = gameObject.GetComponent<Attribute>().getBonusPhysicalDomage(ennemyAttributes.defence);
					float ennemyHealth = ennemyTargeted.GetComponent<Health>().CurrentHealth;
					ennemyTargeted.GetComponent<Health>().CurrentHealth = ennemyHealth - (weaponDmg+bonusDmg);
					string ennemyName = ManagersTable.s_GetPlayerTargetSysManager().GetTargetName();
					string msgToLog = "Vous infligez " + (weaponDmg+bonusDmg) + " PTS de dommage a "+ ennemyName;
					ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog(msgToLog, PlayerMsgSystemManager.MsgType.MSG_DMG_FROM_PLAYER);
					ennemyTargeted.rigidbody.AddExplosionForce(500f, gameObject.transform.position, 0f);
				}
				else
				{
					ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Vous manquez l'attaque.", PlayerMsgSystemManager.MsgType.MSG_DMG_FROM_PLAYER);
				}
				
				m_MinDistanceToPlayer = l_TempRange;
			}						
		}
		else
		{
			if(!m_WarnedShield)
			{
				ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Vous n'avez pas de bouclier.", PlayerMsgSystemManager.MsgType.MSG_DIALOG);
				m_WarnedShield = true;
			}
		}
		
	}	
	
	/////////////////////////////////
	//   FIRESPELL
	/////////////////////////////////
 	private void CheckFireSpell() 
	{
		////////////////////////////////////////////////
		//  FIRESPELL: SPELL HAS BEEN VALIDE TO BE CASTED
		////////////////////////////////////////////////
		if(m_BurningSpellCast)
		{
			if(!burnSpell.particleSystem.isPlaying)
			{
				burnSpell.transform.position = m_BurningSpellTarget.transform.position;				
				burnSpell.particleSystem.Play();	
				burnSpellSound.audio.Play();
			}
			else if(burnSpell.particleSystem.isPlaying && m_FireSpellRdy)
			{			
				m_FireSpellRdy = false;
				StartCoroutine(WaitForBurningSpellReady(1f)); //Do dmg every x seconds
				
				if(m_BurningSpellTarget != null)
				{	
					transform.LookAt(m_BurningSpellTarget.transform);
					float ennemySpirit = m_BurningSpellTarget.GetComponent<Attribute>().spirit;
					float baseDmg = ManagersTable.s_GetPlayerXPManager().GetPlayerLevel()+10;
					float bonuseDmg = gameObject.GetComponent<Attribute>().getBonusSpellDomage(ennemySpirit);
					float dmg = baseDmg + bonuseDmg;
					
					Health pMobHealth        = (Health)m_BurningSpellTarget.GetComponent<Health>();
					pMobHealth.CurrentHealth = pMobHealth.CurrentHealth - dmg;
					ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Vous infligez " + dmg + " PTS de dommage a " +
						                                                  m_BurningSpellTarget.GetComponent<InstanceTargeted>().m_SelfTargetName, PlayerMsgSystemManager.MsgType.MSG_DMG_FROM_PLAYER);
				}
			}
		}
	}	

	/////////////////////////////////
	//   ICESPELL
	/////////////////////////////////
	private void CheckIceSpell() 
	{
		////////////////////////////////////////////////
		//  ICESPELL: SPELL HAS BEEN VALIDE TO BE CASTED
		////////////////////////////////////////////////
		if(m_IceSpellCast)
		{
			if(!iceSpell.particleSystem.isPlaying)
			{	
				iceSpell.transform.position = m_IceSpellTarget.transform.position;				
				iceSpell.particleSystem.Play();	
				iceSpellSound.audio.Play();
			}
			else if(iceSpell.particleSystem.isPlaying && m_IceSpellRdy)
			{			
				m_IceSpellRdy = false;
				StartCoroutine(WaitForIceSpellReady(1f)); //Do dmg every x seconds
				
				if(m_IceSpellTarget != null)
				{
					transform.LookAt(m_IceSpellTarget.transform);
					float ennemySpirit = m_IceSpellTarget.GetComponent<Attribute>().spirit;
					float baseDmg = ManagersTable.s_GetPlayerXPManager().GetPlayerLevel()+5;
					float bonuseDmg = gameObject.GetComponent<Attribute>().getBonusSpellDomage(ennemySpirit);
					float dmg = baseDmg + bonuseDmg;
					FreeAI aI = (FreeAI) m_IceSpellTarget.GetComponent<FreeAI>();
				
					if (aI.AttackSpeed != 4f)   aI.AttackSpeed = 4f;
					if (aI.runspeed    != 1.1f) aI.runspeed    = 1.1f;				
					
					Health myHealth = (Health)m_IceSpellTarget.GetComponent<Health>();
					myHealth.CurrentHealth -= dmg;
					ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Vous infligez " + dmg + " PTS de dommage a " +
						                                                 m_IceSpellTarget.GetComponent<InstanceTargeted>().m_SelfTargetName, PlayerMsgSystemManager.MsgType.MSG_DMG_FROM_PLAYER);
				}
			}
		}
	}
	
	/// <summary>
	/// Checks the equipment.
	/// </summary>
	private void CheckEquipment()
	{
		if(ManagersTable.s_GetPlayerItemsManager().GetRHandWeapon() == null)
		{
			this.transform.FindChild("geoSword").renderer.enabled = false;
		}
		else
		{
			this.transform.FindChild("geoSword").renderer.enabled = true;
		}
		
		if(ManagersTable.s_GetPlayerItemsManager().GetShoulder() == null)
		{
			this.transform.FindChild("geoCloth").renderer.enabled = false;
		}
		else
		{
			this.transform.FindChild("geoCloth").renderer.enabled = true;
		}
	}
	
	/// <summary>
	/// Stops the spells.
	/// </summary>
	private void stopSpells() 
	{
		burnSpell.particleSystem.Stop();
		burnSpellSound.audio.Stop();
		iceSpell.particleSystem.Stop();
		iceSpellSound.audio.Stop();
	}
	
	/////////////////////////////////////////////////
	//  THREADS FOR THE DOING DMG INSIDE A SPELLCAST CYCLE
	////////////////////////////////////////////////
	IEnumerator WaitForBurningSpellReady(float pi_Sec) 
	{
		yield return new WaitForSeconds(pi_Sec);
		m_FireSpellRdy = true;
	}
	
	IEnumerator WaitForIceSpellReady(float pi_Sec) 
	{
		yield return new WaitForSeconds(pi_Sec);
		m_IceSpellRdy = true;
	}	
	
	
	
	IEnumerator WaitForWarpGate(float pi_Sec) 
	{
		m_CanMove = false;
		yield return new WaitForSeconds(pi_Sec);
		ManagersTable.s_GetPlayerHealthManager().SetPlayerMaxHealth(ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<Health>().MaxHealth);
		ManagersTable.s_GetPlayerHealthManager().SetPlayerMaxSpirit(ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<Health>().MaxSpirit);
	 	ManagersTable.s_GetPlayerHealthManager().SetPlayerHealth(ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<Health>().CurrentHealth); //save health
	 	ManagersTable.s_GetPlayerHealthManager().SetPlayerMana(ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<Health>().CurrentSpirit); //save spirit
	 	
	 	float strenght = ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<Attribute>().strenght;
	 	float agility  = ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<Attribute>().agility;
	 	float spirit   = ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<Attribute>().spirit;
	 	float defence  = ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<Attribute>().defence;
	 	ManagersTable.s_GetPlayerAttributeManager().setAttribute(strenght, agility, spirit, defence);
	 	
	 	ManagersTable.s_GetPlayerItemsManager().GetComponent<FenetreInventaire>().CloseInventoryWindowForLevelChange();
	 	
	  	Application.LoadLevel("ToTownLoading");	
	}	
	
	
	/////////////////////////////////////////////////
	//  THREADS FOR THE CYCLE TIME OF A SPELLCAST
	////////////////////////////////////////////////
	IEnumerator CastFireSpellForSeconds(float pi_Sec) 
	{
		m_BurningSpellCast = true;
		yield return new WaitForSeconds(pi_Sec);
		m_BurningSpellCast = false;
		stopSpells();
	}
	
	IEnumerator CastIceSpellForSeconds(float pi_Sec) 
	{
		m_IceSpellCast = true;
		yield return new WaitForSeconds(pi_Sec);
		m_IceSpellCast = false;
		stopSpells();
	}
}