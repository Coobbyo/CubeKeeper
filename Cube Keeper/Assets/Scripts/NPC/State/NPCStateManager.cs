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


	private void Awake()
	{
		npc = GetComponent<NPC>();

		RoamState = new NPCRoamState(this);
		SearchState = new NPCSearchState(this);
		WorkState = new NPCWorkState(this);
		CombatState = new NPCCombatState(this);
		BreedState = new NPCBreedState(this);
	}

	private void Start()
	{
		SwitchState(RoamState);
	}

	private void Update()
	{
		currentState.UpdateState();
	}

	public void SwitchState(NPCBaseState state)
	{
		currentState = state;
		state.EnterState(this);
	}

	public Vector3 GetTarget()
	{
		return currentState.GetTarget();
	}

	public bool IsState(NPCBaseState state)
	{
		return state == currentState;
	}

	private void OnDrawGizmosSelected()
	{
		currentState.OnDrawGizmosSelected();
	}
}
