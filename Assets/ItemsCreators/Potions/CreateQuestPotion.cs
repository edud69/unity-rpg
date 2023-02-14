/*
 * Owner	: Nicolas Messier
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

public class CreateQuestPotion : MonoBehaviour 
{
	public	int							  m_SellValue   = 0;
	public  float						  m_IncrementMP = 0f;
	public  Texture2D					  m_pTexture;
	
	
    private float 						  m_DoubleClickStart    = 0;
	private float	    				  m_MinDistanceToPlayer = 2.8f;
	private bool						  m_ItemWasTakenByPlayer = false;
	private bool 						  m_ItemPickedUp = false;
	private bool						  guiboutton = false;
	
	FenetreQuetes m_Quest;
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void OnGUI () 
	{
		if (guiboutton)
		{
			if (GUI.Button(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 20, 500, 20), "Vous avez recupere la potion de quete!"))
			{
				if (m_Quest.sq_active(1,3))
				{
					m_Quest.maj_sq(1,3);	
				}
				else
				{
					m_Quest.potion_prise();	
				}
				guiboutton = false;
			}
		}

	}
	
	void Start()
	{
		m_Quest=ManagersTable.s_GetQuest();
	}
	
	
	/// <summary>
	/// Raises the mouse up event.
	/// </summary>
    void OnMouseUp()
    {
			audio.clip = (AudioClip)Resources.Load("Sounds/Items/pickupItem", typeof(AudioClip));
			audio.Play();
			m_ItemWasTakenByPlayer = true;
    }

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		if(m_ItemWasTakenByPlayer)
		{
			if(!audio.isPlaying && !guiboutton)
			{
				DestroyImmediate(gameObject);
			}
			else if(!m_ItemPickedUp)
			{
				//pass here till the audio is finished
				guiboutton = true;
				m_ItemPickedUp = true;
			}
		}
	}
}
