using UnityEngine;
using System.Collections;

public class MAJ_Quetes: MonoBehaviour
{
	public GUIText nomTexture;
	private string[] text_aff=new string[3]{"","",""};
	private float time_text = 4f;
	private bool TimerOn=false;
	
	
	
	void Start()
    {
		nomTexture.fontStyle = FontStyle.BoldAndItalic;
    }
			
	public void decreaseTime_text()
	{
    	time_text --;
		if (time_text==0)
		{
			text_aff[0]=text_aff[1];
			text_aff[1]=text_aff[2];
			text_aff[2]="";
			nomTexture.text = text_aff[0] + "\n" + text_aff[1] + "\n" + text_aff[2];
			CancelInvoke("decreaseTime_text");	
			TimerOn=false;
			time_text=4f;
			if (!text_aff[0].Equals(""))
			{
				InvokeRepeating("decreaseTime_text",1.0f,1.0f);
				TimerOn=true;
			}
		}
	}
	
	public void add_text(string t)
	{
		if (text_aff[0]=="")
		{
			text_aff[0]=t;
		}
		else if (text_aff[1]=="")
		{
			text_aff[1]=t;
		}
		else if (text_aff[2]=="")
		{
			text_aff[2]=t;
		}
		else
		{
			text_aff[0]=text_aff[1];
			text_aff[1]=text_aff[2];
			text_aff[2]=t;
		}
		nomTexture.text = text_aff[0] + "\n" + text_aff[1] + "\n" + text_aff[2];
		if (!TimerOn)
		{
			InvokeRepeating("decreaseTime_text",1.0f,1.0f);
			TimerOn=true;
		}
	}
	
	
	
}




