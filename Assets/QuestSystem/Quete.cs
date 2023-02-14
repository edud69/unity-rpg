/*********************************
Script édité par Samuel Breton
pour le cours IFT-2103
31 Octobre 2012
Mise à jour: 27 novembre 2012
***********************************/
using UnityEngine;
using System.Collections;


public class Quete
	{
		// ATRIBUTS DE LA CLASSE QUETE
	
		private Sous_Quete[] sous_quetes; //Liste des sous-quêtes
		private char[] statuts; //Liste des statuts des sous-quêtes
		private char statut; //Statut de la quête (A=active, T=terminée, I=inactive)
		private string description; //Description de la quête
		private int[] debloques; //Listes des sous-quêtes débloquées par les sous-quêtes
		private int recompense; //Récompense en or de la quête
		private string titre; //Titre de la quête
		private int compteur_sous_quetes; //Nombre de sous-quêtes
		private int nb_max_sq; //Nombre maximum de sous-quêtes
	
	//Constructeur paramétré
	public Quete (char stat, string desc, int recomp, string titr, int 
		max_sq)
	{
		setStatut(stat);
		setDescription(desc);
		setRecompense(recomp);
		setTitre(titr);
		compteur_sous_quetes=0;
		nb_max_sq=max_sq;
		sous_quetes=new Sous_Quete[nb_max_sq];
		statuts=new char[nb_max_sq];
		debloques = new int[nb_max_sq];	
	}
	
	//Get/Set
	
	public string getDescription()
	{
		return description;	
	}
	
	public void setDescription(string d)
	{
		description=d;
	}
		
	public char getStatuts(int position)
	{
		return statuts[position];
	}
		
	public void setStatuts(int position, char s)
	{
		statuts[position]=s;	
	}
	
	public char getStatut()
	{
		return statut;
	}
		
	public void setStatut(char s)
	{
		statut=s;	
	}
	
	public Sous_Quete getSq(int position)
	{
		return sous_quetes[position];	
	}
	
	public int getDebloques_sq(int position)
	{
		return debloques[position];	
	}
	
	public int getRecompense()
	{
		return recompense;	
	}
	
	public void setRecompense(int recomp)
	{
		recompense=recomp;
	}
	
	public string getTitre()
	{
		return titre;	
	}
	
	public void setTitre(string titr)
	{
		titre=titr;	
	}
	
	public string getLong_desc_sq(int position)
	{
		return sous_quetes[position].getLong_description();	
	}
	
	public string getCc_succes()
	{
		return sous_quetes[0].getComptCombien();	
	}
	
	public int nbr_max_sq()
	{
		return nb_max_sq;	
	}
	
	public int nbr_sq_actives_terminees()
	{
		int temp=0;
		for (int i=0;i<compteur_sous_quetes;i++)
		{
			if ((statuts[i].Equals('A')) || (statuts[i].Equals('T')))
			{
				temp++;	
			}
		}
		return temp;
	}
		
	// GESTION DES SOUS-QUETES
		
	// Ajouter une sous-quêtes
	public void ajouter_SQ(int combien, string description, char statut, int debloque)
	{
		Sous_Quete temp_sq= new Sous_Quete(combien,description);
		statuts[compteur_sous_quetes]=statut;
		debloques[compteur_sous_quetes]=debloque;
		sous_quetes[compteur_sous_quetes]=temp_sq;
		compteur_sous_quetes++;
	}
		
	// Mettre à jour le statuts des sq
	public void maj_statuts_sq()
	{
		int deb=-1;
		for (int i=0; i<compteur_sous_quetes; i++)
		{
			if (sous_quetes[i].getCombien()==sous_quetes[i].getCompteur())
			{
				statuts[i]='T';	
				deb=getDebloques_sq(i);
			}
			
			if (deb!=-1)
			{
				statuts[deb]='A';	
			}
		}
	}
			
	// Afficher nombre de sous-quêtes
	public int nbr_sq()
	{
		return compteur_sous_quetes;	
	}
	
	//Fonction qui dit si la quête est complétée
	public bool estComplete()
	{
		int temp=0;
		for (int i=0;i<compteur_sous_quetes;i++)
		{
			if ((statuts[i].Equals('T')))
			{
				temp++;	
			}
		}
		if (temp==compteur_sous_quetes)
		{
			return true;	
		}
		return false;
	}
	
	//Fonction qui incrémente le compteur d'une sous-quête
	public void incr_SQ(int idSQ)
	{
		sous_quetes[idSQ].incrm_compteur();
		if (sous_quetes[idSQ].getCompteur()==sous_quetes[idSQ].getCombien())
		{
			statuts[idSQ]='T';	
		}
	}
		
}




















