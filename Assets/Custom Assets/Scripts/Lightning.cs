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
/// Lightning.
/// </summary>
public class Lightning : MonoBehaviour 
{

	
	float m_OffDurationMin = 0.5f;
	float m_OffDurationMax = 7.0f;
	float m_OnDurationMin = 0.02f;
	float m_OnDurationMax = 1.0f;
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		StartCoroutine(LightOnAndOff());
	}
	
	/// <summary>
	/// Lights the on and off.
	/// </summary>
	/// <returns>
	/// The on and off.
	/// </returns>
	IEnumerator LightOnAndOff() 
	{
		while(true)
		{
		    light.enabled = false;
		    yield return new WaitForSeconds(Random.Range(m_OffDurationMin, m_OffDurationMax));
		    light.enabled = true;
		    yield return new WaitForSeconds(Random.Range(m_OnDurationMin, m_OnDurationMax));
		}
	}
}
