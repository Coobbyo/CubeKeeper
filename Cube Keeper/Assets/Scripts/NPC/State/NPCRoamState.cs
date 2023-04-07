using UnityEngine;

public class NPCRoamState : NPCBaseState
{
	private NPCMovement movement;
	private NPCWorker work;

	private Vector3 target;
	private float moveRange;

	private TickTimer moveDelay;
	private TickTimer findDelay;
	private TickTimer breedDelay;

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
		breedDelay = new TickTimer(BreedCheck);
		breedDelay.Stop();
			
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
			breedDelay.Restart();

		target = manager.transform.position;
		manager.npc.SetTarget(null);
	}

	public override void LeaveState()
	{
		findDelay.Stop();
		moveDelay.Stop();
		breedDelay.Stop();
	}

	override public void UpdateState() {}

	private void Search()
	{
		int idleValue = manager.npc.stats.Idleness.GetValue();
		if(Random.value > 0.9 + (float)idleValue * 0.01f)
		{
			manager.SwitchState(manager.SearchState);
		}
	}

	private void FindNewDestination()
	{
		if(movement == null) return;//bandaid!
		
		int idleValue = manager.npc.stats.Idleness.GetValue();
		target = movement.FindNewDestination(moveRange - (float)idleValue * 0.25f);
		Search();
		
		moveDelay.Restart(Random.Range(10 + idleValue, 50 + idleValue));
	}

	private void FindWork()
	{
		if(work == null) return;//bandaid!

		int idleValue = manager.npc.stats.Idleness.GetValue();
		if(!work.FindWork())
			findDelay.Restart(Random.Range(5, 25 + idleValue));
	}

	private void BreedCheck()
	{
		int idleValue = manager.npc.stats.Idleness.GetValue();
		if(Random.value > 0.99)
			manager.SwitchState(manager.BreedState);
		else if(manager.npc.clan != null && !manager.npc.clan.IsFull())
			breedDelay.Restart(10 - idleValue);
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
