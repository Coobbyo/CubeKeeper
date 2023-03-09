using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClanSummary : MonoBehaviour
{
	public NPCUI ui;
	private NPCClan clan;

	[SerializeField] private Image clanColor;
	[SerializeField] private TMP_Text clanName;
	[SerializeField] private TMP_Text clanSize;

	public void SetClan(NPCClan clan)
	{
		this.clan = clan;
		clanColor.color = clan.GetColor();
		clanName.text = clan.ToString();
		clanSize.text = clan.GetClanSize().ToString();
	}

	public void OpenClan()
	{
		ui.SetClan(clan);
		ui.OpenClanDisplay();
	}
}   
