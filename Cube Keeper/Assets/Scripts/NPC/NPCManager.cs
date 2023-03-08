using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
	[SerializeField] private InputReader input;
	[SerializeField] private Transform player;
	[SerializeField] private Transform npcPrefab;

	private List<NPC> unclaimedNPCs = new List<NPC>();
	private List<NPCClan> clans = new List<NPCClan>();

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
		unclaimedNPCs.Add(newNPC);

		return newNPC;
	}

	private void CreateNPC(NPCClan clan)
	{
		NPC npc = CreateNPC();
		unclaimedNPCs.Remove(npc);
		npc.JoinClan(clan);
	}

	public void AddClan(NPCClan clan)
	{
		clans.Add(clan);
	}

	public void RemoveCLan(NPCClan clan)
	{
		clans.Remove(clan);
		clan = null;
	}

	public List<NPCClan> GetClans()
	{
		return clans;
	}
}
