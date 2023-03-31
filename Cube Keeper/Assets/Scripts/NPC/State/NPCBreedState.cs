using System.Collections.Generic;
using UnityEngine;

public class NPCBreedState : NPCBaseState
{
	public enum State
	{
		NeedPartner,
		NeedFood,
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
		movement = manager.npc.movement;
		stateID = 0;
		work = manager.npc.work;

		target = manager.transform.position;
		moveRange = 10f;
		foodData = ResourceManager.Instance.GetResource(ResourceManager.Resource.Food);

		moveDelay = new TickTimer(FindNewDestination);
		moveDelay.Stop();
		findDelay = new TickTimer(Find, 5);
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

		state = State.NeedPartner;

		moveDelay.Restart();
		findDelay.Restart();

		manager.npc.SetTarget(null);
	}

	public override void LeaveState()
	{
		partner = null;
		foodSource = null;

		findDelay.Stop();
		moveDelay.Stop();
	}

	override public void UpdateState()
	{
		if(state != State.BreedReady)
			return;

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
		
		//I want to eventually change this to be affected by lazyness
		moveDelay.Restart(Random.Range(5, 50));
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
	}

	private void FindPartner()
	{
		List<NPC> otherNPCs = manager.npc.FindNearbyNPCs();
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

		findDelay.Restart();
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
			else
				findDelay.Restart();
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
			findDelay.Restart();
		}
		else
		{
			manager.SwitchState(manager.RoamState);
			//findDelay.Stop();
		}
	}

	private void Breed()
	{
		Debug.Log("making a baby");
		NPCManager.Instance.CreateNPC(manager.npc.clan); //This probably creates two babies..but WHO CARES! MOAR BABYZ!
		manager.SwitchState(manager.RoamState);
	}

	override public Vector3 GetTarget()
	{
		return target;
	}

	public override void OnDrawGizmosSelected() {}
}
