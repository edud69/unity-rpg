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
using System.Collections.Generic;
using System;

/// <summary>
/// Player health manager.
/// </summary>
public class PlayerItemsManager : MonoBehaviour 
{	
	private FenetreInventaire m_FenetreInventaire;
	
	private int	  m_PlayerGold 			  	  = 0;
	private int	  m_NbItemsHoldedByPlayer 	  = 0;
	private int	  m_NbItemsMaxPlayerCanHold   = 0;
	
	private int m_RefCountID = -1;
	private int m_ChestId    = -1;
	private int m_HelmId     = -1;
	private int m_BootsId    = -1;
	private int m_RWeaponId  = -1;
	private int m_LShieldId  = -1;
	private int m_ShoulderId = -1;
	
	private List<ItemObject>	m_ItemsList;
	
	private static PlayerItemsManager  Instance;  //Since we use dontdestroyonload <-- we MUST have ONLY ONE instance
	
	
	public int GetChestID()    { return m_ChestId;    }
	public int GetHelmID()     { return m_HelmId;     }
	public int GetBootsID()    { return m_BootsId;    }
	public int GetRWeaponID()  { return m_RWeaponId;  }
	public int GetLShieldID()  { return m_LShieldId;  }
	public int GetShoulderID() { return m_ShoulderId; }
	
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	private void Start()
	{
		m_FenetreInventaire = this.GetComponent<FenetreInventaire>();
		m_ItemsList 		= new List<ItemObject>();
	}
	
	/// <summary>
	/// Sets the fenetre inventaire.
	/// </summary>
	/// <param name='pi_FenetreInventaire'>
	/// Pi_ fenetre inventaire.
	/// </param>
	public void SetFenetreInventaire	(FenetreInventaire      pi_FenetreInventaire)
	{
		m_FenetreInventaire = pi_FenetreInventaire;
	}
	
	
/////////////////////////////////////////////////////
	//ITEMS SECTION
/////////////////////////////////////////////////////
	
	/// <summary>
	/// Gets the player max items.
	/// </summary>
	public int			GetPlayerMaxItems			()
	{
		return m_NbItemsMaxPlayerCanHold;
	}
	
	/// <summary>
	/// Gets the player nb items.
	/// </summary>
	public int			GetPlayerNbItems			()
	{
		return m_NbItemsHoldedByPlayer;
	}
	
	/// <summary>
	/// Gets the items list.
	/// </summary>
	/// <returns>
	/// The items list.
	/// </returns>
	public List<ItemObject>  GetItemsList()
	{
		return m_ItemsList;
	}
	
	/// <summary>
	/// Incs the nb items holded by player.
	/// </summary>
	private void 		IncNbItemsHoldedByPlayer	()
	{
		++m_NbItemsHoldedByPlayer;
		
		if(m_NbItemsHoldedByPlayer > m_NbItemsMaxPlayerCanHold)
		{
			Debug.LogError("ASSERT FAILURE: Le joueur ne peut avoir plus d'items que son max attribue!");
		}
	}
	
	/// <summary>
	/// Decs the nb items holded by player.
	/// </summary>
	private void 		DecNbItemsHoldedByPlayer	()
	{
		--m_NbItemsHoldedByPlayer;
		
		if(0 > m_NbItemsHoldedByPlayer)
		{
			Debug.LogError("ASSERT FAILURE: Le joueur ne peut avoir un nombre negatif d'items!");
		}
	}
	
	
	/// <summary>
	/// Sets the max nb items player can hold.
	/// </summary>
	/// <param name='pi_NbItems'>
	/// Pi_ nb items.
	/// </param>
	public void 		SetMaxNbItemsPlayerCanHold	(int 	pi_NbItems)
	{
		m_NbItemsMaxPlayerCanHold = pi_NbItems;
		
		if(m_NbItemsMaxPlayerCanHold < 0)
		{
			Debug.LogError("ASSERT FAILURE: Valeur negative impossible!");
		}
	}
	
	/// <summary>
	/// Gets the player items list.
	/// </summary>
	public ItemObject 	GetPlayerItem				(int 	pi_itemId)
	{
		if(0 > pi_itemId)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}
		
		for(int i = 0; i < m_ItemsList.Count; ++i)
		{
			if(pi_itemId == m_ItemsList[i].GetID())
			{
				return m_ItemsList[i];
			}
		}
		
		Debug.LogError("ASSERT FAILURE: Index invalide!");
		return null;
	}

	
	
	/// <summary>
	/// Equips the helm.
	/// </summary>
	/// <param name='pi_Id'>
	/// Pi_ identifier.
	/// </param>
	public void 	EquipHelm			(int pi_Id)
	{
		if(0 > pi_Id)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}
		
		bool l_Found = false;
		int i;
		for(i = 0; i < m_ItemsList.Count; ++i)
		{
			if(pi_Id == m_ItemsList[i].GetID())
			{
				l_Found = true;
				break;
			}
		}
		
		if(!l_Found)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}
			
		if(pi_Id == m_HelmId || m_ItemsList[i].GetItemType() != PlayerItemsManager.ItemType.ITEM_HELM)
		{
			Debug.LogError("ASSERT FAILURE: Deja assigne ou  mauvais type!!!");
		}
		
		if(m_ItemsList[i].GetReqLvl() > ManagersTable.s_GetPlayerXPManager().GetPlayerLevel())
		{
			Debug.LogError("ASSERT FAILURE: Level trop bas pour le port de l'arme!!!");
		}
		
		GiveAttributesToPlayer(m_ItemsList[i]);
		audio.PlayOneShot((AudioClip)Resources.Load("Sounds/Items/pickupItem", typeof(AudioClip)));
		m_HelmId = i;
		DecNbItemsHoldedByPlayer();		
	}
	
	/// <summary>
	/// Equips the chest.
	/// </summary>
	/// <param name='pi_Id'>
	/// Pi_ identifier.
	/// </param>
	public void 	EquipChest			(int pi_Id)
	{
		if(0 > pi_Id)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}
		
		bool l_Found = false;
		int i;
		for(i = 0; i < m_ItemsList.Count; ++i)
		{
			if(pi_Id == m_ItemsList[i].GetID())
			{
				l_Found = true;
				break;
			}
		}
		
		if(!l_Found)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}
		
		if(pi_Id == m_ChestId || m_ItemsList[i].GetItemType() != PlayerItemsManager.ItemType.ITEM_CHEST)
		{
			Debug.LogError("ASSERT FAILURE: Deja assigne ou  mauvais type!!!");
		}
		
		if(m_ItemsList[i].GetReqLvl() > ManagersTable.s_GetPlayerXPManager().GetPlayerLevel())
		{
			Debug.LogError("ASSERT FAILURE: Level trop bas pour le port de l'arme!!!");
		}
		
		GiveAttributesToPlayer(m_ItemsList[i]);
		audio.PlayOneShot((AudioClip)Resources.Load("Sounds/Items/pickupItem", typeof(AudioClip)));
		m_ChestId = pi_Id;
		DecNbItemsHoldedByPlayer();
	}
	
	/// <summary>
	/// Equips the boots.
	/// </summary>
	/// <param name='pi_Id'>
	/// Pi_ identifier.
	/// </param>	
	public void 	EquipBoots			(int pi_Id)
	{
		if(0 > pi_Id)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}
		
		bool l_Found = false;
		int i;
		for(i = 0; i < m_ItemsList.Count; ++i)
		{
			if(pi_Id == m_ItemsList[i].GetID())
			{
				l_Found = true;
				break;
			}
		}
		
		if(!l_Found)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}
		
		if(pi_Id == m_BootsId || m_ItemsList[i].GetItemType() != PlayerItemsManager.ItemType.ITEM_BOOT)
		{
			Debug.LogError("ASSERT FAILURE: Deja assigne ou  mauvais type!!!");
		}
		
		if(m_ItemsList[i].GetReqLvl() > ManagersTable.s_GetPlayerXPManager().GetPlayerLevel())
		{
			Debug.LogError("ASSERT FAILURE: Level trop bas pour le port de l'arme!!!");
		}
		
		GiveAttributesToPlayer(m_ItemsList[i]);
		audio.PlayOneShot((AudioClip)Resources.Load("Sounds/Items/pickupItem", typeof(AudioClip)));
		m_BootsId = pi_Id;
		DecNbItemsHoldedByPlayer();
	}
	
	/// <summary>
	/// Equips the L shield.
	/// </summary>
	/// <param name='pi_Id'>
	/// Pi_ identifier.
	/// </param>
	public void 	EquipLShield		(int pi_Id)
	{
		if(0 > pi_Id)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}
		
		bool l_Found = false;
		int i;
		for(i = 0; i < m_ItemsList.Count; ++i)
		{
			if(pi_Id == m_ItemsList[i].GetID())
			{
				l_Found = true;
				break;
			}
		}
		
		if(!l_Found)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}

		if(pi_Id == m_LShieldId || m_ItemsList[i].GetItemType() != PlayerItemsManager.ItemType.ITEM_LHAND)
		{
			Debug.LogError("ASSERT FAILURE: Deja assigne ou  mauvais type!!!");
		}
		
		if(m_ItemsList[i].GetReqLvl() > ManagersTable.s_GetPlayerXPManager().GetPlayerLevel())
		{
			Debug.LogError("ASSERT FAILURE: Level trop bas pour le port de l'arme!!!");
		}
		
		GiveAttributesToPlayer(m_ItemsList[i]);
		audio.PlayOneShot((AudioClip)Resources.Load("Sounds/Items/pickupItem", typeof(AudioClip)));
		m_LShieldId = pi_Id;
		DecNbItemsHoldedByPlayer();
	}
	
	/// <summary>
	/// Equips the R weapon.
	/// </summary>
	/// <param name='pi_Id'>
	/// Pi_ identifier.
	/// </param>
	public void 	EquipRWeapon		(int pi_Id)
	{
		if(0 > pi_Id)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}
		
		bool l_Found = false;
		int i;
		for(i = 0; i < m_ItemsList.Count; ++i)
		{
			if(pi_Id == m_ItemsList[i].GetID())
			{
				l_Found = true;
				break;
			}
		}
		
		if(!l_Found)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}
		
		if(pi_Id == m_RWeaponId || m_ItemsList[i].GetItemType() != PlayerItemsManager.ItemType.ITEM_RHAND)
		{
			Debug.LogError("ASSERT FAILURE: Deja assigne ou  mauvais type!!!");
		}
		
		if(m_ItemsList[i].GetReqLvl() > ManagersTable.s_GetPlayerXPManager().GetPlayerLevel())
		{
			Debug.LogError("ASSERT FAILURE: Level trop bas pour le port de l'arme!!!");
		}
		
		GiveAttributesToPlayer(m_ItemsList[i]);
		audio.PlayOneShot((AudioClip)Resources.Load("Sounds/Items/pickupItem", typeof(AudioClip)));
		m_RWeaponId = pi_Id;
		DecNbItemsHoldedByPlayer();
	}
	
	/// <summary>
	/// Equips the shoulder.
	/// </summary>
	/// <param name='pi_Id'>
	/// Pi_ identifier.
	/// </param>
	public void 	EquipShoulder		(int pi_Id)
	{
		if(0 > pi_Id)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}
		
		bool l_Found = false;
		int i;
		for(i = 0; i < m_ItemsList.Count; ++i)
		{
			if(pi_Id == m_ItemsList[i].GetID())
			{
				l_Found = true;
				break;
			}
		}
		
		if(!l_Found)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}
		
		if(pi_Id == m_ShoulderId || m_ItemsList[i].GetItemType() != PlayerItemsManager.ItemType.ITEM_SHOULDER)
		{
			Debug.LogError("ASSERT FAILURE: Deja assigne ou  mauvais type!!!");
		}
		
		if(m_ItemsList[i].GetReqLvl() > ManagersTable.s_GetPlayerXPManager().GetPlayerLevel())
		{
			Debug.LogError("ASSERT FAILURE: Level trop bas pour le port de l'arme!!!");
		}
		
		GiveAttributesToPlayer(m_ItemsList[i]);
		audio.PlayOneShot((AudioClip)Resources.Load("Sounds/Items/pickupItem", typeof(AudioClip)));
		m_ShoulderId = pi_Id;
		DecNbItemsHoldedByPlayer();
	}
	
	
	/// <summary>
	/// Gives the attributes to player.
	/// </summary>
	/// <param name='pi_Id'>
	/// Pi_ identifier.
	/// </param>
	private void GiveAttributesToPlayer(ItemObject   pi_Item)
	{
		Attribute pAttributes = (Attribute)ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<Attribute>();
        
		if(pi_Item.GetItemType() != PlayerItemsManager.ItemType.ITEM_RHAND)
		{
			float l_Def = pi_Item.GetDefense();
			if(l_Def > 0)
			{
				ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Vous recevez un bonus de " + l_Def + " DEF.", PlayerMsgSystemManager.MsgType.MSG_BONUS_STATS);
				pAttributes.defence += l_Def;
			}
		}

		float l_Spirit = pi_Item.GetSpirit();
		if(l_Spirit > 0)
		{
			ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Vous recevez un bonus de " + l_Spirit + " SPIRIT.", PlayerMsgSystemManager.MsgType.MSG_BONUS_STATS);
			pAttributes.addBonusMana(l_Spirit);
		}
		
		float l_Agility = pi_Item.GetAgility();
		if(l_Agility > 0)
		{
			ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Vous recevez un bonus de " + l_Agility + " AGILITY.", PlayerMsgSystemManager.MsgType.MSG_BONUS_STATS);
			pAttributes.agility += l_Agility;
			
		}
		
		float l_Strenght = pi_Item.GetStrenght();
		if(l_Strenght > 0)
		{
			ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Vous recevez un bonus de " + l_Strenght + " STR.", PlayerMsgSystemManager.MsgType.MSG_BONUS_STATS);
			pAttributes.strenght += l_Strenght;
		}
	}
	
	/// <summary>
	/// Ons the drop.
	/// </summary>
	/// <param name='position'>
	/// Position.
	/// </param>
	public void onDrop(Vector3 position) 
	{		
		GameObject myObject;
		int amountToDrop = 0;
		float random = UnityEngine.Random.Range(0f,1f);		
		int lvl = ManagersTable.s_GetPlayerXPManager().GetPlayerLevel();	
		
		
		if (random <= 0.7f) {						
			float strenght = UnityEngine.Random.Range(0f,6f) + lvl;
			float agility  = UnityEngine.Random.Range(0f,6f) + lvl;
			float spirit   = UnityEngine.Random.Range(0f,6f) + lvl;
			float defence  = UnityEngine.Random.Range(0f,6f) + lvl;			
			
			strenght = (float)Math.Round (strenght);
			agility  = (float)Math.Round (agility);
			spirit   = (float)Math.Round (spirit);
			defence  = (float)Math.Round (defence);	
			
			random = UnityEngine.Random.Range(0f,1f);
			if(random <= 0.08f) {//boots				
				myObject = (GameObject) Instantiate(GameObject.Find("BasicArmorBoots"));				
				CreateBootsArmor attributes = myObject.GetComponent<CreateBootsArmor>();
				
				attributes.m_BonusAgility = agility;
				attributes.m_BonusSpirit = spirit;
				attributes.m_BonusStr = strenght;
				attributes.m_Defense = defence;
				attributes.m_ReqLvl = lvl;
				attributes.m_SellValue = (int)(strenght + agility + spirit + defence)*2;				
			}
			else if(random <= 0.16f) {//chest					
				myObject = (GameObject) Instantiate(GameObject.Find("BasicArmorChest"));				
				CreateChestArmor attributes = myObject.GetComponent<CreateChestArmor>();
				
				attributes.m_BonusAgility = agility;
				attributes.m_BonusSpirit = spirit;
				attributes.m_BonusStr = strenght;
				attributes.m_Defense = defence;
				attributes.m_ReqLvl = lvl;
				attributes.m_SellValue = (int)(strenght + agility + spirit + defence)*2;			
			}
			else if(random <= 0.24f) {//helm							
				myObject = (GameObject) Instantiate(GameObject.Find("BasicArmorHelm"));				
				CreateHelmArmor attributes = myObject.GetComponent<CreateHelmArmor>();
				
				attributes.m_BonusAgility = agility;
				attributes.m_BonusSpirit = spirit;
				attributes.m_BonusStr = strenght;
				attributes.m_Defense = defence;
				attributes.m_ReqLvl = lvl;
				attributes.m_SellValue = (int)(strenght + agility + spirit + defence)*2;	
			}
			else if(random <= 0.32f) {//shoulder				
				myObject = (GameObject) Instantiate(GameObject.Find("BasicArmorShoulder"));				
				CreateShoulderArmor attributes = myObject.GetComponent<CreateShoulderArmor>();
				
				attributes.m_BonusAgility = agility;
				attributes.m_BonusSpirit = spirit;
				attributes.m_BonusStr = strenght;
				attributes.m_Defense = defence;
				attributes.m_ReqLvl = lvl;
				attributes.m_SellValue = (int)(strenght + agility + spirit + defence)*2;	
			}
			else if(random <= 0.4f) {//shield				
				myObject = (GameObject) Instantiate(GameObject.Find("BasicShield"));
				
				CreateShield attributes = myObject.GetComponent<CreateShield>();
				attributes.m_BonusAgility = agility;
				attributes.m_BonusSpirit = spirit;
				attributes.m_BonusStr = strenght;
				attributes.m_Defense = defence;
				attributes.m_ReqLvl = lvl;
				attributes.m_SellValue = (int)(strenght + agility + spirit + defence)*2;	
			}
			else if(random <= 0.48f){//sword				
				myObject = (GameObject) Instantiate(GameObject.Find("BasicSword"));				
				CreateWeapon attributes = myObject.GetComponent<CreateWeapon>();
				
				attributes.m_BonusAgility = agility;
				attributes.m_BonusSpirit = spirit;
				attributes.m_BonusStr = strenght;				
				attributes.m_ReqLvl = lvl;
				attributes.m_SellValue = (int)(strenght + agility + spirit + defence)*2;	
			}
			else if(random <= 0.75f) {//health potion				
				 myObject = (GameObject) Instantiate(GameObject.Find("HealthPotion"));				
			}
			else {//mana potion				
				myObject = (GameObject) Instantiate(GameObject.Find("ManaPotion"));
			}
			
			myObject.transform.position = position;			
		}
	}
	
	/// <summary>
	/// Removes the attributes from player.
	/// </summary>
	/// <param name='pi_Id'>
	/// Pi_ identifier.
	/// </param>
	private void RemoveAttributesFromPlayer(int   pi_Id)
	{
		int  i;
		bool l_Found = false;
		for(i = 0 ; i < m_ItemsList.Count; ++i)
		{
			if(pi_Id == m_ItemsList[i].GetID())
			{
				l_Found = true;
				break;
			}
		}
		
		if(!l_Found)
		{
			Debug.LogError("ASSERT FAILURE: Invalid Index");
		}
		
		Attribute pAttributes = (Attribute)ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<Attribute>();
        
		if(m_ItemsList[i].GetItemType() != PlayerItemsManager.ItemType.ITEM_RHAND)
		{
			float l_Def = m_ItemsList[i].GetDefense();
			if(l_Def > 0)
			{
				ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Vous perdez un bonus de " + l_Def + " DEF.", PlayerMsgSystemManager.MsgType.MSG_BONUS_STATS);
				pAttributes.defence -= l_Def;
			}
		}
		
		float l_Spirit = m_ItemsList[i].GetSpirit();
		if(l_Spirit > 0)
		{
			ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Vous perdez un bonus de " + l_Spirit + " SPIRIT.", PlayerMsgSystemManager.MsgType.MSG_BONUS_STATS);
			pAttributes.removeBonusMana(l_Spirit);
		}
		
		float l_Agility = m_ItemsList[i].GetAgility();
		if(l_Agility > 0)
		{
			ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Vous perdez un bonus de " + l_Agility + " AGILITY.", PlayerMsgSystemManager.MsgType.MSG_BONUS_STATS);
			pAttributes.agility -= l_Agility;
		}
		
		float l_Strenght = m_ItemsList[i].GetStrenght();
		if(l_Strenght > 0)
		{
			ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Vous perdez un bonus de " + l_Strenght + " STR.", PlayerMsgSystemManager.MsgType.MSG_BONUS_STATS);
			pAttributes.strenght -= l_Strenght;
		}
	}
	
	/// <summary>
	/// Validations the item level valide.
	/// </summary>
	/// <returns>
	/// The item level valide.
	/// </returns>
	/// <param name='pi_Id'>
	/// If set to <c>true</c> pi_ identifier.
	/// </param>
    public bool validationItemLevelValide(int pi_Id)
    {
        bool l_Found = false;
        int i;

        for (i = 0; i < m_ItemsList.Count; ++i)
        {
            if (pi_Id == m_ItemsList[i].GetID())
            {
                l_Found = true;
                break;
            }
        }

        if (!l_Found)
        {
            Debug.LogError("ASSERT FAILURE: Index invalide!");
        }
		
		if(m_ItemsList[i].GetReqLvl() <= ManagersTable.s_GetPlayerXPManager().GetPlayerLevel())
		{
			return true;
		}
		else
		{
			ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Vous devez etre lvl " + m_ItemsList[i].GetReqLvl() + " pour le port.", PlayerMsgSystemManager.MsgType.MSG_DIALOG);
		}
		
        return false;
    }

	
	public void 	UnequipHelm		()  			{ RemoveAttributesFromPlayer(m_HelmId);
													  IncNbItemsHoldedByPlayer(); 		audio.PlayOneShot((AudioClip)Resources.Load("Sounds/Items/pickupItem", typeof(AudioClip)));
													  m_HelmId  	  = -1;}
	
	public void 	UnequipChest	()  			{ RemoveAttributesFromPlayer(m_ChestId);
													  IncNbItemsHoldedByPlayer();		audio.PlayOneShot((AudioClip)Resources.Load("Sounds/Items/pickupItem", typeof(AudioClip)));
													  m_ChestId 	  = -1;}
	
	public void 	UnequipBoots	()  			{ RemoveAttributesFromPlayer(m_BootsId);
													  IncNbItemsHoldedByPlayer();		audio.PlayOneShot((AudioClip)Resources.Load("Sounds/Items/pickupItem", typeof(AudioClip)));
													  m_BootsId 	  = -1;}
	
	public void 	UnequipLShield	()  			{ RemoveAttributesFromPlayer(m_LShieldId);
													  IncNbItemsHoldedByPlayer();		audio.PlayOneShot((AudioClip)Resources.Load("Sounds/Items/pickupItem", typeof(AudioClip)));
													  m_LShieldId  = -1;}
	
	public void 	UnequipRWeapon  ()  			{ RemoveAttributesFromPlayer(m_RWeaponId);
													  IncNbItemsHoldedByPlayer();		audio.PlayOneShot((AudioClip)Resources.Load("Sounds/Items/pickupItem", typeof(AudioClip)));
													  m_RWeaponId  = -1;}
	
	public void 	UnequipShoulder ()  			{ RemoveAttributesFromPlayer(m_ShoulderId);
													  IncNbItemsHoldedByPlayer();		audio.PlayOneShot((AudioClip)Resources.Load("Sounds/Items/pickupItem", typeof(AudioClip)));
													  m_ShoulderId = -1;}
	
	
	/// <summary>
	/// Gets the helm.
	/// </summary>
	/// <returns>
	/// The helm.
	/// </returns>
	public ItemObject 		GetHelm			()
	{
		if(m_HelmId == -1)
		{
			return null;
		}
		
		bool l_Found = false;
		int i;
		for(i = 0; i < m_ItemsList.Count; ++i)
		{
			if(m_HelmId == m_ItemsList[i].GetID())
			{
				l_Found = true;
				break;
			}
		}
		
		if(!l_Found)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}
		
		return m_ItemsList[i];
	}
	
	/// <summary>
	/// Gets the boots.
	/// </summary>
	/// <returns>
	/// The boots.
	/// </returns>
	public ItemObject 		GetBoots		()
	{
		if(m_BootsId == -1)
		{
			return null;
		}
		
		bool l_Found = false;
		int i;
		for(i = 0; i < m_ItemsList.Count; ++i)
		{
			if(m_BootsId == m_ItemsList[i].GetID())
			{
				l_Found = true;
				break;
			}
		}
		
		if(!l_Found)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}
		
		return m_ItemsList[i];
	}

	/// <summary>
	/// Gets the chest.
	/// </summary>
	/// <returns>
	/// The chest.
	/// </returns>
	public ItemObject 		GetChest		()
	{
		if(m_ChestId == -1)
		{
			return null;
		}
		
		bool l_Found = false;
		int i;
		for(i = 0; i < m_ItemsList.Count; ++i)
		{
			if(m_ChestId == m_ItemsList[i].GetID())
			{
				l_Found = true;
				break;
			}
		}
		
		if(!l_Found)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}
		
		return m_ItemsList[i];
	}
	
	/// <summary>
	/// Gets the R hand weapon.
	/// </summary>
	/// <returns>
	/// The R hand weapon.
	/// </returns>
	public ItemObject 		GetRHandWeapon	()
	{
		if(m_RWeaponId == -1)
		{
			return null;
		}
		
		bool l_Found = false;
		int i;
		for(i = 0; i < m_ItemsList.Count; ++i)
		{
			if(m_RWeaponId == m_ItemsList[i].GetID())
			{
				l_Found = true;
				break;
			}
		}
		
		if(!l_Found)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}
		
		return m_ItemsList[i];
	}
	
	/// <summary>
	/// Gets the L hand shield.
	/// </summary>
	/// <returns>
	/// The L hand shield.
	/// </returns>
	public ItemObject 		GetLHandShield	()
	{
		if(m_LShieldId == -1)
		{
			return null;
		}
		
		bool l_Found = false;
		int i;
		for(i = 0; i < m_ItemsList.Count; ++i)
		{
			if(m_LShieldId == m_ItemsList[i].GetID())
			{
				l_Found = true;
				break;
			}
		}
		
		if(!l_Found)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}
		
		return m_ItemsList[i];
	}
	
	/// <summary>
	/// Gets the shoulder.
	/// </summary>
	/// <returns>
	/// The shoulder.
	/// </returns>
	public ItemObject 		GetShoulder		()
	{
		if(m_ShoulderId == -1)
		{
			return null;
		}
		
		bool l_Found = false;
		int i;
		for(i = 0; i < m_ItemsList.Count; ++i)
		{
			if(m_ShoulderId == m_ItemsList[i].GetID())
			{
				l_Found = true;
				break;
			}
		}
		
		if(!l_Found)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}
		
		return m_ItemsList[i];
	}
	
	
	
	/// <summary>
	/// Trashs the item.
	/// </summary>
	/// <param name='pi_Id'>
	/// Pi_ identifier.
	/// </param>
	public void 			TrashItem				(int 		 	 pi_Id,     bool pi_FromTradeWindow)
	{
		if(0 > pi_Id)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}
		
		bool l_Found = false;
		int i;
		for(i = 0; i < m_ItemsList.Count; ++i)
		{
			if(pi_Id == m_ItemsList[i].GetID())
			{
				l_Found = true;
				break;
			}
		}
		
		if(!l_Found)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}
		
		m_ItemsList.RemoveAt(i);
		
		if(m_ChestId == pi_Id)
			m_ChestId = -1;
		
		if(m_HelmId == pi_Id)
			m_HelmId = -1;
		
		if(m_BootsId == pi_Id)
			m_BootsId = -1;
		
		if(m_LShieldId == pi_Id)
			m_LShieldId = -1;
		
		if(m_RWeaponId == pi_Id)
			m_RWeaponId = -1;
		
		if(m_ShoulderId == pi_Id)
			m_ShoulderId = -1;
		
		if(!audio.isPlaying || pi_FromTradeWindow)
		{
			ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Vous avez detruit l'item.", PlayerMsgSystemManager.MsgType.MSG_DIALOG);
			audio.clip = (AudioClip)Resources.Load("Sounds/Items/trashItem", typeof(AudioClip));
			audio.Play();
		}
		
		DecNbItemsHoldedByPlayer();
	}
	
	/// <summary>
	/// Pickups the new item.
	/// </summary>
	/// <param name='pi_Object'>
	/// Pi_ object.
	/// </param>
	public bool				PickupNewItem			(ref ItemObject  	pi_Object)
	{
		if(m_NbItemsHoldedByPlayer + 1 <= m_NbItemsMaxPlayerCanHold)
		{	
			m_ItemsList.Add(pi_Object);
			m_ItemsList[m_ItemsList.Count - 1].SetItemObjectId(++m_RefCountID);
            m_FenetreInventaire.ajoutInventaire(pi_Object);
			IncNbItemsHoldedByPlayer();
			
			return true;
		}
		
		return false;
	}
	
	/// Drinks the health potion.
	/// </summary>
	/// <param name='pi_id'>
	/// Pi_id.
	/// </param>
	public void DrinkHealthPotion(int pi_id)
	{
		if(0 > pi_id)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}
		
		bool l_Found = false;
		int i;
		for(i = 0; i < m_ItemsList.Count; ++i)
		{
			if(pi_id == m_ItemsList[i].GetID())
			{
				l_Found = true;
				break;
			}
		}
		
		if(!l_Found)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}
		
		if(GetPlayerItem(pi_id).GetItemType() != ItemType.ITEM_HEALTHPOTION)
		{
			Debug.LogError("ASSERT FAILURE: Ce n'est pas une potion de vie!");
		}
		
		
		audio.clip = (AudioClip)Resources.Load("Sounds/Items/potionDrink", typeof(AudioClip));
		audio.Play();
		
		float l_RestoreHP = GetPlayerItem(pi_id).GetHPorMPBoost();
		ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<Health>().CurrentHealth += l_RestoreHP;
		ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Restauration de " + l_RestoreHP + " PTS de sante.", PlayerMsgSystemManager.MsgType.MSG_HEAL_OR_MANA_REGEN);
		TrashItem(pi_id, false);
	}

	/// <summary>
	/// Drinks the mana potion.
	/// </summary>
	/// <param name='pi_Id'>
	/// Pi_ identifier.
	/// </param>
	public void DrinkManaPotion(int pi_id)
	{
		if(0 > pi_id)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}
		
		bool l_Found = false;
		int i;
		for(i = 0; i < m_ItemsList.Count; ++i)
		{
			if(pi_id == m_ItemsList[i].GetID())
			{
				l_Found = true;
				break;
			}
		}
		
		if(!l_Found)
		{
			Debug.LogError("ASSERT FAILURE: Index invalide!");
		}
		
		if(GetPlayerItem(pi_id).GetItemType() != ItemType.ITEM_MANAPOTION)
		{
			Debug.LogError("ASSERT FAILURE: Ce n'est pas une potion de mana!");
		}
		
		audio.clip = (AudioClip)Resources.Load("Sounds/Items/potionDrink", typeof(AudioClip));
		audio.Play();
		
		float l_RestoreMP = GetPlayerItem(pi_id).GetHPorMPBoost();
		ManagersTable.s_GetPlayerPositionManager().GetPlayer().GetComponent<Health>().CurrentSpirit += l_RestoreMP;
		ManagersTable.s_GetPlayerMsgSysManager().AddMsgToLog("Restauration de " + l_RestoreMP + " PTS de mana.", PlayerMsgSystemManager.MsgType.MSG_HEAL_OR_MANA_REGEN);
		TrashItem(pi_id, false);
	}
	
/////////////////////////////////////////////////////
	//ITEM CLASS
/////////////////////////////////////////////////////
	public class ItemObject
	{
		/// <summary>
		/// Creates the potion.
		/// </summary>
		/// <returns>
		/// The potion.
		/// </returns>
		/// <param name='pi_ItemType'>
		/// Pi_ item type.
		/// </param>
		/// <param name='pi_2DTexture'>
		/// Pi_2 D texture.
		/// </param>
		/// <param name='pi_IncrementHPorMP'>
		/// Pi_ increment H por M.
		/// </param>
		/// <param name='pi_SellValue'>
		/// Pi_ sell value.
		/// </param>
		static public ItemObject 		CreatePotion			(ItemType			pi_ItemType,
						   		 	   				 	 		 Texture2D	    	pi_2DTexture,
						   		 	   				 	 		 float				pi_IncrementHPorMP,
						   		 	   				 	 		 int				pi_SellValue)
		{
			if(pi_ItemType != PlayerItemsManager.ItemType.ITEM_MANAPOTION && 
			   pi_ItemType != PlayerItemsManager.ItemType.ITEM_HEALTHPOTION || 
			   0 > pi_SellValue || 0 > pi_IncrementHPorMP)
			{
				Debug.LogError("ASSERT FAILURE: Mauvais arguments!");
			}
			
			return new ItemObject(pi_ItemType, pi_2DTexture, 0, pi_IncrementHPorMP, pi_SellValue, 0, 0, -1, -1, -1);
		}
		
		/// <summary>
		/// Creates the armor item.
		/// </summary>
		/// <returns>
		/// The armor item.
		/// </returns>
		/// <param name='pi_ItemType'>
		/// Pi_ item type.
		/// </param>
		/// <param name='pi_2DTexture'>
		/// Pi_2 D texture.
		/// </param>
		/// <param name='pi_AttackPower'>
		/// Pi_ attack power.
		/// </param>
		/// <param name='pi_SellValue'>
		/// Pi_ sell value.
		/// </param>
		static public ItemObject 		CreateRHandWeaponItem	(ItemType			pi_ItemType,
						   		 	   	  				 		 Texture2D	        pi_2DTexture,
						   		 	      				 		 float				pi_AttackPower,
						   		 	      				 		 int				pi_SellValue,
														 		 int				pi_ReqLvl,
						               							 float              pi_Strenght,
			               										 float              pi_Agility,
			               										 float              pi_Spirit)
		{		
			if(0 > pi_Strenght || 0 > pi_Agility || 0 > pi_Spirit)
			{
				Debug.LogError("ASSERT FAILURE: Probleme d'attributs!");	
			}
			
			if(pi_ItemType != PlayerItemsManager.ItemType.ITEM_RHAND || 
			   0 > pi_SellValue || 0 > pi_AttackPower || 1 > pi_ReqLvl)
			{
				Debug.LogError("ASSERT FAILURE: Mauvais type d'item!");
			}
			
			return new ItemObject(pi_ItemType, pi_2DTexture, pi_AttackPower, 0, pi_SellValue, 0, pi_ReqLvl, pi_Strenght, pi_Agility, pi_Spirit);
		}
		
		/// <summary>
		/// Creates the armor type item.
		/// </summary>
		/// <returns>
		/// The armor type item.
		/// </returns>
		/// <param name='pi_ItemType'>
		/// Pi_ item type.
		/// </param>
		/// <param name='pi_2DTexture'>
		/// Pi_2 D texture.
		/// </param>
		/// <param name='pi_SellValue'>
		/// Pi_ sell value.
		/// </param>
		/// <param name='pi_Defense'>
		/// Pi_ defense.
		/// </param>
		static public ItemObject 		CreateArmorTypeItem		(ItemType			pi_ItemType,
						   		 	   	  	  			 		 Texture2D	        pi_2DTexture,
						   		 	      	 	 		 		 int				pi_SellValue,
											  			 		 float				pi_Defense,
											  			 		 int				pi_ReqLvl,
						               							 float              pi_Strenght,
			               										 float              pi_Agility,
			               										 float              pi_Spirit)
		{
			if(0 > pi_Strenght || 0 > pi_Agility || 0 > pi_Spirit)
			{
				Debug.LogError("ASSERT FAILURE: Probleme d'attributs!");	
			}
			
			if(pi_ItemType != PlayerItemsManager.ItemType.ITEM_HELM && 
			   pi_ItemType != PlayerItemsManager.ItemType.ITEM_BOOT && 
			   pi_ItemType != PlayerItemsManager.ItemType.ITEM_LHAND && 
			   pi_ItemType != PlayerItemsManager.ItemType.ITEM_CHEST && 
			   pi_ItemType != PlayerItemsManager.ItemType.ITEM_SHOULDER &&
			   pi_ItemType != PlayerItemsManager.ItemType.ITEM_BOOT || 
			   0 > pi_SellValue || 0 > pi_Defense || 1 > pi_ReqLvl)
			{
				Debug.LogError("ASSERT FAILURE: Mauvais type d'item!");
			}
			
			return new ItemObject(pi_ItemType, pi_2DTexture, 0, 0, pi_SellValue, pi_Defense, pi_ReqLvl, pi_Strenght, pi_Agility, pi_Spirit);
		}
		
		
		/// <summary>
		/// Gets the sell value.
		/// </summary>
		/// <returns>
		/// The sell value.
		/// </returns>
		public int 			GetSellValue	()
		{
			return m_SellValue;
		}
		
		/// <summary>
		/// Gets the type of the item.
		/// </summary>
		/// <returns>
		/// The item type.
		/// </returns>
		public ItemType 	GetItemType		()
		{
			return m_ItemType;
		}
		
		/// <summary>
		/// Get2s the D texture.
		/// </summary>
		/// <returns>
		/// The D texture.
		/// </returns>
		public Texture2D 	Get2DTexture	()
		{
			return m_2DTexture;
		}
		
		/// <summary>
		/// Gets the protection.
		/// </summary>
		/// <returns>
		/// The protection.
		/// </returns>
		public float 			GetDefense		()
		{	
			if(m_ItemType != PlayerItemsManager.ItemType.ITEM_HELM && 
			   m_ItemType != PlayerItemsManager.ItemType.ITEM_BOOT && 
			   m_ItemType != PlayerItemsManager.ItemType.ITEM_LHAND && 
			   m_ItemType != PlayerItemsManager.ItemType.ITEM_CHEST && 
			   m_ItemType != PlayerItemsManager.ItemType.ITEM_SHOULDER &&
			   m_ItemType != PlayerItemsManager.ItemType.ITEM_BOOT)
			{
				Debug.LogError("ASSERT FAILURE: Ce n'est pas un type armure!");
			}
			
			return m_Defense;
		}
		
		/// <summary>
		/// Gets the attack power.
		/// </summary>
		/// <returns>
		/// The attack power.
		/// </returns>
		public float 			GetAttackPower	()
		{
			if(m_ItemType != PlayerItemsManager.ItemType.ITEM_RHAND)
			{
				Debug.LogError("ASSERT FAILURE: Ce n'est pas une arme!");
			}
			
			return m_AttackPower;
		}
		
		/// <summary>
		/// Gets the H por MP boost.
		/// </summary>
		/// <returns>
		/// The H por MP boost.
		/// </returns>
		public float 			GetHPorMPBoost	()
		{
			if(m_ItemType != PlayerItemsManager.ItemType.ITEM_MANAPOTION &&
			   m_ItemType != PlayerItemsManager.ItemType.ITEM_HEALTHPOTION)
			{
				Debug.LogError("ASSERT FAILURE: Ce n'est pas une potion!");
			}
			
			return m_IncrementHPorMP;
		}

		/// <summary>
		/// Gets the req lvl.
		/// </summary>
		/// <returns>
		/// The req lvl.
		/// </returns>
		public int 			GetReqLvl		()
		{
			return m_ReqLvl;
		}
		
		/// <summary>
		/// Gets the spirit.
		/// </summary>
		/// <returns>
		/// The spirit.
		/// </returns>
		public float GetSpirit()
		{
			return m_Spirit;
		}
		
		/// <summary>
		/// Gets the strenght.
		/// </summary>
		/// <returns>
		/// The strenght.
		/// </returns>
		public float GetStrenght()
		{
			return m_Strenght;
		}
		
		/// <summary>
		/// Gets the agility.
		/// </summary>
		/// <returns>
		/// The agility.
		/// </returns>
		public float GetAgility()
		{
			return m_Agility;
		}		
		
		/// <summary>
		/// Sets the item object I din list.
		/// </summary>
		/// <param name='pi_ID'>
		/// Pi_ I.
		/// </param>
		public void			SetItemObjectId         (int   pi_ID)
		{
			m_Id = pi_ID;
		}
		
		/// <summary>
		/// Gets the item object I din list.
		/// </summary>
		/// <returns>
		/// The item object I din list.
		/// </returns>
		public int			GetID					()
		{
			return m_Id;
		}
		
		
		
		
		/// <summary>
		/// Initializes a new instance of the <see cref="PlayerItemsManager.ItemObject"/> class.
		/// </summary>
		/// <param name='pi_ItemType'>
		/// Pi_ item type.
		/// </param>
		/// <param name='pi_2DTexture'>
		/// Pi_2 D texture.
		/// </param>
		/// <param name='pi_Protection'>
		/// Pi_ protection.
		/// </param>
		/// <param name='pi_AttackPower'>
		/// Pi_ attack power.
		/// </param>
		/// <param name='pi_IncrementHPorMP'>
		/// Pi_ increment H por M.
		/// </param>
		/// <param name='pi_SellValue'>
		/// Pi_ sell value.
		/// </param>
		/// <param name='pi_Defense'>
		/// Pi_ defense.
		/// </param>
		private ItemObject(ItemType			pi_ItemType,
						   Texture2D		pi_2DTexture,
						   float		    pi_AttackPower,
						   float			pi_IncrementHPorMP,
						   int				pi_SellValue,
						   float		    pi_Defense,
						   int				pi_ReqLvl,
			               float            pi_Strenght,
			               float            pi_Agility,
			               float            pi_Spirit)
		{
			m_ItemType 		  = pi_ItemType;
			m_2DTexture		  = pi_2DTexture;
			m_AttackPower	  = pi_AttackPower;
			m_IncrementHPorMP = pi_IncrementHPorMP;
			m_SellValue		  = pi_SellValue;
			m_Defense		  = pi_Defense;
			m_ReqLvl		  = pi_ReqLvl;
			m_Strenght        = pi_Strenght;
			m_Agility         = pi_Agility;
			m_Spirit          = pi_Spirit;
		}
		
		private ItemType 	m_ItemType;
		private Texture2D	m_2DTexture;
		private float		m_AttackPower;
		private float		m_IncrementHPorMP;
		private int         m_SellValue;
		private float		m_Defense;
		private int         m_ReqLvl;
		private int  		m_Id       = -1;
		private float 	    m_Strenght = -1;
		private float       m_Agility  = -1;
		private float		m_Spirit   = -1;
	}	
	
	
	
	public enum ItemType
	{
		ITEM_MANAPOTION,
		ITEM_HEALTHPOTION,
		ITEM_HELM,
		ITEM_BOOT,
		ITEM_SHOULDER,
		ITEM_RHAND,
		ITEM_LHAND,
		ITEM_CHEST,
	};
	
	
	
	
/////////////////////////////////////////////////////
	//GOLD SECTION
/////////////////////////////////////////////////////
	
	/// <summary>
	/// Sets the player gold.
	/// </summary>
	/// <param name='pi_Gold'>
	/// Pi_ gold.
	/// </param>
	public void 		SetPlayerGold		(int pi_Gold)
	{
		m_PlayerGold = pi_Gold;
		
		if(m_PlayerGold < 0)
		{
			Debug.LogError("ASSERT FAILURE: Le joueur a une valeur negative pour son or!");
		}
	}
	
	/// <summary>
	/// Decs the player gold.
	/// </summary>
	/// <param name='pi_Gold'>
	/// Pi_ gold.
	/// </param>
	public void 		DecPlayerGold		(int pi_Gold)
	{
		m_PlayerGold = m_PlayerGold - pi_Gold;
		
		if(m_PlayerGold < 0)
		{
			Debug.LogError("ASSERT FAILURE: Le joueur a une valeur negative pour son or!");
		}
	}
	
	/// <summary>
	/// Incs the player gold.
	/// </summary>
	/// <param name='pi_Gold'>
	/// Pi_ gold.
	/// </param>
	public void 		IncPlayerGold		(int pi_Gold)
	{
		m_PlayerGold += pi_Gold;
	}
	
	/// <summary>
	/// Gets the player gold.
	/// </summary>
	/// <returns>
	/// The player gold.
	/// </returns>
	public int 			GetPlayerGold		()
	{
		return m_PlayerGold;
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
        }
	}
}

