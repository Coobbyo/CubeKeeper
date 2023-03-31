using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClanBuilder
{
	public ClanHall hall;
	private NPCClan clan;
	private List<Structure> structures = new List<Structure>();

	public ClanBuilder()
	{
		structures = new List<Structure>();
	}

	public ClanBuilder(NPCClan clan)
	{
		this.clan = clan;
	}

	public StructureData GetNextStructureToBuild()
	{
		if(NeedClanHall())
			return BuildManager.Instance.GetStructure(BuildManager.Build.ClanHall);
		
		if(NeedFarm()) //This should stay at the bottom as the last thing to be built
			return BuildManager.Instance.GetStructure(BuildManager.Build.Farm);

		if(NeedHouse()) //This should stay at the bottom as the last thing to be built
			return BuildManager.Instance.GetStructure(BuildManager.Build.House);

		if(NeedTower()) //This should stay at the bottom as the last thing to be built
			return BuildManager.Instance.GetStructure(BuildManager.Build.Tower);
		
		if(NeedStorage()) //This should stay at the bottom as the last thing to be built
			return BuildManager.Instance.GetStructure(BuildManager.Build.Storage);

		return null;
	}

	public List<Structure> GetStructures()
	{
		return structures;
	}

	public List<Structure> GetStructures(StructureData referenceData)
	{
		List<Structure> matchingStructures = new List<Structure>();
		foreach (Structure structure in structures)
		{
			if(structure.GetData() == referenceData)
				matchingStructures.Add(structure);
			else if(structure.GetData() == BuildManager.Instance.GetStructure(BuildManager.Build.BuildSite))
			{
				BuildSite site = structure.GetComponent<BuildSite>();
				if(site != null && site.structureToBuild == referenceData)
					matchingStructures.Add(structure);
			}
		}

		return matchingStructures;
	}

	public List<Structure> GetStructures(BuildManager.Build build)
	{
		StructureData data = BuildManager.Instance.GetStructure(build);
		return GetStructures(data);
	}

	public void AddStructure(Structure structure)
	{
		structures.Add(structure);
	}

	public void RemoveStructure(Structure structure)
	{
		structures.Remove(structure);
	}

	private bool NeedStorage()
	{
		List<Item> resourceDeficit = clan.behaviour.ResourceDeficit();
		if(resourceDeficit == null || resourceDeficit.Count <= 0)
			return true;

		List<Structure> storages = GetStructures(BuildManager.Build.Storage);
		foreach (Structure storageStruct in storages)
		{
			Storage storage = storageStruct.gameObject.GetComponent<Storage>();
			if(storage == null)
			{
				//Only reason it should be null is if it is a build site
				return false;
			}
			else
			{
				if(!storage.IsFull())
					return false;
			}
		}

		return true;
	}

	private bool NeedClanHall()
	{
		if(hall != null)
			return false;
		
		List<Structure> halls = GetStructures(BuildManager.Build.ClanHall);
		foreach(Structure clanHall in halls)
		{
			return false;
		}

		return true;
	}

	private bool NeedFarm()
	{
		bool needFood = false;
		ItemData foodData = ResourceManager.Instance.GetResource(ResourceManager.Resource.Food);
		List<Item> needs = clan.behaviour.ResourceDeficit();
		foreach(Item need in needs)
		{
			if(need.Data == foodData)
				needFood = true;
		}

		if(needFood)
		{
			List<Structure> farms = GetStructures(BuildManager.Build.Farm);
			if(farms.Count >= clan.Size / 2 && farms.Count > 0)
				needFood = false;
		}

		return needFood;
	}

	private bool NeedHouse()
	{
		List<Structure> houses = GetStructures(BuildManager.Build.House);
		foreach(Structure house in houses)
		{
			return false;
		}

		return true;
	}

	private bool NeedTower()
	{
		return false;
	}
}
