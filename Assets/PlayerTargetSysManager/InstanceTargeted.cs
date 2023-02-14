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


public class InstanceTargeted : MonoBehaviour 
{
	public string		    		m_SelfTargetName;
	
	private float	    			m_MinDistanceToPlayer = 2.8f;
	private Transform				m_ChildWithShaders;
	private	MouseCursorScript		m_MouseCursor;

	private PlayerTargetSysManager	m_pPlayerTargetSysManager;
	private PositionManager	        m_pPositionManager;

    private bool                    m_Targeted = false;
	
	/// <summary>
	/// Checks the distance.
	/// </summary>
	/// <returns>
	/// The distance.
	/// </returns>
	private bool 		CheckDistance		()
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
		m_pPositionManager        = ManagersTable.s_GetPlayerPositionManager();
		m_pPlayerTargetSysManager = ManagersTable.s_GetPlayerTargetSysManager();
		
		if(this.gameObject.name   == "Skeleton")
		{
			m_ChildWithShaders = this.transform.FindChild("skeletonNormal").FindChild("skeleton");
		} 
		else if(gameObject.name == "demonBOSS")
		{
			m_ChildWithShaders = this.transform.FindChild("Bip001").FindChild("Bip001 Pelvis");
		}
		else if(gameObject.name == "Golem")
		{
			m_ChildWithShaders = this.transform.FindChild("Ice Golem");
		}
		
		this.RestoreContour();
		m_MouseCursor = GameObject.Find("Main Camera").GetComponent<MouseCursorScript>();
	}
	
	
	
	/// <summary>
	/// Raises the mouse over event.
	/// </summary>
	void 				OnMouseOver			()
	{
		if(this.CheckDistance())
		{
			m_MouseCursor.SetTargetMode(true);
			CreateTargetContour();
		}
	}
	
	/// <summary>
	/// Raises the mouse exit event.
	/// </summary>
	void 				OnMouseExit			()
	{
        if (!m_Targeted)
        {
            this.RestoreContour();
        }

		m_MouseCursor.SetTargetMode(false);
	}
	
	/// <summary>
	/// Restores the contour.
	/// </summary>
	public void 		RestoreContour		()
	{
		//Materials[1] and [0] are for the head and for the body
		m_ChildWithShaders.renderer.materials[0].SetColor("_OutlineColor", Color.black);
		m_ChildWithShaders.renderer.materials[0].SetFloat("_Outline", 0f); //change outline width
		
		if(gameObject.name == "Skeleton")
		{
			m_ChildWithShaders.renderer.materials[1].SetColor("_OutlineColor", Color.black);
			m_ChildWithShaders.renderer.materials[1].SetFloat("_Outline", 0f); //change outline width
		}
	}
	
	/// <summary>
	/// Creates the target contour.
	/// </summary>
	public void 		CreateTargetContour	()
	{
		//Materials[1] and [0] are for the head and for the body
		Color pColor = new Color(0.51f, 0.03f, 0.03f, 1.0f);
		m_ChildWithShaders.renderer.materials[0].SetColor("_OutlineColor", pColor);
	   	m_ChildWithShaders.renderer.materials[0].SetFloat("_Outline", 0.005f); //change outline width
		
		if(gameObject.name == "Skeleton")
		{
			m_ChildWithShaders.renderer.materials[1].SetColor("_OutlineColor", pColor);
	   		m_ChildWithShaders.renderer.materials[1].SetFloat("_Outline", 0.005f); //change outline width
		}
	}
	
	/// <summary>
	/// Raises the mouse up event.
	/// </summary>
    void OnMouseUp()
    {
        if (this.CheckDistance())
        {
            m_MouseCursor.SetTargetMode(false);
            m_pPlayerTargetSysManager.SetTarget(this.gameObject);
            m_Targeted = true;
        }
    }
   
}