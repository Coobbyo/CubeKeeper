using UnityEngine;

public class NPCWorkState : NPCBaseState
{
	private NPCWorker work;

	private TickTimer workDelay;

	public NPCWorkState(NPCStateManager manager)
	{
		this.manager = manager;
		stats = manager.npc.stats;
		work = manager.npc.work;

		workDelay = new TickTimer(DoWork);
	}

	public override void EnterState(NPCStateManager manager)
	{
		//Debug.Log("Working");
		foreach(GameObject effect in manager.stateEffects)
		{
			effect.SetActive(false);
		}
		manager.stateEffects[4].SetActive(true);

		workDelay.Restart();
	}

	public override void UpdateState() {}

	private void DoWork()
	{
		if(work == null) return;//bandaid!
		//TODO: Maybe have an if statement that randomizes frequency based on lazyness
		if(Random.value > 0.25f)
			return;
		
		if(work.targetFrom == null)
		{
			if(work.carryPoint.childCount > 0)
				work.currentTarget = work.targetTo;
			else
			{
				//Debug.Log("Roaming From"); //This one keeps happening
				manager.SwitchState(manager.RoamState);
				return;
			}
		}
		
		if(work.targetTo == null || work.currentTarget == null)
		{
			//Debug.Log("Roaming To");
			manager.SwitchState(manager.RoamState);
			return;
		}

		work.DoWork();

		//TODO: have lazyness effect this
		workDelay.Restart(5);
	}

	override public Vector3 GetTarget()
	{
		if(work.currentTarget == null)
			return manager.transform.position;
		else
			return work.currentTarget.position;
	}

	public override void OnDrawGizmosSelected()
    {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(manager.transform.position, work.workingRange);
    }
}
