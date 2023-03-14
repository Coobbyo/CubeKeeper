using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
	public enum Build
	{
		BuildSite,
		Storage,
		ClanHall
	}
	public StructureData[] Structures;
    //maybe have some effects like when built or destroied?

    private static BuildManager instance;
	public static BuildManager Instance { get {return instance; } private set{} }
	private void Awake()
	{
		if(instance != null && instance != this)
			Destroy(this);
		else
			instance = this;
	}

	public void RequestBuild(Build build, Vector3 position, NPCClan clan)
	{
		GameObject buildSiteGO = Instantiate(GetStructure(0).prefab, position, Quaternion.identity, this.transform);
		BuildSite newBuildSite = buildSiteGO.GetComponent<BuildSite>();

		newBuildSite.structureToBuild = GetStructure(build);
		newBuildSite.Clan = clan;

		clan.builder.AddStructure(newBuildSite);
	}

	public void BuildStructure(StructureData data, Vector3 position, NPCClan clan)
	{
		GameObject structureGO = Instantiate(data.prefab, position, Quaternion.identity, this.transform);
		Structure newStructure = structureGO.GetComponent<Structure>();

		newStructure.Clan = clan;

		clan.builder.AddStructure(newStructure);
	}

	public StructureData GetStructure(Build build)
	{
		return Structures[(int)build];
	}
}
