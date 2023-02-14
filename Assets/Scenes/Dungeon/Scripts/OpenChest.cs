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

/// <summary>
/// Pickup gold.
/// </summary>
public class OpenChest : MonoBehaviour 
{
    float 				m_DoubleClickStart    = 0;
	float	    		m_MinDistanceToPlayer = 2.8f;
	GameObject			m_pChestBottom;
	bool				m_AlreadyOpened = false;
	int 				m_NbItemsInChest = 0;
	PositionManager	    m_pPositionManager;
	
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
		m_pChestBottom     = this.transform.parent.gameObject.transform.FindChild("chest").gameObject; 
		m_pChestBottom.renderer.material.SetColor("_OutlineColor", Color.black);
		m_pChestBottom.renderer.material.SetFloat("_Outline", 0f); //change outline width
		renderer.material.SetColor("_OutlineColor", Color.black);
		renderer.material.SetFloat("_Outline", 0f); //change outline width
	}
	
	/// <summary>
	/// Raises the mouse over event.
	/// </summary>
	void OnMouseOver()
	{
		if(this.CheckDistance() && !m_AlreadyOpened)
		{
			Color pColor = new Color(0.18f, 0.33f, 0f, 1.0f);
			m_pChestBottom.renderer.material.SetColor("_OutlineColor", pColor);
			m_pChestBottom.renderer.material.SetFloat("_Outline", 0.005f); //change outline width
			renderer.material.SetColor("_OutlineColor", pColor);
	   		renderer.material.SetFloat("_Outline", 0.005f); //change outline width
		}
	}
	
	/// <summary>
	/// Raises the mouse exit event.
	/// </summary>
	void OnMouseExit()
	{
		if(!m_AlreadyOpened)
		{
		   m_pChestBottom.renderer.material.SetColor("_OutlineColor", Color.black);
		   m_pChestBottom.renderer.material.SetFloat("_Outline", 0f); //change outline width
		   renderer.material.SetColor("_OutlineColor", Color.black);
		   renderer.material.SetFloat("_Outline", 0f); //change outline width
		}
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
	/// Decs the nb items in chest.
	/// </summary>
	public void DecNbItemsInChest()
	{
		--m_NbItemsInChest;
	}
	
	/// <summary>
	/// Incs the nb items in chest.
	/// </summary>
	public void IncNbItemsInChest()
	{
		++m_NbItemsInChest;
	}
	
	/// <summary>
	/// Gets the nb items in chest.
	/// </summary>
	/// <returns>
	/// The nb items in chest.
	/// </returns>
	public int GetNbItemsInChest()
	{
		return m_NbItemsInChest;
	}

	/// <summary>
	/// Raises the double click event.
	/// </summary>
    void OnDoubleClick()
    {
		if(this.CheckDistance() && !m_AlreadyOpened)
		{
			audio.Play();
			m_AlreadyOpened = true;
			
			StartCoroutine(OpenChestTransform());
		    m_pChestBottom.renderer.material.SetColor("_OutlineColor", Color.black);
		    m_pChestBottom.renderer.material.SetFloat("_Outline", 0f); //change outline width
		    renderer.material.SetColor("_OutlineColor", Color.black);
		    renderer.material.SetFloat("_Outline", 0f); //change outline width
		}
    }
	
	/// <summary>
	/// Opens the chest.
	/// </summary>
	/// <returns>
	/// The chest.
	/// </returns>
	IEnumerator OpenChestTransform()
	{
		uint i = 0;
		while(i < 40)
		{
			transform.Rotate(Vector3.left * 2 * 1);
			transform.Translate(Vector3.up*0.001f);
		    yield return new WaitForSeconds(Random.Range(0.08f, 0.08f));
			++i;
		}
	}
}
