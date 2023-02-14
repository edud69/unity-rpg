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

public class CreateBootsArmor : MonoBehaviour 
{
	public	int							  m_SellValue    = 0;
	public  float						  m_Defense      = 0f;
	public  Texture2D					  m_Texture2D    = null;
	public  int							  m_ReqLvl       = 1;
	public  float						  m_BonusStr     = -1;
	public  float						  m_BonusSpirit  = -1;
	public  float 						  m_BonusAgility = -1;
	
	
    private float 						  m_DoubleClickStart    = 0;
	private float	    				  m_MinDistanceToPlayer = 2.8f;
	private bool						  m_ItemWasTakenByPlayer;
	private bool						  m_ItemPickedUp = false;
	private	PositionManager	    		  m_pPositionManager;
	private PlayerItemsManager			  m_pItemsManager;
	private PlayerMsgSystemManager  	  m_pPlayerMsgSystemManager;
	
	private PlayerItemsManager.ItemObject m_BootsArmor;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		m_pPlayerMsgSystemManager = ManagersTable.s_GetPlayerMsgSysManager();
		m_pPositionManager        = ManagersTable.s_GetPlayerPositionManager();
		m_pItemsManager 	      = ManagersTable.s_GetPlayerItemsManager();
		m_ItemWasTakenByPlayer    = false;
		renderer.material.SetColor("_OutlineColor", Color.black);
		renderer.material.SetFloat("_Outline", 0f); //change outline width
		
		m_BootsArmor = PlayerItemsManager.ItemObject.CreateArmorTypeItem(PlayerItemsManager.ItemType.ITEM_BOOT, 
			                                                             m_Texture2D, 
			                                                             m_SellValue, 
			                                                             m_Defense, 
			                                                             m_ReqLvl, 
			                                                             m_BonusStr, 
			                                                             m_BonusAgility, 
			                                                             m_BonusSpirit);
	}
	

	/// <summary>
	/// Checks the distance.
	/// </summary>
	/// <returns>
	/// The distance.
	/// </returns>
	private bool CheckDistance()
	{
		if(Vector3.Distance(transform.position, m_pPositionManager.GetPlayer().transform.position) < m_MinDistanceToPlayer)
		{
        	return true;
    	}
    	else
		{
        	return false;
    	}
	}
	
	/// <summary>
	/// Raises the mouse over event.
	/// </summary>
	void OnMouseOver()
	{
		if(this.CheckDistance())
		{
			Color pColor = new Color(0.18f, 0.33f, 0f, 1.0f);
			renderer.material.SetColor("_OutlineColor", pColor);
	   		renderer.material.SetFloat("_Outline", 0.005f); //change outline width
		}
	}
	
	/// <summary>
	/// Raises the mouse exit event.
	/// </summary>
	void OnMouseExit()
	{
	   renderer.material.SetColor("_OutlineColor", Color.black);
	   renderer.material.SetFloat("_Outline", 0f); //change outline width
	}
	
	/// <summary>
	/// Raises the mouse up event.
	/// </summary>
    void OnMouseUp()
    {
        if ((Time.time - m_DoubleClickStart) < 0.3f)
        {
            this.OnDoubleClick();
            m_DoubleClickStart = -1;
        }
        else
        {
            m_DoubleClickStart = Time.time;
        }
    }

	/// <summary>
	/// Raises the double click event.
	/// </summary>
    void OnDoubleClick()
    {
		if(this.CheckDistance() && !m_ItemWasTakenByPlayer)
		{
			if(m_pItemsManager.GetPlayerNbItems() + 1 > m_pItemsManager.GetPlayerMaxItems())
			{
				m_pPlayerMsgSystemManager.AddMsgToLog("Votre inventaire est plein.", PlayerMsgSystemManager.MsgType.MSG_DIALOG);
			}
			else
			{
				audio.clip = (AudioClip)Resources.Load("Sounds/Items/pickupItem", typeof(AudioClip));
				audio.Play();
				m_ItemWasTakenByPlayer = true;
			}
		}
    }
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		if(m_ItemWasTakenByPlayer)
		{
			if(!audio.isPlaying)
			{
				DestroyImmediate(gameObject);
			}
			else if(!m_ItemPickedUp)
			{
				//pass here till the audio is finished
				m_pPlayerMsgSystemManager.AddMsgToLog("Vous ramassez des bottes.", PlayerMsgSystemManager.MsgType.MSG_ITEMS);
				renderer.enabled = false; //hide the object untill destruction
				m_pItemsManager.PickupNewItem(ref m_BootsArmor);
				m_ItemPickedUp = true;
			}
		}
	}
}
