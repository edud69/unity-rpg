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
	var g_Texture : Texture;
	
	function OnGUI() 
	{
		GUI.depth = 100;
	    if(!g_Texture)
	    {
	        Debug.LogError("Feed the beef. Give a texture!");
	        return;
	    }
	    
	    GUI.DrawTexture(Rect(0,
	                         0,
	                         Screen.width,
	                         Screen.height), 
	                     g_Texture, 
	                     ScaleMode.StretchToFill, 
	                     true, 
	                     0f);
	}