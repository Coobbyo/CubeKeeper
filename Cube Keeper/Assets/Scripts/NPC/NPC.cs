using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
	public enum State
	{
		Roam,
		Combat,
		Work
	}
	public State state;


	public float interactRange { get; private set; }
	public NPCClan clan { get; private set; }
	public Transform target { get; private set; }
	public Inventory inventory { get; private set; }
	public MeshRenderer colorDisplay;
	
	[SerializeField] private Material Clanless;

	private string id;
	

	[HideInInspector] public NPCMovement movement;
	[HideInInspector] public NPCSocialBehaviour social;
	[HideInInspector] public NPCCombat combat;
	[HideInInspector] public NPCStats stats;
	[HideInInspector] public NPCWorker work;


	private void Awake()
	{
		movement = GetComponent<NPCMovement>();
		social = GetComponent<NPCSocialBehaviour>();
		combat = GetComponent<NPCCombat>();
		stats = GetComponent<NPCStats>();
		work = GetComponent<NPCWorker>();
	}

	private void Start()
	{
		id = "NPC " + Random.Range(0, 10000);
		inventory = new Inventory();
		stats.OnHealthReachedZero += Die;
	}

	public void JoinClan(NPCClan clan)
	{
		this.clan = clan;
		colorDisplay.material.color = clan.GetColor();
		clan.AddMember(this);
	}

	public void LeaveClan()
	{
		colorDisplay.material = Clanless;
		if(clan != null)
			clan.RemoveMember(this);
		clan = null;
	}

	public void SetTarget(Transform t)
	{
		target = t;
	}

	public NPC FindNearbyNPC()
	{
		var nearbyNPCs = FindNearbyNPCs();

		if(nearbyNPCs.Count <= 0)
			return null;
		else
			return nearbyNPCs[Random.Range(0, nearbyNPCs.Count)];
	}

	public List<NPC> FindNearbyNPCs()
	{
		var nearbyNPCs = new List<NPC>();
		Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
		foreach(Collider collider in colliderArray)
		{
			if(collider.TryGetComponent(out NPC npc))
			{
				nearbyNPCs.Add(npc);
			}
		}

		return nearbyNPCs;
	}
	
	private void Die()
	{
		LeaveClan();
		Destroy(this.gameObject);
	}

	override public string ToString()
	{
		return id;
	}
}
