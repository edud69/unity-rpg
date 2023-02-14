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
		Screen.showCursor = false;
		
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
	                     ScaleMode.ScaleAndCrop, 
	                     true, 
	                     0f);
	}