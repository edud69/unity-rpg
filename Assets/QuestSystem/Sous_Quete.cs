/*********************************
Script édité par Samuel Breton
pour le cours IFT-2103
31 Octobre 2012
Mise à jour: 27 novembre 2012
***********************************/

using UnityEngine;
using System.Collections;

public class Sous_Quete 
{
		// ATTRIBUTS DE LA CLASSE SOUS_QUETE
		
		private int combien; // Nombre de fois accomplits pour réussir la sous-quête
		private int compteur; // compteur de sous-quête (Ex: 10, dans 10 ennemis tués sur 20)
		private string description; // explication de la sous-quête
		
		
		// Constructeur de sous-quête
		public Sous_Quete (int pcombien, string pdescription)
		{
			setCombien(pcombien);
			setCompteur(0);
			setDescription(pdescription);
		}
		
		// GESTION DU COMPTEUR/COMBIEN
		
		// Avoir le compteur/combien
		public string getComptCombien()
		{
			string cc;
			cc="( " + getCompteur() + "/" + getCombien() + " )";
			return cc;
		}
		
		// Avoir le compteur seul
		public int getCompteur()
		{
			return compteur;	
		}
		
		// Avoir le combien seul
		
		public int getCombien()
		{
			return combien;	
		}
		
		// Incrémente le compteur
		public void incrm_compteur()
		{
		  	setCompteur(getCompteur()+1);
		}
		
		// Décrémente le compteur
		public void decrm_compteur()
		{
			if (getCompteur()>0)
			{
				setCompteur(getCompteur()-1);	
			}
		}
		
		// Setter combien
		private void setCombien(int c)
		{
			if (c>0)
				combien=c;
			else
				combien=1;
		}
		
		// Setter compteur
		private void setCompteur(int c)
		{
			compteur=c;
		}
		
		// GESTION DU TEXTE
		
		// Affiche la description
		public string getDescription()
		{
			return description;
		}
		
		// Setter description
		private void setDescription(string d)
		{
			description=d;
		}
	
		// Récupérer version longue de titre de sous-quête
		public string getLong_description()
		{
			return getDescription() + " " + getComptCombien();
		}			
}


