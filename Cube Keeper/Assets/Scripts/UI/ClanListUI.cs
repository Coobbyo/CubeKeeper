using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClanListUI : MonoBehaviour
{
	[SerializeField] private Transform GridContent;
	[SerializeField] private Transform summaryPrefab;

	[SerializeField] private NPCUI ui;

	public void Propigate(List<NPCClan> clanList)
	{
		Clear();
		foreach(NPCClan clan in clanList)
		{
			Transform summary = Instantiate(summaryPrefab, GridContent);
			summary.GetComponent<ClanSummary>().SetClan(clan);
			summary.GetComponent<ClanSummary>().ui = ui;
		}
	}

	public void Clear()
	{
		foreach (Transform child in GridContent)
		{
			Destroy(child.gameObject);
		}
	}
}
