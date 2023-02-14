// Created by FANTASY FREE AI v.2
//Mod. by Mathieu Antonuk

using UnityEngine;
using System.Collections;

public class DistanceActive : MonoBehaviour 
{
	//NEEDS PLAYERS LOCATION TO DETECT DISTANCE
	private PositionManager pPositionManager; //change by Mathieu
	//THE DISTANCE FROM PLAYER THE AI WILL BECOME ACTIVE
	public float DistanceToActivateAI;
	private float DistanceForSpell = 1;
	private AnimationClip idle;
	public int checkdistevery=10;
	private int chcount = 0;
	public bool PlayIdleAnimationWhenDeactive=true;
	private bool m_ReadyToAttack = true;
	
	private DontDestroyEnnemies MobManager;
	
	 private IEnumerator WaitForNextAttack(){  
		yield return new WaitForSeconds(Random.Range(2f, 3f)); 
		m_ReadyToAttack = true; 
		GetComponent<FreeAI>().AttackSpeed = 1f;
		GetComponent<FreeAI>().runspeed= 1.7f;
	}
	
	// Use this for initialization
	void Start () 
	{		
		MobManager 		 = ManagersTable.s_GetEnnemiesManager();
		pPositionManager = ManagersTable.s_GetPlayerPositionManager(); //Mod by Mathieu
		FreeAI AI=(FreeAI)GetComponent("FreeAI");
		idle=AI.IdleAnimation;		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!MobManager.GetPlayerIsInDungeon())
		{
			chcount=0;
		}
		else
		{		
			chcount=chcount+1;
			if(chcount >= checkdistevery)
			{
				if(pPositionManager.GetPlayer().transform)
				{ 
					//Mod by Mathieu
					//GET DISTANCE
					float dist=Vector3.Distance(transform.position, pPositionManager.GetPlayer().transform.position); //Mod by Mathieu
					//GET AI COMPONENT
				 	FreeAI AI=(FreeAI)GetComponent("FreeAI");
				
				
					//WHEN DISTANCE BECOMES LESS THE ACTIVATE DISTANCE
					if(dist<=DistanceToActivateAI)
					{
						if(dist<=DistanceForSpell)
						{
							if(m_ReadyToAttack) 
							{
								 m_ReadyToAttack = false;
								StartCoroutine(WaitForNextAttack());
							}
						}
						
						if(AI.enabled)
						{
						}
						else 
						{
							AI.enabled=true;
						}
						
						if(rigidbody.isKinematic)rigidbody.isKinematic=false;
					}
					else
					{
						if(rigidbody.isKinematic){}
						else rigidbody.isKinematic=true;
					
						if(AI.enabled)AI.enabled=false;
						if(PlayIdleAnimationWhenDeactive)
						{
							if(AI.IsDead){}
							else
							{
								if(AI.IdleAnimation)
								{
									AI.AICharacter.animation.CrossFade( idle.name, 0.12f);
								}
							}
						}
					}
				}
			}
		}
	}
}
