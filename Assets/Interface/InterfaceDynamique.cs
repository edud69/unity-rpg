using UnityEngine;
using System.Collections;

/*********************************
Script édité par Nicolas Messier
pour le cours IFT-2103
29 Octobre 2012
***********************************/


//*********************************GUI DYNAMIQUE************************
public class InterfaceDynamique : MonoBehaviour
{
    //attributs
    public float longeurBarreMaximum;        //longueur de la barre de vie au maximum
    public float longeurBarreActuelle;       //longueur de la barre de vie
    public bool estVieJoueur;   //validation du type de barre
    public GUIText nomTexture;
 

    //methode
    public bool EstVieJoueur
    {
        get { return estVieJoueur; }
        set { estVieJoueur = value; }
    }

    //Pour modifier la barre de vie
    public void OnmodificationLongeurBarre(float santeActuelle, float santeMaximum)
    {
        float calcul = (santeActuelle / santeMaximum);
        longeurBarreActuelle = calcul * longeurBarreMaximum;    //obtenir la longueur
        this.guiTexture.pixelInset = new Rect(this.guiTexture.pixelInset.x, this.guiTexture.pixelInset.y, longeurBarreActuelle, this.guiTexture.pixelInset.height);
    }

    //fonction principale de Unity   
    void Start()
    {
		nomTexture.fontStyle = FontStyle.BoldAndItalic;

        if (estVieJoueur)
        {
            nomTexture.pixelOffset = new Vector2(this.guiTexture.pixelInset.x, this.guiTexture.pixelInset.y + 5);
        }

        longeurBarreMaximum = (int)this.guiTexture.pixelInset.width;
        longeurBarreActuelle = longeurBarreMaximum;

        if (!estVieJoueur)
        {
            OnmodificationLongeurBarre(0.0f, 1);
        }
    }

    void Update()
    {
        float Currenthp = 0;
        float MAXhp = 0;
        
        if (!estVieJoueur && ManagersTable.s_GetPlayerTargetSysManager().GetTarget() == null)
        {
            OnmodificationLongeurBarre(0.0f, MAXhp);
        }
    
        if (estVieJoueur)
        {
            Currenthp = ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<Health>().CurrentHealth;
            MAXhp = ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<Health>().MaxHealth;
			
            nomTexture.text = "HP: " +   Currenthp + "/" + MAXhp +  "   Lvl: " + ManagersTable.s_GetPlayerXPManager().GetPlayerLevel() + "   XP: " +
				              ManagersTable.s_GetPlayerXPManager().GetPlayerXP() + "/" + ManagersTable.s_GetPlayerXPManager().GetPlayerXPMaxForLvl() +
					          "       Or: " + ManagersTable.s_GetPlayerItemsManager().GetPlayerGold();
			
            OnmodificationLongeurBarre(Currenthp, MAXhp);
        }
        else 
        {
            if (ManagersTable.s_GetPlayerTargetSysManager().GetTarget() != null)
            {
                nomTexture.enabled = true;
                MAXhp = ManagersTable.s_GetPlayerTargetSysManager().GetTargetMaxHealth();
                Currenthp = ManagersTable.s_GetPlayerTargetSysManager().GetTargetCurrentHealth();
                string nomMob = ManagersTable.s_GetPlayerTargetSysManager().GetTargetName();
                nomTexture.text = nomMob + "         Lvl: " + ManagersTable.s_GetPlayerTargetSysManager().GetTarget().GetComponent<FreeAI>().m_MobLevel;
                nomTexture.pixelOffset = new Vector2(this.guiTexture.pixelInset.x, this.guiTexture.pixelInset.y + 5);
                OnmodificationLongeurBarre(Currenthp, MAXhp);
            }
            else
            {

                nomTexture.enabled = false;
            }
        }
    }



}



