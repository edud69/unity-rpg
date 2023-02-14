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
/// Moving boat.
/// </summary>
public class MovingBoat : MonoBehaviour 
{

	
	float m_WaveDurationMin = 0.03f;
	float m_WaveDurationMax = 0.03f;
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		StartCoroutine(DoOnOff());
	}
	
	/// <summary>
	/// Dos the on off.
	/// </summary>
	/// <returns>
	/// The on off.
	/// </returns>
	IEnumerator DoOnOff() 
	{
		int i;
	  	
		while(true) 
		{
			i = 0;
			while(80 > i)
			{
		   	 transform.Rotate(Vector3.down * 1 * 1);
		   	 yield return new WaitForSeconds(Random.Range(m_WaveDurationMin, m_WaveDurationMax));
		     ++i;
			}
			i = 0;
			

			while(80 > i)
			{
		     transform.Rotate(Vector3.up * 1 * 1);
		     yield return new WaitForSeconds(Random.Range(m_WaveDurationMin, m_WaveDurationMax));
			 ++i;
			}
	  	}	
	}
}