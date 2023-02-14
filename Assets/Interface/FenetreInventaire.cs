	




/* ........


/*********************************
Script édité par Nicolas Messier
pour le cours IFT-2103
16 Octobre 2012
***********************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//*********************************EMPLACEMENT DES EQUIPEMENTS************************

public class EmplacementEquipement
{
    //attributs
    private Texture emplacementIcon;                        //texture d'un emplacement vide       
    private string typeEmplacement;                         //description de l'emplacement
    private Vector2 locationEmplacement = Vector2.zero;     //location du gui de l'emplacement
    private bool estVide = true;                            //validation si l'emplacement est vide ou utilisé
    private bool estSelectionne;                    //validation si l'emplacement est sélectionné (drag and drop)
    private bool estSpecifique = false;                     //validation si l'emplacement est spécifique ou général
    private int pid_Item = -1;                                   //numero de l'objet dans la liste du manager
    

    //GET SET
    public int Pid_Item
    {
        get { return pid_Item; }
        set { pid_Item = value; }
    }

    public Texture EmplacementIcon
    {
        get { return emplacementIcon; }
        set { emplacementIcon = value; }
    }

    public string TypeEmplacement
    {
        get { return typeEmplacement; }
        set { typeEmplacement = value; }
    }

    public bool EstVide
    {
        get { return estVide; }
        set { estVide = value; }
    }

    public bool EstSelectionne
    {
        get { return estSelectionne; }
        set { estSelectionne = value; }
    }

    public bool EstSpecifique
    {
        get { return estSpecifique; }
        set { estSpecifique = value; }
    }

    public Vector2 LocationEmplacement
    {
        get { return locationEmplacement; }
        set { locationEmplacement = value; }
    }


    //Méthodes de la classe
    public void ValidationFocus()
    {
        if (Input.mousePosition.y > (Screen.height - locationEmplacement.y - 32)
            && Input.mousePosition.y < (Screen.height - locationEmplacement.y)
            && Input.mousePosition.x > locationEmplacement.x && Input.mousePosition.x < locationEmplacement.x + 32)
        {
            estSelectionne = true;
        }
        else
        {
            estSelectionne = false;
        }
    }


}


//*********************************EQUIPEMENT DYNAMIQUE************************

public class EquipementDynamique
{
    //attributs
    private Texture icon;                                    //La texture de l'icon
    private string type;                                     //Le type de l'equipement
    private EmplacementEquipement derniereSelection;         //dernier emplacement de l'objet avant la possession
    private EmplacementEquipement survol;                   //l'emplacement survolé de l'objet pendant la possession
    private int pid_Item = 0;                                   //numero de l'objet dans la liste du manager

    //GET SET
    public int Pid_Item
    {
        get { return pid_Item; }
        set { pid_Item = value; }
    }
    
    public Texture Icon
    {
        get { return icon; }
        set { icon = value; }
    }

    public string Type
    {
        get { return type; }
        set { type = value; }
    }

    public EmplacementEquipement DerniereSelection
    {
        get { return derniereSelection; }
        set { derniereSelection = value; }
    }

    public EmplacementEquipement Survol
    {
        get { return survol; }
        set { survol = value; }
    }
}






//*********************************GUI PRINCIPAL************************
public class FenetreInventaire : MonoBehaviour
{
	
	private static int __NB_SLOTS_IN_INVENTORY__ = 40;
	
    //attributs
    private const int dimensionEmplacement = 32;                //dimension d'un emplacement vide
    private bool estOuvert = false;                             //validation si l'écran est ouverte ou non
    private Texture potionVie;                                  //texture des potions de vie
    private Texture bonusDegat;                                 //texture des bonus de dégats
    private Texture arme;                                       //texture des armes
    private Rect rectangleFenetre = new Rect(0, 70, 200, 300); //fenetre de gestion de l'equipement
    private EquipementDynamique equipementDynamique = new EquipementDynamique();            //objet pouvant etre déplacé
    private Rect emplacementDynamique;                          //endroit ou on peut bouger les objets
    private EmplacementEquipement[] emplacementEquipement  = new EmplacementEquipement[__NB_SLOTS_IN_INVENTORY__];      //tableau de saisie des emplacements
    private bool estEnMouvement = false;                        //validation si un objet est en déplacement
    private Vector2 locationTableauEquipement = Vector2.zero;   //l'emplacement du tableau d'item
    private Vector2 PositionBarDefilement = Vector2.zero;       //barre de defilement pour les objets libres
    private int compteur = 0;                                   //compteur d'objets libres
    private EmplacementEquipement dernierEmplacement = new EmplacementEquipement();     //dernier emplacement 
    private int compteurCliqueSouris = 0;                       //nombre de lcique de la souris dans la fenetre
    private Camera interface_camera = new Camera();             //camera pour l'interface
    private Vector2 locationJoueur;                             //location du joueur
    
    //Liste des emplacements
    public Texture emplacementVide;                            //texture des emplacements vides

    private EmplacementEquipement botte = new EmplacementEquipement();            //Emplacement pour les bottes
    private Rect botteLocation  = new Rect(160, 130, dimensionEmplacement, dimensionEmplacement);   //Rectangle d'emplacement
    public Texture botteIcon;                       //public pour glisser une Texture par l'éditeur
    public Texture botteIconFull;                       //texture des bottes utilisée

    public Texture TorseIcon;
    public Texture TorseIconFull;                       //texture utilisée
    private EmplacementEquipement Torse = new EmplacementEquipement();            
    private Rect TorseLocation = new Rect(5, 130, dimensionEmplacement, dimensionEmplacement);

    public Texture BrasGaucheIcon;
    public Texture BrasGaucheIconFull;                       //texture utilisée
    private EmplacementEquipement BrasGauche = new EmplacementEquipement();
    private Rect BrasGaucheLocation = new Rect(160, 85, dimensionEmplacement, dimensionEmplacement);              

    public Texture ArmeDroiteIcon;
    public Texture ArmeDroiteIconFull;                       //texture utilisée
    private EmplacementEquipement ArmeDroite = new EmplacementEquipement();
    private Rect ArmeDroiteLocation = new Rect(5, 85, dimensionEmplacement, dimensionEmplacement);  

    public Texture TeteIcon;
    public Texture TeteIconFull;                       //texture utilisée
    private EmplacementEquipement Tete = new EmplacementEquipement();
    private Rect TeteLocation = new Rect(5, 40, dimensionEmplacement, dimensionEmplacement);                     

    public Texture EpauleIcon;
    public Texture EpauleIconFull;                       //texture utilisée
    private EmplacementEquipement Epaule = new EmplacementEquipement();
    private Rect EpauleLocation = new Rect(160, 40, dimensionEmplacement, dimensionEmplacement);                   
	
	private bool m_DClicked 			= false;
	private bool m_RoutineDClickStarted = false;

    private PlayerItemsManager m_playerItemManager;

    private bool avertissementRequisNTB = false;        //avertissement pour un niveau insuffisant


    //méthode de l'interface
    void OnGUI()
    {		
        //affichage de la fenetre d'avertissement pour un objet de niveau supérieur
        if (avertissementRequisNTB)
        {  
            if (GUI.Button(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 20, 500, 20), "Niveau du joueur trop bas pour équiper l'objet!"))
            {
                avertissementRequisNTB = false;
            }
        }

        if (!estOuvert)
        { 
			GUI.depth = 1;
            if (GUI.Button(new Rect(10,110,90,32), "Inventaire"))
            {
				audio.clip = (AudioClip)Resources.Load("Sounds/Menu/openMenu", typeof(AudioClip));
				audio.Play();
                estOuvert = true;
            }
        }

        if (estOuvert)
        {
			GUI.depth  = 0;
			
            //creation de la fenetre pop-up
            rectangleFenetre = GUI.Window((int)ManagersTable.WINDOWS_IDS.INVENTORY_WINDOW, rectangleFenetre, creationFenetre, "Inventaire");
			
            //gestion du tableau des items
            locationTableauEquipement = Input.mousePosition;
            locationTableauEquipement.y = Screen.height - locationTableauEquipement.y;

            //gestion des objets en déplacement
            if (estEnMouvement && Event.current.type == EventType.MouseUp)
            {
                if (equipementDynamique.Survol.EstSelectionne && equipementDynamique.Survol.EstVide && equipementDynamique.Survol.TypeEmplacement != "Objet")
                {
                    if (equipementDynamique.Survol.TypeEmplacement == equipementDynamique.Type || !equipementDynamique.Survol.EstSpecifique)
                    {
                        if (m_playerItemManager.validationItemLevelValide(equipementDynamique.Pid_Item))
                        {
                            switch (equipementDynamique.Survol.TypeEmplacement)
                            {
                                case "Botte":
                                    botte.EstVide = false;
                                    botte.EmplacementIcon = equipementDynamique.Icon;
                                    botte.Pid_Item = equipementDynamique.Pid_Item;
                                    m_playerItemManager.EquipBoots(botte.Pid_Item);
                                    break;

                                case "Torse":
                                    Torse.EstVide = false;
                                    Torse.EmplacementIcon = equipementDynamique.Icon;
                                    Torse.Pid_Item = equipementDynamique.Pid_Item;								
                                    m_playerItemManager.EquipChest(Torse.Pid_Item);
                                    break;

                                case "BrasGauche":
                                    BrasGauche.EstVide = false;
                                    BrasGauche.EmplacementIcon = equipementDynamique.Icon;
                                    BrasGauche.Pid_Item = equipementDynamique.Pid_Item;
                                    m_playerItemManager.EquipLShield(BrasGauche.Pid_Item);
                                    break;

                                case "ArmeDroite":
                                    ArmeDroite.EstVide = false;
                                    ArmeDroite.EmplacementIcon = equipementDynamique.Icon;
                                    ArmeDroite.Pid_Item = equipementDynamique.Pid_Item;
                                    m_playerItemManager.EquipRWeapon(ArmeDroite.Pid_Item);                             
                                    break;

                                case "Tete":
                                    Tete.EstVide = false;
                                    Tete.EmplacementIcon = equipementDynamique.Icon;
                                    Tete.Pid_Item = equipementDynamique.Pid_Item;
                                    m_playerItemManager.EquipHelm(Tete.Pid_Item);
                                    break;

                                case "Epaule":
                                    Epaule.EstVide = false;
                                    Epaule.EmplacementIcon = equipementDynamique.Icon;
                                    Epaule.Pid_Item = equipementDynamique.Pid_Item;
                                    m_playerItemManager.EquipShoulder(Epaule.Pid_Item);
                                    break;

                            }
                        deplacementTermine(true, false);
                        }
                        else
                        {
                            avertissementRequisNTB = true;
                            deplacementTermine(false, false);
                        }
                    }
                    else
                    {
                        deplacementTermine(false, false);
                    }
                }
                else
                {
                    //validation si clique extérieur de la fenetre pour un trashitem
                    if ((Input.mousePosition.x < rectangleFenetre.x || Input.mousePosition.x > rectangleFenetre.x + rectangleFenetre.width) ||
						(Screen.height - Input.mousePosition.y < rectangleFenetre.y || Screen.height - Input.mousePosition.y > rectangleFenetre.y + rectangleFenetre.height))
                    {
                        equipementDynamique.DerniereSelection.EstVide = true;
                        equipementDynamique.DerniereSelection.Pid_Item = -1;

                        //détruire l'objet
                        m_playerItemManager.TrashItem(equipementDynamique.Pid_Item, false);
                    }
                    //Validation si ajout au tableau d'objet
                    else if ((Input.mousePosition.x >= rectangleFenetre.x || Input.mousePosition.x <= rectangleFenetre.x + rectangleFenetre.width) && (Input.mousePosition.y > rectangleFenetre.height - 120)
                        && trouverEmplacementDisponible() != -1)
                    {
                        int nouvelEmplacement = trouverEmplacementDisponible();

                        emplacementEquipement[nouvelEmplacement].EmplacementIcon = equipementDynamique.Icon;
                        emplacementEquipement[nouvelEmplacement].TypeEmplacement = equipementDynamique.Type;
                        emplacementEquipement[nouvelEmplacement].Pid_Item = equipementDynamique.Pid_Item;
                        emplacementEquipement[nouvelEmplacement].EstVide = false;

                        deplacementTermine(true, true);
                    }
                    else
                    {
						if(equipementDynamique.Survol.EstVide)
						{
	                        int nouvelEmplacement = trouverEmplacementDisponible();
	
	                        emplacementEquipement[nouvelEmplacement].EmplacementIcon = equipementDynamique.Icon;
	                        emplacementEquipement[nouvelEmplacement].TypeEmplacement = equipementDynamique.Type;
	                        emplacementEquipement[nouvelEmplacement].Pid_Item = equipementDynamique.Pid_Item;
	                        emplacementEquipement[nouvelEmplacement].EstVide = false;
	
	                        deplacementTermine(true, true);
						}
						else
						{
                        	deplacementTermine(false, false);
						}
                    }
                }
                estEnMouvement = false;
            }

            if (estEnMouvement == true)
            {
                emplacementDynamique = new Rect(locationTableauEquipement.x - 15, locationTableauEquipement.y - 15, 30, 30);
                GUI.Box(emplacementDynamique, new GUIContent(equipementDynamique.Icon));
            }
        }
    }

    //methode de fin de deplacement d'un objet
    public void deplacementTermine(bool statut, bool tableau)
    {
        switch (statut)
        {
            case true:
                if (!tableau)
                {
                    equipementDynamique.Survol.EmplacementIcon = equipementDynamique.Icon;
                    equipementDynamique.Survol.TypeEmplacement = equipementDynamique.Type;
                    equipementDynamique.Survol.Pid_Item = equipementDynamique.Pid_Item;
                    equipementDynamique.Survol.EstVide = false;
                }
                break;
            case false:
                if (equipementDynamique.DerniereSelection.EstSpecifique)
                {
                    //équipement de l'objet retiré
                    equip();
                }
                else
                {
                    equipementDynamique.DerniereSelection.EmplacementIcon = equipementDynamique.Icon;
                    equipementDynamique.DerniereSelection.TypeEmplacement = equipementDynamique.Type;
                    equipementDynamique.DerniereSelection.Pid_Item = equipementDynamique.Pid_Item;
                    equipementDynamique.DerniereSelection.EstVide = false;
                }
                break;
        }
			
		equipementDynamique.DerniereSelection = null;
    }

    //méthode de la creation de la fenetre
    public void creationFenetre(int IDFenetre)
    {
		GUI.DrawTexture(new Rect(18, 3, 180, 180),
			            interface_camera.targetTexture, ScaleMode.ScaleAndCrop, true);

        if (GUI.Button(new Rect(0, 0, dimensionEmplacement * 2, dimensionEmplacement), "Fermer"))
        {
			audio.clip = (AudioClip)Resources.Load("Sounds/Menu/openMenu", typeof(AudioClip));
			audio.Play();
            estOuvert = false;
        }

       

        //déplacement des items avec la fenetre
        Torse.LocationEmplacement = GUIUtility.GUIToScreenPoint(new Vector2(TorseLocation.x, TorseLocation.y));
        BrasGauche.LocationEmplacement = GUIUtility.GUIToScreenPoint(new Vector2(BrasGaucheLocation.x, BrasGaucheLocation.y));
        ArmeDroite.LocationEmplacement = GUIUtility.GUIToScreenPoint(new Vector2(ArmeDroiteLocation.x, ArmeDroiteLocation.y));
        Tete.LocationEmplacement = GUIUtility.GUIToScreenPoint(new Vector2(TeteLocation.x, TeteLocation.y));
        Epaule.LocationEmplacement = GUIUtility.GUIToScreenPoint(new Vector2(EpauleLocation.x, EpauleLocation.y));
        botte.LocationEmplacement = GUIUtility.GUIToScreenPoint(new Vector2(botteLocation.x, botteLocation.y));
        
        //validation si l'emplacement est sélectionné
        Torse.ValidationFocus();
        BrasGauche.ValidationFocus();
        ArmeDroite.ValidationFocus();
        Tete.ValidationFocus();
        Epaule.ValidationFocus();
        botte.ValidationFocus();

        //ajustement si l'emplacement est selectionné

        //torse
        if (Torse.EstSelectionne)
        {
            if (estEnMouvement)
            {
                equipementDynamique.Survol = Torse;
            }
            else if (Event.current.type == EventType.MouseDrag && !Torse.EstVide)
            {
                if (!estEnMouvement)
                {
                    equipementDynamique.Icon = Torse.EmplacementIcon;
                    equipementDynamique.Type = Torse.TypeEmplacement;
                    equipementDynamique.DerniereSelection = Torse;
                    equipementDynamique.Pid_Item = Torse.Pid_Item;
                    unequip();
                    Torse.EstVide = true;
                    Torse.EmplacementIcon = TorseIcon;
                }
                estEnMouvement = true;
            }
        }

        //bras gauche
        if (BrasGauche.EstSelectionne)
        {
            if (estEnMouvement)
            {
                equipementDynamique.Survol = BrasGauche;
            }
            else if (Event.current.type == EventType.MouseDrag && !BrasGauche.EstVide)
            {
                if (!estEnMouvement)
                {
                    equipementDynamique.Icon = BrasGauche.EmplacementIcon;
                    equipementDynamique.Type = BrasGauche.TypeEmplacement;
                    equipementDynamique.DerniereSelection = BrasGauche;
                    equipementDynamique.Pid_Item = BrasGauche.Pid_Item;
                    unequip();
                    BrasGauche.EstVide = true;
                    BrasGauche.EmplacementIcon = BrasGaucheIcon;
                }
                estEnMouvement = true;
            }
        }

        //ArmeDroite
        if (ArmeDroite.EstSelectionne)
        {
            if (estEnMouvement)
            {
                equipementDynamique.Survol = ArmeDroite;
            }
            else if (Event.current.type == EventType.MouseDrag && !ArmeDroite.EstVide)
            {
                if (!estEnMouvement)
                {
                    equipementDynamique.Icon = ArmeDroite.EmplacementIcon;
                    equipementDynamique.Type = ArmeDroite.TypeEmplacement;
                    equipementDynamique.DerniereSelection = ArmeDroite;
                    equipementDynamique.Pid_Item = ArmeDroite.Pid_Item;
                    unequip();
                    ArmeDroite.EstVide = true;
                    ArmeDroite.EmplacementIcon = ArmeDroiteIcon;
                }
                estEnMouvement = true;
            }
        }

        //Tete
        if (Tete.EstSelectionne)
        {
            if (estEnMouvement)
            {
                equipementDynamique.Survol = Tete;
            }
            else if (Event.current.type == EventType.MouseDrag && !Tete.EstVide)
            {
                if (!estEnMouvement)
                {
                    equipementDynamique.Icon = Tete.EmplacementIcon;
                    equipementDynamique.Type = Tete.TypeEmplacement;
                    equipementDynamique.DerniereSelection = Tete;
                    equipementDynamique.Pid_Item = Tete.Pid_Item;
                    unequip();
                    Tete.EstVide = true;
                    Tete.EmplacementIcon = TeteIcon;
                }
                estEnMouvement = true;
            }
        }

        //Epaule
        if (Epaule.EstSelectionne)
        {
            if (estEnMouvement)
            {
                equipementDynamique.Survol = Epaule;
            }
            else if (Event.current.type == EventType.MouseDrag && !Epaule.EstVide)
            {
                if (!estEnMouvement)
                {
                    equipementDynamique.Icon = Epaule.EmplacementIcon;
                    equipementDynamique.Type = Epaule.TypeEmplacement;
                    equipementDynamique.DerniereSelection = Epaule;
                    equipementDynamique.Pid_Item = Epaule.Pid_Item;
                    unequip();
                    Epaule.EstVide = true;
                    Epaule.EmplacementIcon = EpauleIcon;
                }
                estEnMouvement = true;
            }
        }

        //botte
        if (botte.EstSelectionne)
        {
            if (estEnMouvement)
            {
                equipementDynamique.Survol = botte;
            }
            else if (Event.current.type == EventType.MouseDrag && !botte.EstVide)
            {
                if (!estEnMouvement)
                {
                    equipementDynamique.Icon = botte.EmplacementIcon;
                    equipementDynamique.Type = botte.TypeEmplacement;
                    equipementDynamique.DerniereSelection = botte;
                    equipementDynamique.Pid_Item = botte.Pid_Item;
                    unequip();
                    botte.EstVide = true;
                    botte.EmplacementIcon = botteIcon;
                }
                estEnMouvement = true;
            }
        }

        //Gestion de l'affichage des boutons

        //torse
        if (GUI.RepeatButton(TorseLocation, Torse.EmplacementIcon) && !estEnMouvement)
        { 
            
        }

        //BrasGauche
        if (GUI.RepeatButton(BrasGaucheLocation, BrasGauche.EmplacementIcon) && !estEnMouvement)
        {

        }

        //ArmeDroite
        if (GUI.RepeatButton(ArmeDroiteLocation, ArmeDroite.EmplacementIcon) && !estEnMouvement)
        {

        }

        //Tete
        if (GUI.RepeatButton(TeteLocation, Tete.EmplacementIcon) && !estEnMouvement)
        {

        }

        //Epaule
        if (GUI.RepeatButton(EpauleLocation, Epaule.EmplacementIcon) && !estEnMouvement)
        {

        }

        //botte
        if (GUI.RepeatButton(botteLocation, botte.EmplacementIcon) && !estEnMouvement)
        {

        }
		

        //Affichage de l'argent du joueur
        GUI.Label(new Rect(60, 150, 100, 20), "Or : "+ ManagersTable.s_GetPlayerItemsManager().GetPlayerGold()); 
		
        //Affichage de l'XP
        GUI.Label(new Rect(60, 165, 100, 20), "Level : " + ManagersTable.s_GetPlayerXPManager().GetPlayerLevel().ToString()); 
        GUI.Label(new Rect(60, 180, 100, 20), "XP : " + ManagersTable.s_GetPlayerXPManager().GetPlayerXP().ToString() + "/" + ManagersTable.s_GetPlayerXPManager().GetPlayerXPMaxForLvl().ToString());

        //Gestion des equipements libres d'emplacement
        PositionBarDefilement = GUI.BeginScrollView(new Rect(0, 200, 200, 100), PositionBarDefilement, new Rect(0, 0, 300, 120));
        
        compteur = 0;

        //boucle de creation des emplacements des objets vides
        for (int i = 0; i < 4; ++i)
        {
            for (int j = 0; j < 10; ++j)
            {
                if (emplacementEquipement[compteur] != null)
                {
                    //creation de l'emplacement
                    emplacementEquipement[compteur].LocationEmplacement = GUIUtility.GUIToScreenPoint(new Vector2(30 * j, 30 * i));

                    //ajout de la texture
                    if (emplacementEquipement[compteur].EstVide)
                    {
                        emplacementEquipement[compteur].EmplacementIcon = emplacementVide;
                    }

                    //validation si selectionné
                    emplacementEquipement[compteur].ValidationFocus();

                    if (emplacementEquipement[compteur].EstSelectionne && Event.current.type == EventType.MouseDrag && !emplacementEquipement[compteur].EstVide)
                    {
                        if (!estEnMouvement)
                        {
                            equipementDynamique.Icon = emplacementEquipement[compteur].EmplacementIcon;
                            equipementDynamique.Type = emplacementEquipement[compteur].TypeEmplacement;
                            equipementDynamique.DerniereSelection = emplacementEquipement[compteur];
                            equipementDynamique.Pid_Item = emplacementEquipement[compteur].Pid_Item;
                            emplacementEquipement[compteur].EstVide = true;
							emplacementEquipement[compteur].TypeEmplacement = "Objet";
                            emplacementEquipement[compteur].EmplacementIcon = emplacementVide;
                        }
                        estEnMouvement = true;
                        equipementDynamique.Survol = emplacementEquipement[compteur];
                    }
                    else if (emplacementEquipement[compteur].EstSelectionne && Event.current.type == EventType.MouseUp && !m_RoutineDClickStarted && !emplacementEquipement[compteur].EstVide)
                    {
                        if (compteurCliqueSouris == 0)
                        {
                            dernierEmplacement = emplacementEquipement[compteur];
                            if (!estEnMouvement)
                            {
                                ++compteurCliqueSouris;
                                StartCoroutine(RoutineDClick());
                            }
                        }
                    }
                    else
                    {
                        if (dernierEmplacement.EstSelectionne && emplacementEquipement[compteur].EstSelectionne && compteurCliqueSouris == 1)
                        {
                            if (m_RoutineDClickStarted && Event.current.type == EventType.MouseUp && dernierEmplacement.EstSelectionne && emplacementEquipement[compteur].EstSelectionne)
                            {
                                m_DClicked = true;
                            }

                            if (!m_RoutineDClickStarted)
                            {
                                compteurCliqueSouris = 0;
                            }

                            if (m_DClicked && !emplacementEquipement[compteur].EstVide)
                            {
                                if (m_playerItemManager.validationItemLevelValide(emplacementEquipement[compteur].Pid_Item))
                                {
                                    m_RoutineDClickStarted = false;
                                    compteurCliqueSouris = 0;
                                    switch (emplacementEquipement[compteur].TypeEmplacement)
                                    {
                                        case "ArmeDroite":
                                            if (ArmeDroite.EstVide)
                                            {
                                                ArmeDroite.EstVide = false;
                                                ArmeDroite.EmplacementIcon = emplacementEquipement[compteur].EmplacementIcon;
                                                ArmeDroite.Pid_Item = emplacementEquipement[compteur].Pid_Item;
                                                m_playerItemManager.EquipRWeapon(ArmeDroite.Pid_Item);
                                                emplacementEquipement[compteur].EstVide = true;
                                                emplacementEquipement[compteur].EmplacementIcon = emplacementVide;
                                                emplacementEquipement[compteur].TypeEmplacement = "Objet";
                                            }
                                            break;

                                        case "Botte":
                                            if (botte.EstVide)
                                            {
                                                botte.EstVide = false;
                                                botte.EmplacementIcon = emplacementEquipement[compteur].EmplacementIcon;
                                                botte.Pid_Item = emplacementEquipement[compteur].Pid_Item;
                                                m_playerItemManager.EquipBoots(botte.Pid_Item);
                                                emplacementEquipement[compteur].EstVide = true;
                                                emplacementEquipement[compteur].EmplacementIcon = emplacementVide;
                                                emplacementEquipement[compteur].TypeEmplacement = "Objet";
                                            }
                                            break;

                                        case "Torse":
                                            if (Torse.EstVide)
                                            {
                                                Torse.EstVide = false;
                                                Torse.EmplacementIcon = emplacementEquipement[compteur].EmplacementIcon;
                                                Torse.Pid_Item = emplacementEquipement[compteur].Pid_Item;
                                                m_playerItemManager.EquipChest(Torse.Pid_Item);
                                                emplacementEquipement[compteur].EstVide = true;
                                                emplacementEquipement[compteur].EmplacementIcon = emplacementVide;
                                                emplacementEquipement[compteur].TypeEmplacement = "Objet";
                                            }
                                            break;

                                        case "BrasGauche":
                                            if (BrasGauche.EstVide)
                                            {
                                                BrasGauche.EstVide = false;
                                                BrasGauche.EmplacementIcon = emplacementEquipement[compteur].EmplacementIcon;
                                                BrasGauche.Pid_Item = emplacementEquipement[compteur].Pid_Item;
                                                m_playerItemManager.EquipLShield(BrasGauche.Pid_Item);
                                                emplacementEquipement[compteur].EstVide = true;
                                                emplacementEquipement[compteur].EmplacementIcon = emplacementVide;
                                                emplacementEquipement[compteur].TypeEmplacement = "Objet";
                                            }
                                            break;

                                        case "Tete":
                                            if (Tete.EstVide)
                                            {
                                                Tete.EstVide = false;
                                                Tete.EmplacementIcon = emplacementEquipement[compteur].EmplacementIcon;
                                                Tete.Pid_Item = emplacementEquipement[compteur].Pid_Item;
                                                m_playerItemManager.EquipHelm(Tete.Pid_Item);
                                                emplacementEquipement[compteur].EstVide = true;
                                                emplacementEquipement[compteur].EmplacementIcon = emplacementVide;
                                                emplacementEquipement[compteur].TypeEmplacement = "Objet";
                                            }
                                            break;

                                        case "Epaule":
                                            if (Epaule.EstVide)
                                            {
                                                Epaule.EstVide = false;
                                                Epaule.EmplacementIcon = emplacementEquipement[compteur].EmplacementIcon;
                                                Epaule.Pid_Item = emplacementEquipement[compteur].Pid_Item;
                                                m_playerItemManager.EquipShoulder(Epaule.Pid_Item);
                                                emplacementEquipement[compteur].EstVide = true;
                                                emplacementEquipement[compteur].EmplacementIcon = emplacementVide;
                                                emplacementEquipement[compteur].TypeEmplacement = "Objet";
                                            }
                                            break;

                                        case "HEALTHPOTION":
                                            ManagersTable.s_GetPlayerItemsManager().DrinkHealthPotion(emplacementEquipement[compteur].Pid_Item);
                                            emplacementEquipement[compteur].EstVide = true;
                                            emplacementEquipement[compteur].EmplacementIcon = emplacementVide;
                                            emplacementEquipement[compteur].TypeEmplacement = "Objet";
                                            break;

                                        case "MANAPOTION":

                                            ManagersTable.s_GetPlayerItemsManager().DrinkManaPotion(emplacementEquipement[compteur].Pid_Item);
                                            emplacementEquipement[compteur].EstVide = true;
                                            emplacementEquipement[compteur].EmplacementIcon = emplacementVide;
                                            emplacementEquipement[compteur].TypeEmplacement = "Objet";
                                            break;
                                    }
                                    avertissementRequisNTB = false;
                                }
                                else
                                {
                                    avertissementRequisNTB = true;
                                }
                                m_DClicked = false;
                            }
                            else if (emplacementEquipement[compteur].EstSelectionne && estEnMouvement)
                            {
                                equipementDynamique.Survol = emplacementEquipement[compteur];
                            }
                        }

                        //affichage de l'emplacement
                        if (emplacementEquipement[compteur] == null)
                        {
                            GUI.Box(new Rect(30 * j, 30 * i, 30, 30), new GUIContent(emplacementVide));
                        }
                        else
                        {
                            GUI.Box(new Rect(30 * j, 30 * i, 30, 30), emplacementEquipement[compteur].EmplacementIcon);
                        }
                    }
                    ++compteur;
                }
            }
        }
        GUI.EndScrollView();
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
     
		if(!estEnMouvement)
		{
        	GUI.DragWindow();
		}
    }
	
	
	
    //Méthode pour trouver un emplacement vide
    private int trouverEmplacementDisponible()
    {
        for (int i = 0; i < __NB_SLOTS_IN_INVENTORY__; ++i)
        {
            if (emplacementEquipement[i].EstVide == true)
            {
                return i;
            }
        }
        return -1;
    }

    //Méthode pour ajouter des objets
    public void ajoutInventaire(PlayerItemsManager.ItemObject objetAjoute)
    {
        int emplacementDisponible = trouverEmplacementDisponible();

        if (objetAjoute.GetItemType() == PlayerItemsManager.ItemType.ITEM_CHEST)
        {
            emplacementEquipement[emplacementDisponible].EmplacementIcon = objetAjoute.Get2DTexture();
            emplacementEquipement[emplacementDisponible].TypeEmplacement = "Torse";
            emplacementEquipement[emplacementDisponible].Pid_Item = objetAjoute.GetID();
            emplacementEquipement[emplacementDisponible].EstVide = false;
            emplacementEquipement[emplacementDisponible].EstSpecifique = true;
        }

        if (objetAjoute.GetItemType() == PlayerItemsManager.ItemType.ITEM_BOOT)
        {
            emplacementEquipement[emplacementDisponible].EmplacementIcon = objetAjoute.Get2DTexture();
            emplacementEquipement[emplacementDisponible].TypeEmplacement = "Botte";
            emplacementEquipement[emplacementDisponible].Pid_Item = objetAjoute.GetID();
            emplacementEquipement[emplacementDisponible].EstVide = false;
            emplacementEquipement[emplacementDisponible].EstSpecifique = true;
        }

        if (objetAjoute.GetItemType() == PlayerItemsManager.ItemType.ITEM_RHAND)
        {
            emplacementEquipement[emplacementDisponible].EmplacementIcon = objetAjoute.Get2DTexture();
            emplacementEquipement[emplacementDisponible].TypeEmplacement = "ArmeDroite";
            emplacementEquipement[emplacementDisponible].Pid_Item = objetAjoute.GetID();
            emplacementEquipement[emplacementDisponible].EstVide = false;
            emplacementEquipement[emplacementDisponible].EstSpecifique = true;
        }

        if (objetAjoute.GetItemType() == PlayerItemsManager.ItemType.ITEM_LHAND)
        {
            emplacementEquipement[emplacementDisponible].EmplacementIcon = objetAjoute.Get2DTexture();
            emplacementEquipement[emplacementDisponible].TypeEmplacement = "BrasGauche";
            emplacementEquipement[emplacementDisponible].Pid_Item = objetAjoute.GetID();
            emplacementEquipement[emplacementDisponible].EstVide = false;
            emplacementEquipement[emplacementDisponible].EstSpecifique = true;

            
        }

        if (objetAjoute.GetItemType() == PlayerItemsManager.ItemType.ITEM_HELM)
        {
            emplacementEquipement[emplacementDisponible].EmplacementIcon = objetAjoute.Get2DTexture();
            emplacementEquipement[emplacementDisponible].TypeEmplacement = "Tete";
            emplacementEquipement[emplacementDisponible].Pid_Item = objetAjoute.GetID();
            emplacementEquipement[emplacementDisponible].EstVide = false;
            emplacementEquipement[emplacementDisponible].EstSpecifique = true;
        }

        if (objetAjoute.GetItemType() == PlayerItemsManager.ItemType.ITEM_SHOULDER)
        {
            emplacementEquipement[emplacementDisponible].EmplacementIcon = objetAjoute.Get2DTexture();
            emplacementEquipement[emplacementDisponible].TypeEmplacement = "Epaule";
            emplacementEquipement[emplacementDisponible].Pid_Item = objetAjoute.GetID();
            emplacementEquipement[emplacementDisponible].EstVide = false;
            emplacementEquipement[emplacementDisponible].EstSpecifique = true;
        }

        if (objetAjoute.GetItemType() == PlayerItemsManager.ItemType.ITEM_HEALTHPOTION)
        {
            emplacementEquipement[emplacementDisponible].EmplacementIcon = objetAjoute.Get2DTexture();
            emplacementEquipement[emplacementDisponible].TypeEmplacement = "HEALTHPOTION";
            emplacementEquipement[emplacementDisponible].Pid_Item = objetAjoute.GetID();
            emplacementEquipement[emplacementDisponible].EstVide = false;
            emplacementEquipement[emplacementDisponible].EstSpecifique = false;
        }

        if (objetAjoute.GetItemType() == PlayerItemsManager.ItemType.ITEM_MANAPOTION)
        {
            emplacementEquipement[emplacementDisponible].EmplacementIcon = objetAjoute.Get2DTexture();
            emplacementEquipement[emplacementDisponible].TypeEmplacement = "MANAPOTION";
            emplacementEquipement[emplacementDisponible].Pid_Item = objetAjoute.GetID();
            emplacementEquipement[emplacementDisponible].EstVide = false;
            emplacementEquipement[emplacementDisponible].EstSpecifique = false;
        }

        /*script supplémentaire au besoin
        if (objetAjoute.GetItemType() == PlayerItemsManager.ItemType.ITEM_QUEST)
        {
            emplacementEquipement[emplacementDisponible].EmplacementIcon = objetAjoute.Get2DTexture();
            emplacementEquipement[emplacementDisponible].TypeEmplacement = "ItemQuest";
            emplacementEquipement[emplacementDisponible].Pid_Item = objetAjoute.GetID();
            emplacementEquipement[emplacementDisponible].EstVide = false;
            emplacementEquipement[emplacementDisponible].EstSpecifique = false; 
        }
        */
    }


    //méthose Awake de Unity
    void Awake()
    {
        for (int i = 0; i < __NB_SLOTS_IN_INVENTORY__; ++i)
        {
            emplacementEquipement[i] = new EmplacementEquipement();
            emplacementEquipement[i].EmplacementIcon = emplacementVide;
            emplacementEquipement[i].TypeEmplacement = "Objet";
            emplacementEquipement[i].EstSpecifique = false;
        }

        //initialisation des emplacements
        botte.EmplacementIcon = botteIcon;
        botte.TypeEmplacement = "Botte";
        botte.EstSpecifique = true;

        Torse.EmplacementIcon = TorseIcon;
        Torse.TypeEmplacement = "Torse";
        Torse.EstSpecifique = true;

        BrasGauche.EmplacementIcon = BrasGaucheIcon;
        BrasGauche.TypeEmplacement = "BrasGauche";
        BrasGauche.EstSpecifique = true;

        ArmeDroite.EmplacementIcon = ArmeDroiteIcon;
        ArmeDroite.TypeEmplacement = "ArmeDroite";
        ArmeDroite.EstSpecifique = true;

        Tete.EmplacementIcon = TeteIcon;
        Tete.TypeEmplacement = "Tete";
        Tete.EstSpecifique = true;

        Epaule.EmplacementIcon = EpauleIcon;
        Epaule.TypeEmplacement = "Epaule";
        Epaule.EstSpecifique = true;

    }
	
	
	//callback de la fenetre d'achat pour enlever l'icone de l'inventaire
	public void CallDeleteItemFromTradeWindow(int pi_ID)
	{
		for (int i = 0; i < __NB_SLOTS_IN_INVENTORY__; ++i)
		{
			if(null != emplacementEquipement[i])
			{
				if(emplacementEquipement[i].Pid_Item == pi_ID)
				{
		            emplacementEquipement[i].EmplacementIcon = emplacementVide;
		            emplacementEquipement[i].TypeEmplacement = "Objet";
					emplacementEquipement[i].EstVide         = true;
		            emplacementEquipement[i].EstSpecifique   = false;		
					break;
				}
			}
		}
	}

    void Start()
    {
        //limiter le nombre d'objet dans la liste du manager
        ManagersTable.s_GetPlayerItemsManager().SetMaxNbItemsPlayerCanHold(__NB_SLOTS_IN_INVENTORY__); //NE PAS INCLURE LES SLOTS D'EQUIP                                 

        m_playerItemManager = GameObject.Find("PlayerItemsManager").gameObject.GetComponent<PlayerItemsManager>();

        avertissementRequisNTB = false;
    }


    void Update()
    {
		if(m_AssignationDone)
		{
	        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
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
    }
	
	//fermer l'inventaire pour changement de niveau
	public void CloseInventoryWindowForLevelChange()
	{
		m_AssignationDone    = false;
		estOuvert            = false;
		interface_camera     = null;
	}
	
	private bool   m_AssignationDone = false;
	
	/// <summary>
	/// Sets the interface camera.
	/// </summary>
	/// <param name='pi_Cam'>
	/// Pi_ cam.
	/// </param>
	public void SetInterfaceCamera(Camera   pi_Cam)
	{
		interface_camera  = pi_Cam;
		m_AssignationDone = true;
	}
	
	
	IEnumerator RoutineDClick()
	{
		m_RoutineDClickStarted = true;
		yield return new WaitForSeconds(0.8f);
		m_RoutineDClickStarted = false;
	}

    private void unequip()
    {
        switch (equipementDynamique.Type)
        {
            case "ArmeDroite":
                    ArmeDroite.EstVide = true;
                    ArmeDroite.EmplacementIcon = ArmeDroiteIcon;
                    ArmeDroite.Pid_Item = -1;
                    m_playerItemManager.UnequipRWeapon();
                break;

            case "Botte":
                    botte.EstVide = true;
                    botte.EmplacementIcon = botteIcon;
                    botte.Pid_Item = -1;
                    m_playerItemManager.UnequipBoots();
                break;

            case "Torse":
                Torse.EstVide = true;
                Torse.EmplacementIcon = TorseIcon;
                Torse.Pid_Item = -1;
                m_playerItemManager.UnequipChest();
                break;

            case "BrasGauche":
                BrasGauche.EstVide = true;
                BrasGauche.EmplacementIcon = TorseIcon;
                BrasGauche.Pid_Item = -1;
                m_playerItemManager.UnequipLShield();
                break;

            case "Tete":
                Tete.EstVide = true;
                Tete.EmplacementIcon = TeteIcon;
                Tete.Pid_Item = -1;
                m_playerItemManager.UnequipHelm();
                break;

            case "Epaule":
                Epaule.EstVide = true;
                Epaule.EmplacementIcon = EpauleIcon;
                Epaule.Pid_Item = -1;
                m_playerItemManager.UnequipShoulder();
                break;
        }
    }

    private void equip()
    {
        switch (equipementDynamique.Type)
        {
            case "ArmeDroite":
                ArmeDroite.EstVide = false;
                ArmeDroite.EmplacementIcon = equipementDynamique.Icon;
                ArmeDroite.Pid_Item = equipementDynamique.Pid_Item;
                m_playerItemManager.EquipRWeapon(equipementDynamique.Pid_Item);
                break;

            case "Botte":
                botte.EstVide = false;
                botte.EmplacementIcon = equipementDynamique.Icon;
                botte.Pid_Item = equipementDynamique.Pid_Item;
                m_playerItemManager.EquipBoots(equipementDynamique.Pid_Item);
                break;

            case "Torse":
                Torse.EstVide = false;
                Torse.EmplacementIcon = equipementDynamique.Icon;
                Torse.Pid_Item = equipementDynamique.Pid_Item;
                m_playerItemManager.EquipChest(equipementDynamique.Pid_Item);
                break;

            case "BrasGauche":
                BrasGauche.EstVide = false;
                BrasGauche.EmplacementIcon = equipementDynamique.Icon;
                BrasGauche.Pid_Item = equipementDynamique.Pid_Item;
                m_playerItemManager.EquipLShield(equipementDynamique.Pid_Item);
                break;

            case "Tete":
                Tete.EstVide = false;
                Tete.EmplacementIcon = equipementDynamique.Icon;
                Tete.Pid_Item = equipementDynamique.Pid_Item;
                m_playerItemManager.EquipHelm(equipementDynamique.Pid_Item);
                break;

            case "Epaule":
                Epaule.EstVide = false;
                Epaule.EmplacementIcon = equipementDynamique.Icon;
                Epaule.Pid_Item = equipementDynamique.Pid_Item;
                m_playerItemManager.EquipShoulder(equipementDynamique.Pid_Item);
                break;
        }        
    }
}
