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

public class MouseCursorScript : MonoBehaviour 
{
	public Texture m_CursorImage;
	public Texture m_CursorTargetImage;
	private bool   m_Targeted;
	
	void Start() 
	{
	    Screen.showCursor = false;
	}
	
	public void SetTargetMode(bool pi_Bool) { m_Targeted = pi_Bool; }
	
	public bool GetTargetMode()	    		{ return m_Targeted; }
	
	void OnGUI() 
	{
		GUI.depth = 0;
	    Vector3 pMousePos = Input.mousePosition;
	    
		if(!m_Targeted)
		{
		    Rect pPos = new Rect(pMousePos.x,
                    			 Screen.height - pMousePos.y,
                     			 m_CursorImage.width,
                     			 m_CursorImage.height);
			
	    	GUI.Label(pPos, m_CursorImage);
		}
		else
		{
			Rect pPos = new Rect(pMousePos.x,
                     			 Screen.height - pMousePos.y,
                     			 m_CursorTargetImage.width,
                     			 m_CursorTargetImage.height);
			
	    	GUI.Label(pPos, m_CursorTargetImage);
		}
	}
}
