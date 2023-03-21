using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClanBuilder
{
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
		
		if(NeedStorage())
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
		bool storageNeeded = true;
		List<Structure> storages = GetStructures(BuildManager.Build.Storage);
		foreach (Structure storageStruct in storages)
		{
			Storage storage = storageStruct.gameObject.GetComponent<Storage>();
			if(storage == null)
			{
				//Only reason it should be null is if it is a build site
				storageNeeded = false;
			}
			else
			{
				if(!storage.IsFull())
					storageNeeded = false;
			}
		}

		return storageNeeded;
	}

	private bool NeedClanHall()
	{
		List<Structure> halls = GetStructures(BuildManager.Build.ClanHall);
		foreach(Structure hall in halls)
		{
			return false;
		}

		return true;
	}	
}
