using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
	[SerializeField] private InputReader input;
	[SerializeField] private Transform player;
	[SerializeField] private Transform npcPrefab;
	[SerializeField] private Transform clanPrefab;

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
		//newNPC.combat.bulletPrefab = bulletPrefab;

		unclaimedNPCs.Add(newNPC);
		return newNPC;
	}

	private void CreateNPC(NPCClan clan)
	{
		NPC npc = CreateNPC();
		unclaimedNPCs.Remove(npc);
		npc.JoinClan(clan);
	}

	public NPCClan CreateClan()
	{
		Transform clanGO = Instantiate(clanPrefab, transform);
		ClanBehaviour clanB = clanGO.GetComponent<ClanBehaviour>();
		NPCClan clan = new NPCClan();

		clanB.Clan = clan;
		clanB.name = clan.ToString();
		clan.behaviour = clanB;

		clans.Add(clan);
		//clanToGO.Add(clan, clanGO.gameObject);
		
		return clan;
	}

	public void RemoveCLan(NPCClan clan)
	{
		//Debug.Log("Removing " + clan.ToString());
		clans.Remove(clan);
		Destroy(clan.behaviour.gameObject);
		//if(clanToGO.TryGetValue(clan, out GameObject value))
			//Destroy(value);
		//clanToGO.Remove(clan);
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
