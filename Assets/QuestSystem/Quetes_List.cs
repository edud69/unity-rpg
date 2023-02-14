/*********************************
Script édité par Samuel Breton
pour le cours IFT-2103
31 Octobre 2012
Mise à jour: 27 novembre 2012
***********************************/
using UnityEngine;
using System.Collections;

public class Quetes_List 
{
	//Attributs de la classe Quetes_List
	private int compteur_quete; //Nombre de quêtes
	private Quete[] quetes;     //Liste des quêtes
	private int nb_max_quete;	//Nombre maximum de quêtes
	
	//Constructeur paramétré
	public Quetes_List(int max_quete)
	{
		compteur_quete=0;
		nb_max_quete=max_quete;
		quetes=new Quete[nb_max_quete];
	}
	
	//Get/Set
	
	public Quete getQuete(int idQuete)
	{
		return quetes[idQuete];	
	}
	
	public Sous_Quete getSq(int idQuete, int idSQuete)
	{
		return quetes[idQuete].getSq(idSQuete);	
	}
	
	public int nbr_quete()
	{
		return compteur_quete;	
	}
	
	// Retourne la liste des titres des quêtes
	public string[] getTitres()
	{
		string[] titrs=new string[nb_max_quete];
		if (nb_max_quete>0)
		{
			for (int i=0;i<nb_max_quete;i++)
			{
				if (i<compteur_quete)
				{
					titrs[i]=quetes[i].getTitre();
				}
				else
				{
					titrs[i]="Emplacement vide";
				}
			}
		}
		return titrs;
	}
	
	// Retourne la liste des descriptions longues et des statuts de sous-quête pour toutes les quêtes
	public string[][][] getDesc_sq()
	{
		string[][][] desc=new string[nb_max_quete][][];
		
		for (int i=0;i<compteur_quete;i++)
		{
			desc[i]=new string[quetes[i].nbr_max_sq()][];
			for (int j=0;j<quetes[i].nbr_sq();j++)
			{
				desc[i][j]=new string[2];
				desc[i][j][0]=quetes[i].getLong_desc_sq(j);
				desc[i][j][1]=quetes[i].getStatuts(j).ToString();
			}
		}
		return desc;
	}
	
	// Retourne la liste des descriptions des quêtes
	public string[] getDescriptions()
	{
		string[] desc=new string[nb_max_quete];
		if (nb_max_quete>0)
		{
			for (int i=0;i<nb_max_quete;i++)
			{
				if (i<compteur_quete)
				{
					desc[i]=quetes[i].getDescription();
				}
				else
				{
					desc[i]="";
				}
				
			}
		}
		return desc;
	}
	
	// Retourne la liste des récompenses des quêtes
	public string[] getRecompenses()
	{
		string[] recomp=new string[nb_max_quete];
		if (nb_max_quete>0)
		{
			for (int i=0;i<nb_max_quete;i++)
			{
				if (i<compteur_quete)
				{
					if (quetes[i].getRecompense()==0)
					{
						recomp[i]="Aucune";
					}
					else
					{
						recomp[i]=quetes[i].getRecompense().ToString() + " or";
					}
				}
				else
				{
					recomp[i]="";
				}	
			}
		}
		return recomp;
	}
	
	// Retourne la liste des récompenses des quêtes
	public string[] getCc_succes()
	{
		string[] recomp=new string[nb_max_quete];
		if (nb_max_quete>0)
		{
			for (int i=0;i<nb_max_quete;i++)
			{
				if (i<compteur_quete)
				{
					recomp[i]=quetes[i].getCc_succes();
				}
				else
				{
					recomp[i]="";
				}	
			}
		}
		return recomp;
	}
	
	public int nbr_max_quete()
	{
		return nb_max_quete;	
	}
	
	//Gestion des quêtes et sous-quêtes
	
	//Fonction qui sert à ajouter une quête
	public void ajouter_quete(Quete q)
	{
		quetes[compteur_quete]=q;
		compteur_quete++;
	}
	
	//Fonction qui sert à ajouter une quête
	public void ajouter_quete(char stat, string desc, int recomp, string titr, int nb_max_sq)
	{
		Quete temp_q=new Quete(stat,desc,recomp,titr,nb_max_sq);
		quetes[compteur_quete]=temp_q;
		compteur_quete++;
	}
	
	// Ajouter une sous-quête à une quête
	public void ajouter_sq(int idQuete, int pcombien, string pdescription, char stat, int debloq)
	{
		quetes[idQuete].ajouter_SQ(pcombien,pdescription, stat, debloq);
	}
	
	//Retourne le nombre de sous-quêtes maximum d'un quête
	public int nbr_max_sq(int idQuete)
	{
		return quetes[idQuete].nbr_max_sq();
	}
	
	//Retourne le nombre de sous-quête d'une quête
	public int nbr_sq(int idQuete)
	{
		return quetes[idQuete].nbr_sq();	
	}
	
	//Retourne le nombre de sous-quêtes actives ou terminées d'une quête
	public int nbr_sq_actives(int idQuete)
	{
		return quetes[idQuete].nbr_sq_actives_terminees();	
	}
	
	//Incrémente le compteur une sous-quête
	public void incr_sq(int idQ, int idSQ)
	{
		quetes[idQ].incr_SQ(idSQ);	
		quetes[idQ].maj_statuts_sq();
		quetes[idQ].estComplete();
	}
	
	//Indique si une quête est complétée
	public bool estComplete(int idQ)
	{
		return quetes[idQ].estComplete();
	}
	
	//Rend une quête inactive
	public void enleverQuete(int idQ)
	{
		quetes[idQ].setStatut('I');	
	}
	
	//Retourne la récompense d'une quête
	public int getRecompense(int idQ)
	{
		return quetes[idQ].getRecompense();	
	}
	
}
























