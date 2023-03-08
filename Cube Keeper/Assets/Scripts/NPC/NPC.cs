using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
	public enum State
	{
		Roam,
		Combat
	}
	public State state;

	
	public MeshRenderer colorDisplay;
	[SerializeField] private float interactRange = 1f;
	[SerializeField] private Material Clanless;

	private string id;
	private NPCClan clan;

	[HideInInspector] public NPCMovement movement;
	[HideInInspector] public NPCSocialBehaviour social;
	[HideInInspector] public NPCCombat combat;
	[HideInInspector] public NPCStats stats;

	private void Awake()
	{
		movement = GetComponent<NPCMovement>();
		social = GetComponent<NPCSocialBehaviour>();
		combat = GetComponent<NPCCombat>();
		stats = GetComponent<NPCStats>();
	}

	private void Start()
	{
		id = "NPC " + Random.Range(0, 10000);
		stats.OnHealthReachedZero += Die;
	}

	public NPCClan GetClan()
	{
		return clan;
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
		Destroy(this.gameObject);
	}

	override public string ToString()
	{
		return id;
	}
}