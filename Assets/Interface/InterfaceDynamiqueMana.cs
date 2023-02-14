using UnityEngine;
using System.Collections;

/*********************************
Script édité par Nicolas Messier
pour le cours IFT-2103
29 Octobre 2012
***********************************/


//*********************************GUI DYNAMIQUE************************
public class InterfaceDynamiqueMana : MonoBehaviour
{
    //attributs
    public float longeurBarreMaximum;        //longueur de la barre de spirit au maximum
    public float longeurBarreActuelle;       //longueur de la barre de spirit
    public GUIText nomTexture;
 

    //Pour modifier la barre de spirit
    public void OnmodificationLongeurBarre(float spiritActuel, float spiriteMaximum)
    {
        float calcul = (spiritActuel / spiriteMaximum);
        longeurBarreActuelle = calcul * longeurBarreMaximum;    //obtenir la longueur
        this.guiTexture.pixelInset = new Rect(this.guiTexture.pixelInset.x, this.guiTexture.pixelInset.y, longeurBarreActuelle, this.guiTexture.pixelInset.height);
    }

    //fonction principale de Unity   
    void Start()
    {
		nomTexture.fontStyle = FontStyle.BoldAndItalic;


        longeurBarreMaximum = (int)this.guiTexture.pixelInset.width;
        longeurBarreActuelle = longeurBarreMaximum;
    }

    void Update()
    {
        nomTexture.text = "Spirit: " + ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<Health>().CurrentSpirit + "/" 
				                     + ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<Health>().MaxSpirit;
		
        OnmodificationLongeurBarre(ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<Health>().CurrentSpirit, 
			                       ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<Health>().MaxSpirit);
	}
}



