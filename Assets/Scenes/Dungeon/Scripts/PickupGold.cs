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

/// <summary>
/// Pickup gold.
/// </summary>
public class PickupGold : MonoBehaviour 
{
	
	public int			m_AmountGoldToGive;

    float 				m_DoubleClickStart    = 0;
	float	    		m_MinDistanceToPlayer = 2.8f;
	bool				m_ItemWasTakenByPlayer;
	bool				m_GoldGiven = false;
	PositionManager	    m_pPositionManager;
	PlayerItemsManager	m_pItemsManager;
	PlayerMsgSystemManager  m_pPlayerMsgSystemManager;
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
	/// Start this instance.
	/// </summary>
	void Start()
	{
		m_pPlayerMsgSystemManager = ManagersTable.s_GetPlayerMsgSysManager();
		m_pPositionManager        = ManagersTable.s_GetPlayerPositionManager();
		m_pItemsManager 	      = ManagersTable.s_GetPlayerItemsManager();
		m_ItemWasTakenByPlayer    = false;
		renderer.material.SetColor("_OutlineColor", Color.black);
		renderer.material.SetFloat("_Outline", 0f); //change outline width
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
			audio.Play();
			m_ItemWasTakenByPlayer = true;
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
			else if(!m_GoldGiven)
			{
				//pass here till the audio is finished
				m_pPlayerMsgSystemManager.AddMsgToLog("Vous ramassez " + m_AmountGoldToGive + " pieces d'or.", PlayerMsgSystemManager.MsgType.MSG_ITEMS);
				renderer.enabled = false; //hide the object untill destruction
				m_pItemsManager.IncPlayerGold(m_AmountGoldToGive);
				m_GoldGiven = true;
			}
		}
	}
	
}