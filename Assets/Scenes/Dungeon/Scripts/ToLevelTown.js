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
 
 
function OnCollisionEnter (pi_Collision : Collision) 
{
	 if(pi_Collision.gameObject.name == "player")
	 {
	 	var pTablesManager = GameObject.Find("MANAGER_TABLES");
	 	
	 	pTablesManager.GetComponent("ManagersTable").s_GetPlayerTargetSysManager().SetTarget(null); //Remove targeted mobs
		pTablesManager.GetComponent("ManagersTable").s_GetEnnemiesManager().SetPlayerIsInDungeon(false); //break the thread for AI mobs

	 	pTablesManager.GetComponent("ManagersTable").s_GetPlayerPositionManager().SetLastPositionInDungeon(38.80f, 47.29f, 7.28f); //Registers last position in scene;
	 	pTablesManager.GetComponent("ManagersTable").s_GetPlayerHealthManager().SetPlayerMaxHealth(pi_Collision.gameObject.GetComponent("Health").MaxHealth); //save health
	 	pTablesManager.GetComponent("ManagersTable").s_GetPlayerHealthManager().SetPlayerMaxSpirit(pi_Collision.gameObject.GetComponent("Health").MaxSpirit); //save spirit
	 	pTablesManager.GetComponent("ManagersTable").s_GetPlayerHealthManager().SetPlayerHealth(pi_Collision.gameObject.GetComponent("Health").CurrentHealth); //save health
	 	pTablesManager.GetComponent("ManagersTable").s_GetPlayerHealthManager().SetPlayerMana(pi_Collision.gameObject.GetComponent("Health").CurrentSpirit); //save spirit
	 	
	 	var strenght = pi_Collision.gameObject.GetComponent("Attribute").strenght;
	 	var agility  = pi_Collision.gameObject.GetComponent("Attribute").agility;
	 	var spirit   = pi_Collision.gameObject.GetComponent("Attribute").spirit;
	 	var defence  = pi_Collision.gameObject.GetComponent("Attribute").defence;
	 	pTablesManager.GetComponent("ManagersTable").s_GetPlayerAttributeManager().setAttribute(strenght, agility, spirit, defence);
	 	
	 	pTablesManager.GetComponent("ManagersTable").s_GetPlayerItemsManager().GetComponent("FenetreInventaire").CloseInventoryWindowForLevelChange();
	 	
	  	Application.LoadLevel("ToTownLoading");
 	}
}