using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClanBehaviour : MonoBehaviour
{
	public Transform memebersParent;
	public Transform structuresParent;
	private Timer buildTimer;
	private NPCClan clan;
	public NPCClan Clan
	{
		get { return clan; }
		set
		{
			clan = value;
		}
	}

	private void Start()
	{
		//Debug.Log("Starting clan");
		name = clan.ToString();

		buildTimer = new Timer(CheckBuildNeeds, 5f);
	}

	private void Update()
	{
		//return;
		if(Clan == null)
			Debug.LogError("Clan is null");
		buildTimer.Decrement();
	}

	private void CheckBuildNeeds()
	{
		StructureData nextBuild = Clan.builder.GetNextStructureToBuild();
		if(nextBuild != null)
			BuildManager.Instance.RequestBuild(nextBuild, Clan.GetRandomMemeber().transform.position, Clan);
		buildTimer.Restart();
	}
}
