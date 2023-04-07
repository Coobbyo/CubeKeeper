using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHungeredState : NPCBaseState
{
	private NPCMovement movement;
	private NPCWorker work;

	private int hungered;
	private Vector3 target;
	private float moveRange;
	private ItemData foodData;
	private Transform foodSource;

	private TickTimer moveDelay;
	private TickTimer findDelay;

	public NPCHungeredState(NPCStateManager manager)
	{
		this.manager = manager;
		stats = manager.npc.stats;
		stateID = 5;
		movement = manager.npc.movement;
		work = manager.npc.work;

		hungered = 0;
		target = manager.transform.position;
		moveRange = 5f;
		foodData = ResourceManager.Instance.GetResource(ResourceManager.Resource.Food);

		moveDelay = new TickTimer(FindNewDestination);
		moveDelay.Stop();
		findDelay = new TickTimer(FindFood);
		findDelay.Stop();
			
		manager.npc.SetTarget(null);
	}

	public override void EnterState()
	{
		foreach(GameObject effect in manager.stateEffects)
		{
			effect.SetActive(false);
		}
		manager.stateEffects[stateID].SetActive(true);

		moveDelay.Restart();
		findDelay.Restart();

		target = manager.transform.position;
		manager.npc.SetTarget(null);
	}

	public override void LeaveState()
	{
		stats.TakeDamage(hungered++);
		findDelay.Stop();
		moveDelay.Stop();
	}

	public override void UpdateState()
	{
	}

	private void FindNewDestination()
	{
		if(movement == null) return;//bandaid!
		
		int idleValue = manager.npc.stats.Idleness.GetValue();
		target = movement.FindNewDestination(moveRange);
		
		moveDelay.Restart(Random.Range(25 + idleValue, 50 + idleValue));
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
				manager.npc.ConsumeFood();
				manager.SwitchState(manager.RoamState);
				hungered = 0;
				Debug.Log("Hunger reset");
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
		}
	}

	public override Vector3 GetTarget()
	{
		return target;
	}

	public override void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(manager.transform.position, moveRange);
	}
}
