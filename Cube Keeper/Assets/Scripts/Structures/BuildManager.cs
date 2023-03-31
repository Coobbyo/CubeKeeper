using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
	public enum Build
	{
		BuildSite,
		Storage,
		ClanHall,
		Farm,
		House,
		Tower
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

	public bool RequestBuild(Build build, Vector3 position, NPCClan clan)
	{
		return RequestBuild(GetStructure(build), position, clan);
	}

	public bool RequestBuild(StructureData data, Vector3 position, NPCClan clan)
	{
		float radius = 3f;
		LayerMask mask = new LayerMask();
		mask |= (1 << LayerMask.NameToLayer("Spawnable"));
		Collider[] colliders = Physics.OverlapSphere(position, radius, mask);

		if(colliders.Length > 0)
		{
			for(int i = 0; i < 3; i++)
			{
				Vector3 point = new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y);
				Vector3 newPoint = transform.position + point * radius * 10;
				colliders = Physics.OverlapSphere(position, radius, mask);
				if(colliders.Length > 0)
					continue;
				else
					break;
			}

			if(colliders.Length > 0) //Still not able to build
				return false;
		}

		GameObject buildSiteGO = Instantiate(GetStructure(0).prefab, position, Quaternion.identity, clan.behaviour.structuresParent);
		BuildSite newBuildSite = buildSiteGO.GetComponent<BuildSite>();

		newBuildSite.structureToBuild = data;
		newBuildSite.Clan = clan;

		clan.builder.AddStructure(newBuildSite);

		return true;
	}

	public void BuildStructure(StructureData data, Vector3 position, NPCClan clan)
	{
		GameObject structureGO = Instantiate(data.prefab, position, Quaternion.identity, clan.behaviour.structuresParent);
		Structure newStructure = structureGO.GetComponent<Structure>();

		newStructure.Clan = clan;

		clan.builder.AddStructure(newStructure);
		
		ClanHall newHall;
		if(newHall = newStructure.GetComponent<ClanHall>())
			clan.builder.hall = newHall;
	}

	public StructureData GetStructure(Build build)
	{
		return Structures[(int)build];
	}
}
