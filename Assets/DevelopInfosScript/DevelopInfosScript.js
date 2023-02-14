// Attach this to a GUIText to make a frames/second indicator.
//
// It calculates frames/second over each updateInterval,
// so the display does not keep changing wildly.
//
// It is also fairly accurate at very low FPS counts (<10).
// We do this not by simply counting frames per interval, but
// by accumulating FPS for each frame. This way we end up with
// correct overall FPS even if the interval renders something like
// 5.5 frames.
 
var updateInterval = 0.5;
 
private var accum = 0.0; // FPS accumulated over the interval
private var frames = 0; // Frames drawn over the interval
private var timeleft : float; // Left time for current interval

private var pTableManager : GameObject;
 
function Start()
{
    if( !guiText )
    {
        print ("FramesPerSecond needs a GUIText component!");
        enabled = false;
        return;
    }
    
    
    pTableManager = GameObject.Find("MANAGER_TABLES");
    timeleft      = updateInterval; 
}
 
function Update()
{
    //Mod by Mathieu
	var l_CurrentLVL = pTableManager.GetComponent("ManagersTable").s_GetPlayerXPManager().GetPlayerLevel();
	var l_MaxXP      = pTableManager.GetComponent("ManagersTable").s_GetPlayerXPManager().GetPlayerXPMaxForLvl();
	var l_CurrentXP  = pTableManager.GetComponent("ManagersTable").s_GetPlayerXPManager().GetPlayerXP();
	var l_Gold       = pTableManager.GetComponent("ManagersTable").s_GetPlayerItemsManager().GetPlayerGold();
	var l_CurrentHP  = pTableManager.GetComponent("ManagersTable").s_GetPlayerPositionManager().GetPlayer().GetComponent("Health").CurrentHealth;
	var l_MaxHP      = pTableManager.GetComponent("ManagersTable").s_GetPlayerPositionManager().GetPlayer().GetComponent("Health").MaxHealth;
	var l_MobHP      = pTableManager.GetComponent("ManagersTable").s_GetPlayerTargetSysManager().GetTargetCurrentHealth();
	var l_MobMaxHP   = pTableManager.GetComponent("ManagersTable").s_GetPlayerTargetSysManager().GetTargetMaxHealth();
	var l_MobName    = pTableManager.GetComponent("ManagersTable").s_GetPlayerTargetSysManager().GetTargetName();
	var l_MaxMana    = pTableManager.GetComponent("ManagersTable").s_GetPlayerPositionManager().GetPlayer().GetComponent("Health").MaxSpirit;
	var l_CurrentMana= pTableManager.GetComponent("ManagersTable").s_GetPlayerPositionManager().GetPlayer().GetComponent("Health").CurrentSpirit;
	//Mod by Mathieu

    timeleft -= Time.deltaTime;
    accum += Time.timeScale/Time.deltaTime;
    ++frames;
 
    // Interval ended - update GUI text and start new interval
    if( timeleft <= 0.0 )
    {
        // display two fractional digits (f2 format)
        guiText.text = "<<< DEV_INFOS >>>  FPS: " + (accum/frames).ToString("f2") + "\nLVL: " + l_CurrentLVL +
                       "      XP: " + l_CurrentXP + "/" + l_MaxXP + "       Or: " + l_Gold       +
                       "\nHP: "     + l_CurrentHP + "/" + l_MaxHP + "\n\n" +
                       "Target: "  + l_MobName   + "   HP: "     + l_MobHP + "/" + l_MobMaxHP + 
                       "\nMana: "  + l_CurrentMana + "/" + l_MaxMana;
        timeleft = updateInterval;
        accum    = 0.0;
        frames   = 0;
    }
}