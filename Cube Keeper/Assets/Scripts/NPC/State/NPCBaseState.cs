using UnityEngine;

public abstract class NPCBaseState
{
	protected NPCStateManager manager;
	protected NPCStats stats;
	public int stateID;

	abstract public void EnterState();
	abstract public void LeaveState();
	abstract public void UpdateState();
	abstract public Vector3 GetTarget();
	abstract public void OnDrawGizmosSelected();
}
