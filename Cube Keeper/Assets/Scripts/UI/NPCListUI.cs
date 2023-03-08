using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCListUI : MonoBehaviour
{
	[SerializeField] private Transform GridContent;
	[SerializeField] private Transform summaryPrefab;

	[SerializeField] private NPCUI ui;

	public void Propigate(List<NPC> npcList)
	{
		Clear();
		foreach(NPC npc in npcList)
		{
			Transform summary = Instantiate(summaryPrefab, GridContent);
			summary.GetComponent<NPCSummary>().SetNPC(npc);
			summary.GetComponent<NPCSummary>().ui = ui;
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
