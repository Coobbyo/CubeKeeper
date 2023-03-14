using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClanBuilder
{
	private List<Structure> structures = new List<Structure>();

	public ClanBuilder()
	{
		structures = new List<Structure>();
	}

	public StructureData nextStructureToBuild()
	{
		return BuildManager.Instance.GetStructure(BuildManager.Build.ClanHall);
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
}
