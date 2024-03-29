using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NPC))]
public class NPCStateManager : MonoBehaviour
{
	public NPC npc;

	public GameObject[] stateEffects;

	private NPCBaseState currentState;
	public NPCRoamState RoamState;
	public NPCSearchState SearchState;
	public NPCWorkState WorkState;
	public NPCCombatState CombatState;
	public NPCBreedState BreedState;
	public NPCHungeredState HungeredState;


	private void Awake()
	{
		npc = GetComponent<NPC>();

		RoamState = new NPCRoamState(this);
		SearchState = new NPCSearchState(this);
		WorkState = new NPCWorkState(this);
		CombatState = new NPCCombatState(this);
		BreedState = new NPCBreedState(this);
		HungeredState = new NPCHungeredState(this);
	}

	private void Start()
	{
		currentState = RoamState;
		currentState.EnterState();
	}

	private void Update()
	{
		currentState.UpdateState();
	}

	private void OnDestroy()
	{
		//Debug.Log("State Manager Destroied");
		currentState.LeaveState();
		currentState = null;
	}

	public void SwitchState(NPCBaseState state)
	{
		currentState.LeaveState();
		state.EnterState();
		currentState = state;
	}

	public Vector3 GetTarget()
	{
		return currentState.GetTarget();
	}

	public bool IsState(NPCBaseState state)
	{
		if(currentState == null)
		{
			return false;
		}
		return state.stateID == currentState.stateID;
	}

	private void OnDrawGizmosSelected()
	{
		currentState.OnDrawGizmosSelected();
	}
}
