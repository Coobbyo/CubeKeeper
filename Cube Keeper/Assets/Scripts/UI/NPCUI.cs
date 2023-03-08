using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCUI : MonoBehaviour
{
	[SerializeField] private NPCManager npcManager;
	[SerializeField] private ClanListUI clanList;
	[SerializeField] private ClanDisplayUI clanDisplay;
	[SerializeField] private NPCDisplayUI npcDisplay;

	private void OnEnable()
	{
		OpenClanList();
		clanList.Propigate(npcManager.GetClans());
	}

	public void OpenUI()
	{
		gameObject.SetActive(true);
		OpenClanList();
	}

	public void CloseUI()
	{
		gameObject.SetActive(false);
	}

	public void SetClan(NPCClan clan)
	{
		clanDisplay.SetClan(clan);
	}

	public void SetNPC(NPC npc)
	{
		npcDisplay.SetNPC(npc);
	}

	public void OpenClanList()
	{
		clanDisplay.gameObject.SetActive(false);
		npcDisplay.gameObject.SetActive(false);
		clanList.gameObject.SetActive(true);
	}

	public void OpenClanDisplay()
	{
		clanList.gameObject.SetActive(false);
		npcDisplay.gameObject.SetActive(false);
		clanDisplay.gameObject.SetActive(true);
	}

	public void OpenNPCDisplay()
	{
		clanList.gameObject.SetActive(false);
		clanDisplay.gameObject.SetActive(false);
		npcDisplay.gameObject.SetActive(true);
	}
}
