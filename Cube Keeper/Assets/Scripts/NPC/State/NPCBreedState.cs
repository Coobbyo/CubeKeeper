using UnityEngine;

public class NPCBreedState : NPCBaseState
{
	public NPCBreedState(NPCStateManager manager)
	{
		this.manager = manager;
		stats = manager.npc.stats;
	}

	override public void EnterState(NPCStateManager manager)
	{
		Debug.Log("Breeding");
		foreach(GameObject effect in manager.stateEffects)
		{
			effect.SetActive(false);
		}
		manager.stateEffects[0].SetActive(true);
	}

	override public void UpdateState() {}

	override public Vector3 GetTarget()
	{
		return manager.npc.target.position;
	}

	public override void OnDrawGizmosSelected()
	{
	}
}
