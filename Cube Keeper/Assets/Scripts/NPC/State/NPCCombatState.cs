using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCombatState : NPCBaseState
{
	NPCCombat combat;

	public NPCCombatState(NPCStateManager manager)
	{
		this.manager = manager;
		stats = manager.npc.stats;
		combat = manager.npc.combat;
	}

	override public void EnterState(NPCStateManager manager)
	{
		//Debug.Log("Fighting");
		foreach(GameObject effect in manager.stateEffects)
		{
			effect.SetActive(false);
		}
		manager.stateEffects[1].SetActive(true);
	}

	override public void UpdateState()
	{
		if(combat.Target == null)
		{
			FindTarget();
		}
		else
		{
			combat.Attack(combat.target);
		}
	}

	private void FindTarget()
	{
		List<NPC> otherNPCs = manager.npc.FindNearbyNPCs();
		var targets = new List<Damageable>();

		foreach(NPC otherNPC in otherNPCs)
		{
			if(IsTarget(otherNPC)) //Check if less than max health
			{
				targets.Add(otherNPC.stats);
			}
		}

		if(targets.Count > 0)
		{
			combat.Target = targets[Random.Range(0, targets.Count)].transform;
		}
		else
			manager.SwitchState(manager.RoamState);
	}

	public bool IsTarget(NPC otherNPC)
	{
		if(stats.Aggression.GetValue() > 0f && manager.npc.clan.IsEnemy(otherNPC.clan))
			return true;
		if(stats.Aggression.GetValue() > 5f && otherNPC.clan == null)
			return true;

		if(stats.Damage.GetValue() > 0f)
			return false;

		if(manager.npc.IsFriend(otherNPC) &&
			otherNPC.stats.CurrentHealth <= otherNPC.stats.MaxHealth.GetValue()) //Has less than full health
			return true;

		return false;
	}

	override public Vector3 GetTarget()
	{
		if(combat.Target == null)
			return manager.transform.position;
		else
			return combat.Target.position;
	}

	public override void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
	}
}
