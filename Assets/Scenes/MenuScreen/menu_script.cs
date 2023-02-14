using UnityEngine;
using System.Collections;

public class menu_script : MonoBehaviour 
{


	// Use this for initialization
	void Start () 
    {
}
	
	// Update is called once per frame
	void Update () 
    {

	}

    void OnGUI()
    { 
        if (GUI.Button(new Rect(Screen.width / 2 - 125, Screen.height / 3.0f, 250, 50), "Commencer l'aventure!"))
        {
            Application.LoadLevel(1);
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 125, Screen.height / 2.25f, 250, 50), "Quitter!"))
        {
            Application.Quit();
        }
    }
}
