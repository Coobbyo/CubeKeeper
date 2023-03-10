using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCSummary : MonoBehaviour
{
	public NPCUI ui;
	private NPC npc;

	[SerializeField] private Image clanColor;
	[SerializeField] private TMP_Text npcName;

	public void SetNPC(NPC npc)
	{
		this.npc = npc;

		if(npc.clan == null)
			Debug.Log(ui + " does not have a clan");
		else
			clanColor.color = npc.clan.GetColor();
		
		npcName.text = npc.ToString();
	}

	public void OpenNPC()
	{
		//Debug.Log("OpenNPC");
		ui.SetNPC(npc);
		ui.OpenNPCDisplay();
	}
}   
