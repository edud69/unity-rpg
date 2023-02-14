using UnityEngine;
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

/// <summary>
/// Open door.
/// </summary>
public class OpenDoor : MonoBehaviour 
{

    float 		            m_DoubleClickStart    = 0;
	float	                m_MinDistanceToPlayer = 2.8f;
	PositionManager	        m_pPositionManager;
	
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
		m_pPositionManager = ManagersTable.s_GetPlayerPositionManager();
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
		if(!animation.isPlaying && this.CheckDistance())
		{
			animation.Play();
			audio.Play();
		}
    }
	
}