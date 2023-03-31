using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSocialBehaviour : MonoBehaviour
{
	private NPC npc;
	public NPCClan clan
	{
		get { return npc.clan; }
		private set
		{
			if(value == null)
				npc.LeaveClan();
			else
			{
				npc.LeaveClan();
				loyalty.ClearModifiers();
				loyalty.AddModifier(5);
				npc.JoinClan(value);
			}
		}
	}

	private Stat friendliness
	{
		get { return npc.stats.Friendliness; }
		set { npc.stats.Friendliness = value; }
	}
	private Stat loyalty
	{
		get { return npc.stats.Loyalty; }
		set { npc.stats.Loyalty = value; }
	}
	private Stat aggression
	{
		get { return npc.stats.Loyalty; }
		set { npc.stats.Loyalty = value; }
	}

	private TickTimer socialDelay;

	private void Awake()
	{
		npc = GetComponent<NPC>();
	}

	private void Start()
	{
		friendliness = npc.stats.Friendliness;
		loyalty = npc.stats.Loyalty;
		aggression = npc.stats.Aggression;
		socialDelay = new TickTimer(Socialize, Random.Range(0, 10 - friendliness.GetValue()));
	}

	private void OnDestroy()
	{
		socialDelay.Stop();
	}

	public void Interact(NPCSocialBehaviour otherNPC)
	{
		NPCClan otherClan = otherNPC.clan;

		if(loyalty.GetValue() < -10)
			clan = null;

		if(clan == null)
		{
			if(otherClan == null && friendliness.GetValue() > 0)
			{
				clan = NPCManager.Instance.CreateClan();
				otherNPC.SwapClan(clan);
			}
			else if(otherClan != null)
			{
				if(!SwapClan(otherClan) && friendliness.GetValue() > 3)
					clan = NPCManager.Instance.CreateClan();
			}
		}
		else //clan != null
		{
			if(otherClan == null)
			{
				otherNPC.SwapClan(clan);
			}
			else //otherClan != null
			{
				if(clan != otherClan) //Checking if we should notify our clan about theirs
				{
					clan.Notify(otherClan, ClaculateDifferences(otherNPC));
				}

				if(clan.IsEnemy(otherClan))
				{
					if(!npc.stateManager.IsState(npc.stateManager.CombatState))
						//npc.stateManager.SwitchState(npc.stateManager.CombatState);
					return;
				}
			}
		}

		ModifyStats();
	}

	private void Socialize()
	{
		if(clan != null && loyalty.GetValue() > 10 || npc == null)
			return;

		NPC otherNPC = npc.FindNearbyNPC();
		if(otherNPC ==  null)
			return;
		else
			otherNPC.social.Interact(this);

		socialDelay.Restart(Random.Range(5, 10 - friendliness.GetValue()));
	}

	private bool SwapClan(NPCClan newClan)
	{
		float minSwap = clan == null? 1 + friendliness.GetValue() : -loyalty.GetValue();
		float maxSwap = friendliness.GetValue();
		float swapValue = Random.Range(minSwap, maxSwap);
		
		if(swapValue > 0f)
		{
			clan = newClan;
			return true;
		} else
		{
			loyalty.AddModifier(1);
			return false;
		}
	}

	private void ModifyStats()
	{
		friendliness.AddModifier(Random.Range(-1, 2));
		if(clan != null)
			loyalty.AddModifier(Random.Range(-1, 2));
		aggression.AddModifier(Random.Range(0, 2));

		npc.stats.ClampStats();
	}

	private int ClaculateDifferences(NPCSocialBehaviour otherNPC)
	{
		int differrences = 0;
		differrences += Mathf.Abs(friendliness.GetValue() - otherNPC.friendliness.GetValue());
		differrences += Mathf.Abs(loyalty.GetValue() - otherNPC.loyalty.GetValue());
		differrences += Mathf.Abs(aggression.GetValue() - otherNPC.aggression.GetValue());

		if(differrences <= 5)
			return 1;
		else if(differrences >= 10)
			return -1;

		return 0;
	}
}
