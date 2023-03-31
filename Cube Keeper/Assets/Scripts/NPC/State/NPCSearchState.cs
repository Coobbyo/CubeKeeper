using UnityEngine;

public class NPCSearchState : NPCBaseState
{
	private NPCMovement movement;
	private NPCWorker work;

	private Vector3 target;
	private float moveRange;

	private TickTimer moveDelay;
	private TickTimer findDelay;

	public NPCSearchState(NPCStateManager manager)
	{
		this.manager = manager;
		movement = manager.npc.movement;
		stateID = 3;
		stats = manager.npc.stats;
		work = manager.npc.work;
		
		target = manager.transform.position;
		moveRange = 25f;

		moveDelay = new TickTimer(FindNewDestination);
		moveDelay.Stop();
		findDelay = new TickTimer(FindWork);
		findDelay.Stop();
			
		manager.npc.SetTarget(null);
	}

	override public void EnterState()
	{
		//Debug.Log("Searching");
		foreach(GameObject effect in manager.stateEffects)
		{
			effect.SetActive(false);
		}
		manager.stateEffects[stateID].SetActive(true);

		moveDelay.Restart();
		findDelay.Restart();

		manager.npc.SetTarget(null);
	}

	public override void LeaveState()
    {
        findDelay.Stop();
		moveDelay.Stop();
    }

	override public void UpdateState() {}

	private void Roam()
	{
		if(Random.value > 0.99)
		{
			manager.SwitchState(manager.RoamState);
		}
	}

	private void FindNewDestination()
	{
		if(movement == null) return;//bandaid!
			
		
		target = movement.FindNewDestination(moveRange);
		Roam();
		
		//I want to eventually change this to be affected by lazyness
		moveDelay.Restart(Random.Range(25, 50));
	}
	
	private void FindWork()
	{
		if(work == null) return;//bandaid!
		if(!work.FindWork())
			findDelay.Restart(Random.Range(5, 10));
		//else
			//Debug.Log("Finding work");
	}

	override public Vector3 GetTarget()
	{
		return target;
	}

	override public void OnDrawGizmosSelected()
    {
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(manager.transform.position, moveRange);
    }

    
}
