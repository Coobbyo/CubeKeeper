using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
	public Material Clanless;
	
	[SerializeField] private InputReader input;
	[SerializeField] private Transform player;
	[SerializeField] private Transform npcPrefab;
	[SerializeField] private Transform clanPrefab;
	[SerializeField] private ClanData[] premadeClans;

	private List<NPC> unclaimedNPCs = new List<NPC>();
	private List<NPCClan> clans = new List<NPCClan>();
	private List<ClanBehaviour> clanBs = new List<ClanBehaviour>();
	//private Dictionary<NPCClan, GameObject> clanToGO = new Dictionary<NPCClan, GameObject>();

	private static NPCManager instance;
	public static NPCManager Instance { get {return instance; } private set{} }
	private void Awake()
	{
		if(instance != null && instance != this)
			Destroy(this);
		else
			instance = this;
	}

	private void Start()
	{
		input.CreateEvent += HandleCreate;
		input.SuperCreateEvent += HandleCreate;
	}

	private void HandleCreate()
	{
		CreateNPC();
	}

	private NPC CreateNPC()
	{
		float spawnRadius = 0.5f;
		Vector3 spawnPoint = player.position +
			new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius));
		Transform npcGO = Instantiate(npcPrefab, spawnPoint, Quaternion.identity, this.transform);
		
		NPC newNPC = npcGO.GetComponent<NPC>();
		NPCClan newClan = CreateClan(premadeClans[Random.Range(0, premadeClans.Length)]);
		newNPC.JoinClan(newClan);

		unclaimedNPCs.Add(newNPC);
		return newNPC;
	}

	public void CreateNPC(NPCClan clan)
	{
		NPC npc = CreateNPC();
		unclaimedNPCs.Remove(npc);
		npc.JoinClan(clan);
	}

	public NPCClan CreateClan()
	{
		Transform clanGO = Instantiate(clanPrefab, transform);
		ClanBehaviour clan = clanGO.GetComponent<ClanBehaviour>();
		clan.Clan = new NPCClan();
		clan.Clan.behaviour = clan;

		//Debug.Log("Adding " + clan.Clan.ToString());
		clans.Add(clan.Clan);
		//clanToGO.Add(clan.Clan, clanGO.gameObject);
		
		return clan.Clan;
	}

	public NPCClan CreateClan(ClanData referenceData)
	{
		foreach(NPCClan npcClan in clans)
		{
			if(npcClan.ClanName == referenceData.clanName)
				return npcClan;
		}

		Transform clanGO = Instantiate(clanPrefab, transform);
		ClanBehaviour clan = clanGO.GetComponent<ClanBehaviour>();
		clan.Clan = new NPCClan(referenceData);
		clan.Clan.behaviour = clan;

		//Debug.Log("Adding " + clan.Clan.ToString());
		clans.Add(clan.Clan);
		//clanToGO.Add(clan.Clan, clanGO.gameObject);
		
		return clan.Clan;
	}

	public void RemoveClan(NPCClan clan)
	{
		clans.Remove(clan);
		ClanBehaviour behaviour = clan.behaviour;

		foreach (NPC npc in clan.Members)
		{
			npc.LeaveClan();
		}

		foreach(Structure structure in clan.builder.GetStructures())
		{
			structure.Clan = null;
			structure.Crumble();
		}

		Destroy(behaviour.gameObject);
		clan = null;
	}

	public ClanBehaviour GetClan(NPCClan clan)
	{
		/*
		if(clanToGO.TryGetValue(clan, out GameObject value))
		{
			if(value == null)
				Debug.Log("Why is this null");
			return value.GetComponent<ClanBehaviour>();
		}
		return null;
		*/

		/*
		Debug.Log("Looking for " + clan.ToString());
		foreach (ClanBehaviour clanB in clanBs)
		{
			if(clan.ToString() == clanB.name)
				return clanB;
		}
		return null;
		*/
		return null;
	}

	public List<NPCClan> GetClans()
	{
		return clans;
	}
}
