using System.Collections.Generic;
using UnityEngine;

public class NPCBreedState : NPCBaseState
{
	public enum State
	{
		Needy,
		BreedReady,
		PostPartum
	}
	public State state;

	public NPC partner;

	private NPCMovement movement;
	private NPCWorker work;

	private Vector3 target;
	private float moveRange;
	private ItemData foodData;
	private Transform foodSource;

	private TickTimer moveDelay;
	private TickTimer findDelay;

	public NPCBreedState(NPCStateManager manager)
	{
		this.manager = manager;
		stats = manager.npc.stats;
		stateID = 0;
		movement = manager.npc.movement;
		work = manager.npc.work;

		target = manager.transform.position;
		moveRange = 10f;
		foodData = ResourceManager.Instance.GetResource(ResourceManager.Resource.Food);

		moveDelay = new TickTimer(FindNewDestination);
		moveDelay.Stop();
		findDelay = new TickTimer(Find, 10);
		findDelay.Stop();
			
		manager.npc.SetTarget(null);
	}

	override public void EnterState()
	{
		Debug.Log("Breeding");
		foreach(GameObject effect in manager.stateEffects)
		{
			effect.SetActive(false);
		}
		manager.stateEffects[stateID].SetActive(true);

		state = State.Needy;

		moveDelay.Restart();
		findDelay.Restart();

		manager.npc.SetTarget(null);
	}

	public override void LeaveState()
	{
		Debug.Log("Leaving Breeding");
		partner = null;
		foodSource = null;

		findDelay.Stop();
		moveDelay.Stop();
	}

	override public void UpdateState()
	{
		if(state != State.BreedReady)
			return;

		if(partner.GetPartner() != manager.npc || !partner.stateManager.IsState(this))
		{
			state = State.Needy;
			findDelay.Restart();
		}

		if(Vector3.Distance(manager.transform.position, partner.transform.position) <= manager.npc.interactRange)
		{
			//Debug.Log("Close enough to breed");
			if(partner.stateManager.BreedState.state == NPCBreedState.State.BreedReady)
			{
				Breed();
			}
			//Debug.Log("Partner can't breed");
		}
	}

	private void FindNewDestination()
	{
		if(movement == null) return;//bandaid!

		if(partner != null)
			return;
		
		target = movement.FindNewDestination(moveRange);
		Roam();
		
		int maxValue = Mathf.Clamp(stats.Idleness.GetValue() * 5, 0, 100);
		moveDelay.Restart(Random.Range(5, maxValue +1));
	}

	private void Roam()
	{
		if(Random.value > 0.99 || manager.npc.clan == null || manager.npc.clan.IsFull())
		{
			manager.SwitchState(manager.RoamState);
		}
	}

	private void Find()
	{
		if(partner == null)
			FindPartner();
		else if(manager.npc.work.ItemCarried != foodData)
			FindFood();

		if(state == State.Needy)
			findDelay.Restart();
	}

	private void FindPartner()
	{
		List<NPC> otherNPCs;
		if(manager.npc.clan == null)
			otherNPCs = manager.npc.FindNearbyNPCs();
		else
			otherNPCs = manager.npc.clan.Members;
		var partners = new List<NPC>();

		foreach(NPC otherNPC in otherNPCs)
		{
			if(otherNPC.stateManager.IsState(this))
			{
				partners.Add(otherNPC);
			}
		}

		if(partners.Count > 0)
		{
			partner = partners[Random.Range(0, partners.Count)];
			partner.SetPartner(manager.npc);
			moveDelay.Stop();
		}
	}

	private void FindFood()
	{
		if(manager.npc.work.IsItemCarried() && manager.npc.work.ItemCarried != foodData)
			work.DropItem();

		if(foodSource != null)
		{
			if(Vector3.Distance(manager.transform.position, foodSource.position) <= manager.npc.interactRange)
			{
				work.FindFromTarget();
				if(work.targetFrom != null)
					work.Withdraw();
			}

			if(manager.npc.work.IsItemCarried() && manager.npc.work.ItemCarried == foodData)
			{
				state = State.BreedReady;
				manager.npc.SetTarget(partner.transform);
				return;
			}
		}

		StructureData farmData = BuildManager.Instance.GetStructure(BuildManager.Build.Farm);
		List<Structure> structures = manager.npc.clan.builder.GetStructures(farmData);
		var farms = new List<Farm>();

		foreach(Structure structure in structures)
		{
			if(structure.GetData() == farmData) //Check if less than max health
			{
				farms.Add(structure.GetComponent<Farm>());
			}
		}

		if(farms.Count > 0)
		{
			foodSource = farms[Random.Range(0, farms.Count)].transform;
			manager.npc.SetTarget(foodSource.transform);
		}
		else
		{
			manager.SwitchState(manager.RoamState);
		}
	}

	private void Breed()
	{
		//Debug.Log("making a baby");
		NPCManager.Instance.CreateNPC(manager.npc.clan); //This probably creates two babies..but WHO CARES! MOAR BABYZ!
		state = State.PostPartum;
		manager.SwitchState(manager.RoamState);
	}

	override public Vector3 GetTarget()
	{
		return target;
	}

	public override void OnDrawGizmosSelected() {}
}
