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

public class DontDestroyTreasures : MonoBehaviour 
{
	
	public static DontDestroyTreasures  Instance; //Since we use dontdestroyonload <-- we MUST have ONLY ONE instance
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
	}
	
	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
        if(Instance)
            DestroyImmediate(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
			
			ManagersTable.SetTreasuresManager(this.gameObject.GetComponent<DontDestroyTreasures>());
        }
	}
}
