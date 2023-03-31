using UnityEngine;

public class NPCRoamState : NPCBaseState
{
	private NPCMovement movement;
	private NPCWorker work;

	private Vector3 target;
	private float moveRange;

	private TickTimer moveDelay;
	private TickTimer findDelay;

	public NPCRoamState(NPCStateManager manager)
	{
		this.manager = manager;
		stats = manager.npc.stats;
		stateID = 2;
		movement = manager.npc.movement;
		work = manager.npc.work;

		target = manager.transform.position;
		moveRange = 10f;

		moveDelay = new TickTimer(FindNewDestination);
		moveDelay.Stop();
		findDelay = new TickTimer(FindWork);
		findDelay.Stop();
			
		manager.npc.SetTarget(null);
	}

	override public void EnterState()
	{
		//Debug.Log("Roaming");

		foreach(GameObject effect in manager.stateEffects)
		{
			effect.SetActive(false);
		}
		manager.stateEffects[stateID].SetActive(true);

		moveDelay.Restart();
		findDelay.Restart();

		if(manager.npc.clan != null && !manager.npc.clan.IsFull())
			TimeTickSystem.OnTick_Big += BreedCheck;

		manager.npc.SetTarget(null);
	}

	public override void LeaveState()
	{
		findDelay.Stop();
		moveDelay.Stop();
		TimeTickSystem.OnTick_Big -= BreedCheck;
	}

	override public void UpdateState() {}

	private void Search()
	{
		if(Random.value > 0.99)
		{
			manager.SwitchState(manager.SearchState);
		}
	}

	private void FindNewDestination()
	{
		if(movement == null) return;//bandaid!
		
		target = movement.FindNewDestination(moveRange);
		Search();
		
		//I want to eventually change this to be affected by lazyness
		moveDelay.Restart(Random.Range(5, 50));
	}

	private void FindWork()
	{
		if(work == null) return;//bandaid!
		if(!work.FindWork())
			findDelay.Restart(Random.Range(5, 50));
	}

	private void BreedCheck(int tick)
	{
		if(Random.value > 0.99)
			manager.SwitchState(manager.BreedState);
	}	

	override public Vector3 GetTarget()
	{
		//Debug.Log("From state" + target);
		return target;
	}

	public override void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(manager.transform.position, moveRange);
	}
}
