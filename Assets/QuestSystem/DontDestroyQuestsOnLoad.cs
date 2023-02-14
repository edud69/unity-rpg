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

public class DontDestroyQuestsOnLoad : MonoBehaviour 
{
	
	private static DontDestroyQuestsOnLoad  Instance;  //Since we use dontdestroyonload <-- we MUST have ONLY ONE instance

	
	void Awake()
	{
        if(Instance)
            DestroyImmediate(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
	}
}
