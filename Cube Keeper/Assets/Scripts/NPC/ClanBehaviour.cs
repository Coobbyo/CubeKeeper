using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ClanBehaviour : MonoBehaviour
{
	public Transform memebersParent;
	public Transform structuresParent;
	private TickTimer buildTimer;
	private NPCClan clan;
	public NPCClan Clan
	{
		get { return clan; }
		set
		{
			clan = value;
		}
	}

	private int failedBuilds;

	private void Start()
	{
		//Debug.Log("Starting clan");
		name = clan.ToString();

		buildTimer = new TickTimer(CheckBuildNeeds, 5);
	}

	private void Update()
	{
		//return;
		if(Clan == null)
			Debug.LogError("Clan is null");
	}

	private void CheckBuildNeeds()
	{
		StructureData nextBuild = Clan.builder.GetNextStructureToBuild();
		if(nextBuild != null)
			if(!BuildManager.Instance.RequestBuild(nextBuild, Clan.GetRandomMemeber().transform.position, Clan)) failedBuilds++;

		if(failedBuilds > 3)
			Clan.Search();

		buildTimer.Restart();
	}

	public List<Item> TotalResources()
	{
		var structures = clan.builder.GetStructures(BuildManager.Build.Storage);
		structures.Union<Structure>(clan.builder.GetStructures(BuildManager.Build.BuildSite).ToList<Structure>());

		var resources = new Inventory();
		foreach(Structure structure in structures)
		{
			var storage = structure.GetComponent<IInventory>();
			if(storage == null)
				continue;

			foreach(Item item in storage.GetItems())
			{
				resources.Add(item);
			}
		}

		return resources.items;
	}

	public List<Item> ResourcesNeeded()
	{
		var structures = clan.builder.GetStructures(BuildManager.Build.BuildSite);

		var resources = new Inventory();
		foreach(Structure structure in structures)
		{
			var site = structure.GetComponent<BuildSite>();
			if(site == null)
				continue;

			foreach(Item item in site.GetNeededResources())
			{
				resources.Add(item);
			}
		}

		return resources.items;
	}

	public List<Item> ResourceDeficit()
	{
		Inventory resourceDeficit = new Inventory(ResourcesNeeded());
		foreach(Item item in TotalResources())
		{
			resourceDeficit.Remove(item);
		}

		//If there is a deficit we should send cubes to search

		return resourceDeficit.items;
	}
}
