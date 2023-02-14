/*
 * Owner	: BY FREE FANTASY AI 2.0
 *
 * Modification History 
 * --------------------
 *
 * Dated	Version		Who		Description
 * ----------------------------------------------------------
 * oct 2012  2.1		Mathieu Antonuk       Implementation for the player AND ennemies, + Code cleanup
 * oct 2012  2.2		Mathieu Antonuk		  Support for target system and XP system
 * oct 2012	 3.0		Mathieu Antonuk		  Implementation of sounds
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FreeAI : MonoBehaviour {
	//THE CHARACTER COLLISION LAYER FOR TARGETS
	public Transform AICharacter;
	
	
	/* MODIFIED 
	---------------------------
	---------------------
	by Mathieu */
	public int 		       m_XpToGiveOnDeath;	
	public int			   m_MobLevel;
	private AudioClip	   m_pRunSE;
	private AudioClip	   m_pDieSE;
	private AudioClip      m_pAttackSE;
	PlayerMsgSystemManager m_pPlayerMsgSystemManager;
	/* MODIFIED 
	---------------------------
	---------------------
	by Mathieu */
	
	public int CharacterCollisionLayer=15;
	//ENABLES MELEE COMBAT
	public bool EnableCombat=true;
	//THE TARGET WHICH HE FOLLOWS AND ATTACKS
	public Transform Target;
	//THE VECTOR OF THE TARGET
	private Vector3 CurrentTarget;
	//TARGET VISIBIL BOOL
	private bool TargetVisible;
	private bool MoveToTarget;
	//SPEED WHICH THE AI TURNS
	public float turnspeed=5;
	//SPEED WHICH AI RUNS
	public float runspeed=4;
	public float Damage=10;
	public float AttackSpeed=1;
	public float AttackRange=5;
	
	//WHEN THE DAMAGE HAS BEEN DEALT
	private bool damdealt;

	//ANIMATIONS
	public AnimationClip RunAnimation;
	public AnimationClip IdleAnimation;
	public AnimationClip AttackAnimation;
	public AnimationClip DeathAnimation;
	private bool stop;
	private bool Swing;
	public bool IsDead;
	private bool DeadPlayed;
	
	private MouseCursorScript m_MouseCursor;	
	
	private float Atimer;
	private bool startfollow;
	//PATHFINDING STUFF
	public bool EnableFollowNodePathFinding;
	public bool DebugShowPath;
	public float DistanceNodeChange=1.5f;
	public List<Vector3> Follownodes;
	private int curf;

	/* MODIFIED 
	---------------------------
	---------------------
	by Mathieu */
	private bool 				TargetReassigned;
	private PlayerXPManager 	pPlayerXPManager;
	/* MODIFIED 
	---------------------------
	---------------------
	by Mathieu */
	
	/* MODIFIED 
	---------------------------
	---------------------
	by Samuel */
	FenetreQuetes			m_Quest;
	/* MODIFIED 
	---------------------------
	---------------------
	by Samuel */
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		/* MODIFIED 
	---------------------------
	---------------------
	by Samuel */
		m_Quest					  = ManagersTable.s_GetQuest();
		/* MODIFIED 
	---------------------------
	---------------------
	by Samuel */
		if(gameObject.name == "Skeleton")
		{
			m_pRunSE    = (AudioClip)Resources.Load("Sounds/Skeletons/run", typeof(AudioClip));
			m_pDieSE    = (AudioClip)Resources.Load("Sounds/Skeletons/die", typeof(AudioClip));
			m_pAttackSE = (AudioClip)Resources.Load("Sounds/Skeletons/attack", typeof(AudioClip));
			audio.pitch = 1.5f;
		} 
		else if(gameObject.name == "demonBOSS")
		{
			m_pRunSE    = (AudioClip)Resources.Load("Sounds/demonBoss/walk", typeof(AudioClip));
			m_pDieSE    = (AudioClip)Resources.Load("Sounds/demonBoss/die", typeof(AudioClip));
			m_pAttackSE = (AudioClip)Resources.Load("Sounds/demonBoss/attack", typeof(AudioClip));
			audio.pitch = 1f;
		}
		else if(gameObject.name == "Golem")
		{
			AICharacter = this.transform;
			m_pRunSE    = (AudioClip)Resources.Load("Sounds/demonBoss/walk", typeof(AudioClip));
			m_pDieSE    = (AudioClip)Resources.Load("Sounds/demonBoss/die", typeof(AudioClip));
			m_pAttackSE = (AudioClip)Resources.Load("Sounds/demonBoss/attack", typeof(AudioClip));
			audio.pitch = 1f;			
		}
		else
		{
			m_pRunSE = null;
		}
		
		m_pPlayerMsgSystemManager = ManagersTable.s_GetPlayerMsgSysManager();
		pPlayerXPManager          = ManagersTable.s_GetPlayerXPManager();	
		if(AICharacter)
		{
		}
		else 
		{
			AICharacter=transform;	
		}
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () 
	{
		if(!ManagersTable.s_GetEnnemiesManager().GetPlayerIsInDungeon())
		{
			//Mod by Mathieu, Freeze the object in space, because level is changed and we dont want the object to fall
			m_MouseCursor = null;
			this.transform.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			Target = null;
			TargetReassigned = false;
		}
		else
		{
			/* MODIFIED 
			---------------------------
			---------------------
			by Mathieu */
			if(!TargetReassigned || Target == null)
			{
				m_MouseCursor = GameObject.Find("Main Camera").GetComponent<MouseCursorScript>();
				this.transform.rigidbody.constraints = RigidbodyConstraints.None;
				this.transform.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
				Target 			   = ManagersTable.s_GetPlayerPositionManager().GetPlayer().transform;
				TargetReassigned   = true;
			}
			/* MODIFIED 
			---------------------------
			---------------------
			by Mathieu */

			if(IsDead)
			{
				if(DeathAnimation)
				{
					if(DeadPlayed)
					{
						/* MODIFIED 
						---------------------------
						---------------------
						by Mathieu */
						if(!AICharacter.animation.isPlaying && !audio.isPlaying)
						{
							/* MODIFIED 
	---------------------------
	---------------------
	by Samuel */
							if(gameObject.name == "Skeleton")
							{
								if (m_Quest.sq_active(1,1))
								{
									m_Quest.maj_sq(1,1);
								}
								else
								{
									m_Quest.squelette_tue();
								}
							}
							else if(gameObject.name == "demonBOSS")
							{
								if (m_Quest.sq_active(2,0) && m_Quest.q_active(2))
								{
									m_Quest.maj_sq(2,0);
								}
								else
								{
									m_Quest.chefdemon_mort();
								}
								m_Quest.maj_succes(2);
							}
							m_Quest.maj_succes(1);
							/* MODIFIED 
	---------------------------
	---------------------
	by Samuel */
							m_pPlayerMsgSystemManager.AddMsgToLog("Vous recevez " + m_XpToGiveOnDeath + "pts d'experience.", PlayerMsgSystemManager.MsgType.MSG_DIALOG);
							pPlayerXPManager.IncPlayerXP(m_XpToGiveOnDeath);
							
							if(this.gameObject == ManagersTable.s_GetPlayerTargetSysManager().GetTarget())
							{
								ManagersTable.s_GetPlayerTargetSysManager().SetTarget(null);
								m_MouseCursor.SetTargetMode(false);
							}
							onDrop ();
							
							DestroyObject(gameObject);
						}
						/* MODIFIED 
						---------------------------
						---------------------
						by Mathieu */
					}
					else 
					{
						audio.clip = m_pDieSE;
						audio.Play();
						
						AICharacter.animation.CrossFade(DeathAnimation.name, 0.1f);	
						
						DeadPlayed = true;
					}
				}
			}
			else
			{
				//COMBAT BEHAVE
				if(Target)
				{
					float Tdist = Vector3.Distance(Target.position, transform.position);	
					if(Tdist<=AttackRange)
					{
						if(TargetVisible)
						{
							stop=true;
						}
					}
					else
					{
						stop = false;
					}
			
					//RAYCAST VISION SYSTEM	
					RaycastHit hit        = new RaycastHit();	
					LayerMask lay         = CharacterCollisionLayer;
					Vector3 pdir          = (Target.transform.position - transform.position).normalized;
					float playerdirection = Vector3.Dot(pdir, transform.forward);
					if(Physics.Linecast(transform.position, Target.position, out hit, lay))
					{
						TargetVisible=false;	
					}
					else
					{
						if(playerdirection > 0)
						{
							startfollow=true;
							TargetVisible=true;	
						}
						//TargetVisible=false;	
					}
				}
				
				//IF THE TARGET IS VISIBLE
				if(TargetVisible)
				{
					CurrentTarget = Target.position;
					MoveToTarget  = true;
				}

				//MOVES/RUNS TO TARGET
				if(MoveToTarget)
				{
					if(stop)
					{
					}
					else
					{
						transform.position += transform.forward * +runspeed * Time.deltaTime;
					}
					
					if(RunAnimation)
					{
						if(stop)
						{
							//COMBAT!
							if(EnableCombat)
							{
								onAttack();								
							}
							else 
							{
								AICharacter.animation.CrossFade( IdleAnimation.name, 0.12f);
							}
						}
						else
						{
							Atimer = 0;
							AICharacter.animation.CrossFade( RunAnimation.name, 0.12f);
							
							if(!audio.isPlaying)
							{
								audio.clip = m_pRunSE;
								audio.Play();
							}
						}
					}
				}
				else
				{
					if(IdleAnimation)
					{
						AICharacter.animation.CrossFade( IdleAnimation.name, 0.12f);
					}
				}		
		
				//FOLLOW PATHFINDING
				if(TargetVisible)
				{
				}
				else
				{
					if(EnableFollowNodePathFinding&startfollow)
					{
						if(Follownodes.Count <= 0)
						{
							Follownodes.Add(CurrentTarget);
						}
		
						RaycastHit hit = new RaycastHit();	
						LayerMask lay  = CharacterCollisionLayer;
					
						if(Physics.Linecast(Follownodes[Follownodes.Count-1], Target.position, out hit, lay))
						{	
							Follownodes.Add(Target.position);	
						}
	
						float dist = Vector3.Distance(transform.position, Follownodes[0]);
						if(dist < DistanceNodeChange)
						{
							Follownodes.Remove(Follownodes[0]);
						}
					}
				}
			}		
		
			if(TargetVisible&Follownodes.Count>0)
			{
				Follownodes.Clear();
			}
		
			if(DebugShowPath)
			{
				if(Follownodes.Count > 0)
				{
					int listsize=Follownodes.Count;
					Debug.DrawLine(Follownodes[0], transform.position, Color.green);
				
					for (int i = 0; i < listsize; i++)
					{
						if(i<Follownodes.Count-1)
						{							
							Debug.DrawLine(Follownodes[i], Follownodes[i+1], Color.green);													
						}
					}				
				}

				//POINT AT TARGET
				if(MoveToTarget)
				{			
					if(Follownodes.Count > 0)
					{
						transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Follownodes[0] - transform.position), turnspeed * Time.deltaTime);	
					}
					else
					{
						transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(CurrentTarget - transform.position), turnspeed * Time.deltaTime);	
					}	
				}				
				transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
			}
		}
	}
	
	private void onDrop() {
		ManagersTable.s_GetPlayerItemsManager().onDrop(this.transform.position);
	}
	
	private void onAttack() {
		Attribute heroAttribute = (Attribute)Target.transform.GetComponent("Attribute");
		Health hp = (Health)Target.transform.GetComponent("Health");	
		
		if(hp.CurrentHealth > 0)
		{
			Atimer += Time.deltaTime;	
			AICharacter.animation[AttackAnimation.name].speed = AICharacter.animation[AttackAnimation.name].length / AttackSpeed;
			AICharacter.animation.CrossFade( AttackAnimation.name, 0.12f);		
						
			if(!damdealt)
			{
				if(Atimer >= AttackSpeed*0.35&Atimer <= AttackSpeed*0.45)
				{											
					bool canAttack = gameObject.GetComponent<Attribute>().canHit(heroAttribute.agility);
					//LETS DO SOME DAMAGE!
					if(hp && canAttack)
					{
						float weaponDmg = 10f;//base dmg
						float bonusDmg = gameObject.GetComponent<Attribute>().getBonusPhysicalDomage(heroAttribute.defence);
						
						hp.CurrentHealth -= (weaponDmg+bonusDmg);
						
						string msgToLog = gameObject.GetComponent<InstanceTargeted>().m_SelfTargetName + " vous inflicte " + (weaponDmg+bonusDmg) + "pts de degats.";						
						m_pPlayerMsgSystemManager.AddMsgToLog(msgToLog, PlayerMsgSystemManager.MsgType.MSG_DMG_TO_PLAYER);
						damdealt=true;	
						
						audio.clip = m_pAttackSE;
						audio.Play();
					} 
					else if (hp)
					{
						damdealt=true;	
						string msgToLog = gameObject.GetComponent<InstanceTargeted>().m_SelfTargetName + " vous manque.";
						m_pPlayerMsgSystemManager.AddMsgToLog(msgToLog, PlayerMsgSystemManager.MsgType.MSG_DMG_TO_PLAYER);
					}
				}
			}
	
			if(Atimer>=AttackSpeed)
			{
				damdealt = false;
				Atimer = 0;
			}
		}
		else 
		{
			AICharacter.animation.CrossFade( IdleAnimation.name, 0.12f);
		}
	}
}
