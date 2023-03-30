using UnityEngine;

public abstract class NPCBaseState
{
	protected NPCStateManager manager;
	protected NPCStats stats;

	abstract public void EnterState(NPCStateManager manager);
	abstract public void UpdateState();
	abstract public Vector3 GetTarget();
	abstract public void OnDrawGizmosSelected();
}
