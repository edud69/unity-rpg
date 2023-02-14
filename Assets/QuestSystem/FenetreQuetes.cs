/*********************************
Script édité par Samuel Breton
pour le cours IFT-2103
31 Octobre 2012
Mise à jour: 27 novembre 2012
***********************************/
using UnityEngine;
using System.Collections;

public class FenetreQuetes : MonoBehaviour{
	
	//Attributs de la classe FenetreQuetes
	Quetes_List quetes_actives;  //Liste des quêtes actives
	Quetes_List quetes_terminees;//Liste des quêtes terminées
	Quetes_List succes;			 //Liste des succès
	MAJ_Quetes m_maj;			 //Pour afficher les quêtes en haut de l'écran
	
	private int quest_done=0;	 //Compteur de quêtes effectuées
	
	private bool estOuvert = false; //Dit si la fenêtre est ouverte
	private bool estQEC = true;	 	//Dit si l'onglet Quetes en cours est ouvert
	private bool estQT = false;		//Dit si l'onglet Quetes terminées est ouvert
	private bool estS = false;		//Dit si l'onglet Succès est ouvert
	
	private int hauteur_succes=150; //Hauteur d'un box succès
		
	//Vecteurs pour les scrollbox
	private Vector2 scrollViewVector1 = Vector2.zero;  
	private Vector2 scrollViewVector2 = Vector2.zero;
	
	private string[] titres;		//Titres des quêtes de quetes_actives
	private string[][][] desc_sq;	//Descriptions des sous-quêtes des quêtes de quetes_actives
	private string[] descriptions;  //Descriptions des quêtes de quetes_actives
	private string[] recompenses;   //Récompenses des quêtes de quetes_actives
	
	private string[] titres_succes;			//Titres des succès
	private string[] cc_succes;				//Compteur (Compteur/Combien) de succes
	private string[] descriptions_succes;   //Descriptions des succès
	
	private string[] titres_qt;			//Titres des quêtes de quetes_terminees
	private string[][][] desc_sq_qt;	//Descriptions des sous-quêtes des quêtes de quetes_terminees
	private string[] descriptions_qt;	//Descriptions des quêtes de quetes_terminees
	private string[] recompenses_qt;	//Récompenses des quêtes de quetes_terminees
	
	private int[] dimensions=new int[4] {25,0,0,0}; //Liste des dimensions pour l'affichage des quêtes en cours
	
	private string[] description_affiche= new string[7] {"Choisissez une quete","","","","","",""}; //Liste du textes d'une quête pour les quêtes en cours
	
	private int[] dimensions_qt=new int[4] {25,0,0,0}; //Liste des dimensions pour l'affichage des quêtes terminées
	
	private string[] description_affiche_qt= new string[7] {"Choisissez une quete","","","","","",""}; //Liste du textes d'une quête pour les quêtes terminées
	
	private Rect rectangleFenetre = new Rect(100, 100, 800, 500); //Rectangle pour la fenêtre Quêtes
	
	private bool chefdemon=false;  //Indique si le commandant démon a déjà été tué
	private int nb_squelette=0;	   //Nombre de squelettes tués
	private bool potion=false;	   //Indique si la potion de quête a déjà été ramassée
	
	//Fonction Start() de Unity
	void Start()
	{		
		GameObject.Find("excla_chef_1").renderer.enabled=false;
		GameObject.Find("excla_chef_2").renderer.enabled=false;
		succes=  new Quetes_List(10);
		quetes_terminees = new Quetes_List(3);
		quetes_actives = new Quetes_List(3);
		create_quest();
		m_maj=ManagersTable.s_MAJ_Quete();
		// Actives
		
		maj_listes_q(quetes_actives, ref titres, ref descriptions, ref recompenses, ref desc_sq);
		
		// QT
		
		maj_listes_q(quetes_terminees, ref titres_qt, ref descriptions_qt, ref recompenses_qt, ref desc_sq_qt);
		
		// Succès
		
		maj_listes_s();
		m_maj.add_text(quetes_actives.getSq(0,0).getLong_description());
	}
	
	//Fonction Update() de Unity
	public void Update()
	{
		maj_listes_q(quetes_actives, ref titres, ref descriptions, ref recompenses, ref desc_sq);
		maj_listes_q(quetes_terminees, ref titres_qt, ref descriptions_qt, ref recompenses_qt, ref desc_sq_qt);
		maj_listes_s();
		if (Input.GetKeyDown(KeyCode.Q))
	    {
			audio.clip = (AudioClip)Resources.Load("Sounds/Menu/openMenu", typeof(AudioClip));
			audio.Play();
				
	        if (estOuvert)
	        {
	            estOuvert = false;
	        }
	        else
	        {
	            estOuvert = true;
	        }
	    }
	}
	
	#region Text_format
	//Fonction qui sert à mettre en place le texte à afficher
	public void build_description(int position, Quetes_List QL, ref string[] affich, ref string[] t, ref string[][][] dsq, ref string[] d, ref string[] r, bool activ)
	{
		if (position<QL.nbr_quete())
		{
			int nb_sq=QL.nbr_sq(position);
			affich[0]=t[position];
			affich[1]="Sous-quêtes:";
			affich[2]="";
			for (int i=0;i<nb_sq;i++)
			{
				if (activ)
				{
					if ((dsq[position][i][1].Equals("A")) || (dsq[position][i][1].Equals("T")))
					{
						affich[2]+="-";
						affich[2]+=dsq[position][i][0];
						affich[2]+="\n";
					}
				}
				else
				{
					if (dsq[position][i][1].Equals("T"))
					{
						affich[2]+="-";
						affich[2]+=dsq[position][i][0];
						affich[2]+="\n";
					}
				}		
			}
			affich[3]= "Description:";
			affich[4]= d[position];
			affich[5]= "Recompenses:";
			affich[6]= r[position];
		}
		else
		{
			affich[0]="Choisissez une quete";
			affich[1]="";
			affich[2]="";
			affich[3]="";
			affich[4]="";
			affich[5]="";
			affich[6]="";
		}
	}
	
	//Fonction qui sert à calculer la dimension des Label pour l'affichage
	public void build_dimension(int position, Quetes_List QL, ref int[] dim, ref string[] affich)
	{
		dim[0]=calcul_dim(30,longueur_string(affich[0]),30);
		
		if (position<QL.nbr_quete())
		{
			dim[1]=(20*QL.nbr_sq_actives(position));
		}
		else
		{
			dim[1]=0;
		}
		dim[2]=calcul_dim(20,longueur_string(affich[4]),54)+80;
		dim[3]=calcul_dim(20,longueur_string(affich[6]),54)+24;
	}
	
	//Fonction qui calcul la dimension d'un Label en fonction du texte à l'intérieur
	public int calcul_dim(int dbase, int nbcaract, int nbparligne)
	{
		float temp;
		if (nbcaract<=nbparligne)
		{
			return dbase;
		}
		else 
		{
			temp=Mathf.Ceil(float.Parse(nbcaract.ToString())/float.Parse(nbparligne.ToString()));
			temp=(float.Parse(dbase.ToString())*temp);
		}
		return int.Parse(temp.ToString());
	}
	
	//Fonction qui retoune la longueur d'un string
	public int longueur_string(string s)
	{
		char[] c=s.ToCharArray(0,s.Length);
		int dim=0;
		c.GetLength(dim);
		return dim;
	}
	
	#endregion
	
	#region gestion de quete
	//Fonction qui cré les quêtes au lancement du jeu
	public void create_quest()
	{
		quetes_actives.ajouter_quete('A',"Dieu semble avoir abandonné cet endroit. Je dois comprendre ce qu’il est arrive a cette ile.", 0, "Reconnaissance", 1);
		quetes_actives.ajouter_sq(0,1,"Parler à l’homme à l’entrée du village pour vous informer.",'A',-1);
		
		
		quetes_actives.ajouter_quete('I',"Le mauvais sort s’abat sur nous. Les heretiques ont erige un temple reliant notre monde a l’enfer. Le sacrifice d’ames humaines genere une breche temporaire permettant aux demons d’entrer dans notre monde. Vous devriez aller vous presenter au chef du village, il se trouve au nord-ouest.",1500,"Reduire les effectifs",5);
		quetes_actives.ajouter_sq(1,1,"Parler au chef du village.",'A',1);
		quetes_actives.ajouter_sq(1,8,"Tuez 8 squelettes dans le donjon.",'I',2);
		quetes_actives.ajouter_sq(1,1,"Allez parler au chef du village.",'I',3);
		quetes_actives.ajouter_sq(1,1,"Trouver la potion qui guerira le guerrier du village.",'I',4);
		quetes_actives.ajouter_sq(1,1,"Rapportez potion au chef du village et obtenez votre recompense.",'I',-1);
		
		
		quetes_actives.ajouter_quete('I',"L’armee est affaibli. C’est le temps de donner le coup de grace! Sans leur commandant, ils seront anéantis et nous repousserons les forces ennemies hors du donjon. Allez dans les profondeurs du donjon et tuez le commandant demon. Faites attention, c’est un puissant seigneur de guerre.", 10000, "Trancher la tete",2);
		quetes_actives.ajouter_sq(2,1,"Tuez le commandant démon.",'A',1);
		quetes_actives.ajouter_sq(2,1,"Allez annoncer la mort du commandant au chef du village et récupérer votre recompense.",'I',-1);
		
		succes.ajouter_quete('A', "Vous devez accomplir toutes les quetes du jeu.", 0, "Aventurier", 1);
		succes.ajouter_sq(0, 3,"", 'A', -1);
		
		succes.ajouter_quete('A', "Tuez 16 démons.", 0, "Tueur de démons", 1);
		succes.ajouter_sq(1, 16,"", 'A', -1);
		
		succes.ajouter_quete('A', "Vaincre le commandant demon.", 0, "Qui est le maitre?", 1);
		succes.ajouter_sq(2, 1,"", 'A', -1);
			
	}	
	
	//Fonction qui sert à activer une quête
	public void activer_quete(int idQ)
	{
		if (quetes_actives.getQuete(idQ-1).getStatut().Equals('I'))
		{
			m_maj.add_text("Quetes: " + quetes_actives.getQuete(idQ).getTitre() + " debutee");
			quetes_actives.getQuete(idQ).setStatut('A');
			m_maj.add_text(quetes_actives.getSq(idQ,0).getLong_description());
			if (idQ==2 && chefdemon && sq_active(2,0))
			{
				maj_sq(2,0);	
			}
		}
		
	}
	
	//Fonction qui sert à vérifier si une quête est active
	public bool q_active(int idQ)
	{
		if (quetes_actives.getQuete(idQ).getStatut().Equals('A'))
		{
			return true;
		}
		return false;
	}
	
	//Fonction qui sert à vérifier si une sous-quête est active
	public bool sq_active(int idQ, int idSQ)
	{
		if (quetes_actives.getQuete(idQ).getStatuts(idSQ).Equals('A'))
		{
			return true;
		}
		return false;
	}
	
	//Fonction qui sert à mettre à jour une sous-quête
	public void maj_sq(int idQ, int idSQ)
	{
		if (quetes_actives.getQuete(idQ).getStatut().Equals('A'))
		{
			if (sq_active(idQ,idSQ))
			{
				quetes_actives.incr_sq(idQ,idSQ);
				m_maj.add_text(quetes_actives.getSq(idQ,idSQ).getLong_description());
				if (quetes_actives.estComplete(idQ))
				{
					int rec=quetes_actives.getRecompense(idQ);
					m_maj.add_text("Quetes: " + quetes_actives.getQuete(idQ).getTitre() + " accomplie!");
					if (rec!=0)
					{
						ManagersTable.s_GetPlayerItemsManager().IncPlayerGold(rec);	
					}
					quetes_terminees.ajouter_quete(quetes_actives.getQuete(idQ));
					quetes_actives.enleverQuete(idQ);
					quest_done++;
					maj_succes(0);
				}
				else if ((quetes_actives.getQuete(idQ).nbr_sq()!=idSQ+1) && (quetes_actives.getQuete(idQ).getStatuts(idSQ).Equals('T')))
				{
					m_maj.add_text(quetes_actives.getSq(idQ,idSQ+1).getLong_description());
					if (idQ==1 && (idSQ+1)==1 && nb_squelette>8)
					{
						for (int i=0;i<8;i++)
						{
							maj_sq(1,1);	
						}
					}
					else if (idQ==1 && (idSQ+1)==3 && potion)
					{
						maj_sq(1,3);	
					}
					
				}
			}	
		}
	}
	
	//Fonction qui sert à mettre à jour un succès
	public void maj_succes(int idS)
	{
		if (succes.getQuete(idS).getStatut().Equals('A'))
		{
			succes.incr_sq(idS,0);
			if (succes.getSq(idS,0).getCompteur()==1)
			{
				m_maj.add_text("Succes: " + succes.getQuete(idS).getTitre() + " déverrouille");	
			}
			if (succes.estComplete(idS))
			{
				m_maj.add_text("Succes: " + succes.getQuete(idS).getTitre() + " reussi");
				succes.enleverQuete(idS);
			}
		}
	}
	
	//Get de quest_done
	public int getQuest_done()
	{
		return quest_done;	
	}
	
	public void chefdemon_mort()
	{
		chefdemon=true;	
	}
	
	public void squelette_tue()
	{
		nb_squelette++;	
	}
	
	public void potion_prise()
	{
		potion=true;	
	}
	#endregion
	
	#region MAJ Interface
	//Fonction qui met à jour les listes de textes de quêtes
	public void maj_listes_q(Quetes_List QL, ref string[] t, ref string[] d, ref string[] r, ref string[][][] dsq)
	{	
		int max_q=QL.nbr_max_quete();
		t=new string[max_q];
		d= new string[max_q];
		r=new string[max_q];
		dsq=new string[max_q][][];
		for (int i=0;i<max_q;i++)
		{
			if (i<QL.nbr_quete())
			{
				dsq[i]=new string[QL.nbr_max_sq(i)][];	
				for (int j=0; j<QL.nbr_max_sq(i); j++)
				{
					dsq[i][j]=new string[2];	
				}
			}
		}
		
		t=QL.getTitres();
		dsq=QL.getDesc_sq();
		d=QL.getDescriptions();
		r=QL.getRecompenses();
	}
	
	//Fonction qui met à jour les listes de succès
	public void maj_listes_s()
	{
		int max_suc=succes.nbr_max_quete();
		
		titres_succes=new string[max_suc];
		descriptions_succes=new string [max_suc];
		cc_succes=new string[max_suc];
		
		titres_succes=succes.getTitres();
		descriptions_succes=succes.getDescriptions();
		cc_succes=succes.getCc_succes();
	}
	
	//Fonction OnGui() de Unity
	void OnGUI () {
		
		if (!estOuvert)
        { 
			GUI.depth = 1;
			if (GUI.Button (new Rect (10,145,90,32), "Quetes")) 
			{
				audio.clip = (AudioClip)Resources.Load("Sounds/Menu/openMenu", typeof(AudioClip));
				audio.Play();
				estOuvert = true;
			}
		}
		if (estOuvert)
        { 
			GUI.depth = 0;
			rectangleFenetre = GUI.Window((int)ManagersTable.WINDOWS_IDS.QUEST_WINDOW, rectangleFenetre, creationFenetre, "Quetes");
		}
	}
	
	//Fonction qui cré la fenêtre Quêtes
	public void creationFenetre(int IDFenetre)
    {
		float btn_fermer_largeur=64;
		float btn_fermer_hauteur=32;
		
			
        if (GUI.Button(new Rect(rectangleFenetre.width-btn_fermer_largeur-15, rectangleFenetre.height-btn_fermer_hauteur-15, btn_fermer_largeur, btn_fermer_hauteur), "Fermer"))
        {
			audio.clip = (AudioClip)Resources.Load("Sounds/Menu/openMenu", typeof(AudioClip));
			audio.Play();
            estOuvert = false;
        }
		
		if (GUI.Button(new Rect(20, 25, 150, 32), "Quêtes en cours"))
        {
            estQEC= true;
			estQT=false;
			estS=false;
        }
		
		if (GUI.Button(new Rect(170, 25, 150, 32), "Quêtes terminees"))
        {
            estQT= true;
			estQEC=false;
			estS=false;
        }
		
		if (GUI.Button(new Rect(320, 25, 150, 32), "Succes"))
        {
            estQT= false;
			estQEC=false;
			estS=true;
        }
		
		if (estQEC)
		{
			scrollViewVector1 = GUI.BeginScrollView (new Rect (15, 100, 350, 280), scrollViewVector1, new Rect (0, 0, 0, quetes_actives.nbr_quete()*50));		
			
			for (int i=0; i<quetes_actives.nbr_quete(); i++)
			{
				if (quetes_actives.getQuete(i).getStatut().Equals('A'))
				{
					if (GUI.Button(new Rect(30, 0, 320, 30), titres[i]))
	        		{
						build_description(i, quetes_actives, ref description_affiche, ref titres, ref desc_sq, ref descriptions, ref recompenses,true);
						build_dimension(i, quetes_actives, ref dimensions, ref description_affiche);
	        		}
				}
			}
			
			GUI.EndScrollView();
				
			scrollViewVector2 = GUI.BeginScrollView (new Rect (400, 100, 350, 280), scrollViewVector2, new Rect (0, 0, 0, dimensions[0]+dimensions[1]+dimensions[2]+dimensions[3]+110));
			
			
			creer_onglet_Q(dimensions,description_affiche);	
			
			GUI.EndScrollView();	
		}
		
		if (estQT)
		{
			scrollViewVector1 = GUI.BeginScrollView (new Rect (15, 100, 350, 280), scrollViewVector1, new Rect (0, 0, 0, quetes_terminees.nbr_quete()*50));		
			
			for (int i=0; i<quetes_terminees.nbr_quete(); i++)
			{
				if (GUI.Button(new Rect(30, (i*35), 320, 30), titres_qt[i]))
	        	{
					build_description(i, quetes_terminees, ref description_affiche_qt, ref titres_qt, ref desc_sq_qt, ref descriptions_qt, ref recompenses_qt,false);
					build_dimension(i, quetes_terminees, ref dimensions_qt, ref description_affiche_qt);
	        	}
			}
			
			GUI.EndScrollView();
			
			scrollViewVector2 = GUI.BeginScrollView (new Rect (400, 100, 350, 280), scrollViewVector2, new Rect (0, 0, 0, dimensions_qt[0]+dimensions_qt[1]+dimensions_qt[2]+dimensions_qt[3]+110));
			
			
			creer_onglet_Q(dimensions_qt,description_affiche_qt);	
			
			
			GUI.EndScrollView();
		}
		
		if (estS)
		{
			//succes
			
			scrollViewVector1 = GUI.BeginScrollView (new Rect (20, 115, 700, 320), scrollViewVector1, new Rect (0, 0, 0, (succes.nbr_quete()*hauteur_succes)));
			
			creer_onglet_succes();
			
			GUI.EndScrollView();
			
		}
				
		//Limiter la sortie de la fenetre
        if (rectangleFenetre.x < 0)
        {
            rectangleFenetre.x = 0;
        }

        if (rectangleFenetre.y < 0)
        {
            rectangleFenetre.y = 0;
        }

        if (rectangleFenetre.y > Screen.height - rectangleFenetre.height)
        {
            rectangleFenetre.y = Screen.height - rectangleFenetre.height;
        }

        if (rectangleFenetre.x > Screen.width - rectangleFenetre.width)
        {
            rectangleFenetre.x = Screen.width - rectangleFenetre.width;
        }
		
		GUI.DragWindow();	
	}
	
	//Fonction qui cré l'onglet Quêtes en cours et Quêtes terminées
	public void creer_onglet_Q(int[] dim, string[] affich)
	{
		GUI.TextArea (new Rect (0, 0, 345, 1500), "");
			
		GUI.Label (new Rect (0, 0, 345, dim[0]), affich[0]);
				
		GUI.Label (new Rect (0, dim[0]+10, 345, 25), affich[1]);
				
		GUI.Label (new Rect (10, dim[0]+40, 345, dim[1]), affich[2]);
				
		GUI.Label (new Rect (0, dim[0]+dim[1]+45, 345, 25), affich[3]);
				
		GUI.Label (new Rect (0, dim[0]+dim[1]+75, 345, dim[2]), affich[4]);
				
		GUI.Label (new Rect (0, dim[0]+dim[1]+dim[2]+80, 345, 25), affich[5]);
				
		GUI.Label (new Rect (0, dim[0]+dim[1]+dim[2]+110, 345, dim[3]), affich[6]);
	}
	
	//Fonction qui cré l'onglet Succès
	public void creer_onglet_succes()
	{
		for (int i=0; i<succes.nbr_quete() ; i++)
		{
			GUI.TextArea (new Rect (0, i*hauteur_succes, 690, hauteur_succes), "");
			
			GUI.TextArea (new Rect (25, 25+(i*hauteur_succes), 100, 100), "Image");
			
			// titre de quete
			if (succes.getSq(i,0).getCompteur()==0)
			{
				GUI.Label (new Rect (150, 15+(i*hauteur_succes), 500, 30), "???");
			
				// description de quete
				GUI.Label (new Rect (150, 70+(i*hauteur_succes), 500, 60), "???");
			}
			else
			{
				GUI.Label (new Rect (150, 15+(i*hauteur_succes), 500, 30), titres_succes[i]);
			
				// description de quete
				GUI.Label (new Rect (150, 70+(i*hauteur_succes), 500, 60), descriptions_succes[i]);
			}
			//descrition longue de sq
			GUI.Label (new Rect (600, 15+(i*hauteur_succes), 70, 30), cc_succes[i]);
		}
	}
	#endregion

}

