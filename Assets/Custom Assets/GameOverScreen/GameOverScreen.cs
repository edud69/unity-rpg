/*
 * Owner	: Mathieu Antonuk
 *
 * Modification History 
 * --------------------
 *
 * Dated	Version		Who		Description
 * ----------------------------------------------------------
 */

using UnityEngine;
using System.Collections;

public class GameOverScreen : MonoBehaviour 
{
	public Texture m_texture;
	
	
	void Start()
	{
		this.enabled = false;
	}

	/// <summary>
	/// Raises the GU event.
	/// </summary>
	void OnGUI()
	{
		if(!m_texture)
	    {
	        Debug.LogError("Feed the beef. Give a texture!");
	        return;
	    }
	    
	    GUI.DrawTexture(new Rect(0,
	                             0,
	                             Screen.width,
	                             Screen.height), 
	                    m_texture, 
	                    ScaleMode.ScaleAndCrop, 
	                    true, 
	                    0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
