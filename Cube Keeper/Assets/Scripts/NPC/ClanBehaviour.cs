using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClanBehaviour : MonoBehaviour
{
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
		if(NeedStorage())
			BuildManager.Instance.RequestBuild(BuildManager.Build.Storage, Clan.GetRandomMemeber().transform.position, Clan);
		else if(NeedClanHall())
			BuildManager.Instance.RequestBuild(BuildManager.Build.ClanHall, Clan.CenterPoint, Clan);
		
		buildTimer.Restart();
	}

	private bool NeedStorage()
	{
		bool storageNeeded = true;
		List<Structure> storages = clan.builder.GetStructures(BuildManager.Build.Storage);
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
		List<Structure> halls = clan.builder.GetStructures(BuildManager.Build.ClanHall);
		foreach(Structure hall in halls)
		{
			return false;
		}

		return true;
	}
}
