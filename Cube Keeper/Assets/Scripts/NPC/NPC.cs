using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NPCStateManager))]
public class NPC : MonoBehaviour
{
	public NPCStateManager stateManager;

	public float interactRange { get; private set; }
	public NPCClan clan { get; private set; }
	public Transform target { get; private set; }
	public Inventory inventory { get; private set; }
	public MeshRenderer colorDisplay;

	private string id;

	[HideInInspector] public NPCMovement movement;
	[HideInInspector] public NPCSocialBehaviour social;
	[HideInInspector] public NPCCombat combat;
	[HideInInspector] public NPCStats stats;
	[HideInInspector] public NPCWorker work;


	private void Awake()
	{
		stateManager = GetComponent<NPCStateManager>();

		movement = GetComponent<NPCMovement>();
		social = GetComponent<NPCSocialBehaviour>();
		combat = GetComponent<NPCCombat>();
		stats = GetComponent<NPCStats>();
		work = GetComponent<NPCWorker>();
	}

	private void Start()
	{
		id = "NPC " + Random.Range(0, 10000);
		interactRange = 3f;
		inventory = new Inventory();
		stats.OnHealthReachedZero += Die;
	}

	public bool JoinClan(NPCClan clan)
	{
		if(!clan.AddMember(this))
			return false;
		this.clan = clan;
		stats.Clan = clan;
		colorDisplay.material.color = clan.Color;
		
		//if(NPCManager.Instance.GetClan(clan) == null)
			//Debug.Log("it's clanGO");
		transform.SetParent(clan.behaviour.memebersParent, true);

		return true;
	}

	public void LeaveClan()
	{
		colorDisplay.material = NPCManager.Instance.Clanless;
		if(clan != null)
		{
			//Debug.Log("Leaving " + clan.ToString());
			clan.RemoveMember(this);
			stats.Clan = null;
			clan = null;
		}
		transform.SetParent(NPCManager.Instance.transform, true);
	}

	public void SetPartner(NPC partner)
	{
		stateManager.BreedState.partner = partner;
		//SetTarget(partner.transform);
	}

	public NPC GetPartner()
	{
		return stateManager.BreedState.partner;
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
	
	public bool IsFriend(NPC otherNPC)
	{
		return clan == otherNPC.clan || clan.IsFriend(otherNPC.clan);
	}

	public bool IsEnemy(NPC otherNPC)
	{
		return clan.IsEnemy(otherNPC.clan);
	}

	public void Search()
	{
		if(Random.value < (stats.Idleness.GetValue() * 0.05f))
			return;
		if(stateManager.IsState(stateManager.RoamState))
			stateManager.SwitchState(stateManager.SearchState);
	}

	public bool ConsumeFood()
	{
		if(work.Consume())
		{
			stats.Heal(10);
			return true;
		} else
		{
			return false;
		}
	}

	private void Die()
	{
		//Debug.Log("Fly you fools!");
		work.DropItem();
		LeaveClan();
		Destroy(gameObject);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, interactRange);
	}

	override public string ToString()
	{
		return id;
	}
}
