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
/// Call explosion.
/// </summary>
public class CallExplosion : MonoBehaviour 
{
	float 	  m_ExplosionIntervalMin = 1.0f;
	
	float 	  m_ExplosionIntervalMax = 5.00f;
	
	Detonator m_EruptOne;
	
	Detonator m_EruptTwo;
	
	Detonator m_EruptThree;
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		GameObject pObjectEruptionOne = GameObject.Find("BigEruption1");
		m_EruptOne = (Detonator) pObjectEruptionOne.GetComponent(typeof(Detonator));
		
		GameObject pObjectEruptionTwo = GameObject.Find("BigEruption2");
		m_EruptTwo = (Detonator) pObjectEruptionTwo.GetComponent(typeof(Detonator));
		
		GameObject pObjectEruptionThree = GameObject.Find("BigEruption3");
		m_EruptThree = (Detonator) pObjectEruptionThree.GetComponent(typeof(Detonator));
		
		StartCoroutine(CallExplosionOneAfterOne());
	}
	
	
	/// <summary>
	/// Calls the explosion one after one.
	/// </summary>
	/// <returns>
	/// The explosion one after one.
	/// </returns>
	IEnumerator CallExplosionOneAfterOne() 
	{
		while(true)
		{
			m_EruptOne.Explode();
			yield return new WaitForSeconds(Random.Range(m_ExplosionIntervalMin, m_ExplosionIntervalMax));
			m_EruptTwo.Explode();
			yield return new WaitForSeconds(Random.Range(m_ExplosionIntervalMin, m_ExplosionIntervalMax));
			m_EruptThree.Explode ();
			yield return new WaitForSeconds(Random.Range(m_ExplosionIntervalMin, m_ExplosionIntervalMax));
		}
	}
}