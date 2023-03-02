using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSocialBehaviour : MonoBehaviour
{
	public Stat friendliness { get; private set; }
	public Stat loyalty { get; private set; }
	public Stat aggression { get; private set; }

	[SerializeField] private Material Clanless;
	

	private NPC npc;
	private NPCClan clan;
	private Timer socialDelay = new Timer();

	private void Awake()
	{
		npc = GetComponent<NPC>();
	}

	private void Start()
	{
		friendliness = npc.stats.Friendliness;
		loyalty = npc.stats.Loyalty;
		aggression = npc.stats.Aggression;
		socialDelay.Set(Socialize, Random.Range(0f, 10f - friendliness.GetValue()));
	}

	private void Update()
	{
		socialDelay.Decrement();
	}

	public void Interact(NPCSocialBehaviour otherNPC)
	{
		NPCClan otherClan = otherNPC.GetClan();

		if(clan != null && clan.IsEnemy(otherClan))
		{
			npc.state = NPC.State.Combat;
			return;
		}

		if(loyalty.GetValue() < -10)
		{
			LeaveClan();
			
			if(friendliness.GetValue() > 0)
				JoinClan(new NPCClan(this));
		}

		if(clan == null && otherClan == null && friendliness.GetValue() > 0)
			JoinClan(new NPCClan(this));

		if(clan == null && otherClan != null)
			SwapClan(otherClan);
		else if(clan != null && otherClan == null)
			otherNPC.SwapClan(clan);

		friendliness.AddModifier(Random.Range(-1, 2));
		if(clan != null)
			loyalty.AddModifier(Random.Range(-1, 2));
		aggression.AddModifier(Random.Range(-1, 2));

		if(clan != otherClan && clan != null && otherClan != null)
		{
			if(Mathf.Abs(friendliness.GetValue() - otherNPC.friendliness.GetValue()) < 2 ||
				Mathf.Abs(loyalty.GetValue() - otherNPC.loyalty.GetValue()) < 2 ||
				Mathf.Abs(aggression.GetValue() - otherNPC.aggression.GetValue()) < 2)
			{
				clan.Notify(otherClan, 1);
			}
			else if(Mathf.Abs(friendliness.GetValue() - otherNPC.friendliness.GetValue()) > 5 ||
				Mathf.Abs(loyalty.GetValue() - otherNPC.loyalty.GetValue()) > 5 ||
				Mathf.Abs(aggression.GetValue() - otherNPC.aggression.GetValue()) > 5)
			{
				clan.Notify(otherClan, -1);
			}
		}

		ClampStats();
	}

	private void Socialize()
	{
		//if(npc.state == NPC.State.Combat)
			//return;
		
		if(clan != null && loyalty.GetValue() > 10)
			return;

		NPC otherNPC = npc.FindNearbyNPC();
		if(otherNPC ==  null)
			return;
		else
			otherNPC.social.Interact(this);

		socialDelay.Restart(Random.Range(5f, 10f - friendliness.GetValue()));
	}

	private bool SwapClan(NPCClan newClan)
	{
		float swapValue = Random.Range(clan == null? 1 + friendliness.GetValue() : -loyalty.GetValue(), friendliness.GetValue());
		if(swapValue > 0f)
		{
			LeaveClan();
			JoinClan(newClan);
			return true;
		} else
		{
			loyalty.AddModifier(1);
			return false;
		}
	}

	public NPCClan GetClan()
	{
		return clan;
	}

	private void JoinClan(NPCClan newClan)
	{
		loyalty.ClearModifiers();
		loyalty.AddModifier(5);
		npc.colorDisplay.material.color = newClan.GetColor();
		newClan.AddMember(this);
		clan = newClan;
	}

	private void LeaveClan()
	{
		npc.colorDisplay.material = Clanless;
		if(clan != null)
			clan.RemoveMember(this);
		
		clan = null;
	}

	private void ClampStats()
	{
		int friendlinessCap = 10;
		int loyaltyCap = 15;
		int agressionCap = 10;

		if(friendliness.GetValue() > friendlinessCap ||
			friendliness.GetValue() < -friendlinessCap)
		{
			friendliness.ClearModifiers();
		}

		if(loyalty.GetValue() > loyaltyCap ||
			loyalty.GetValue() < -loyaltyCap)
		{
			loyalty.ClearModifiers();
		}

		if(aggression.GetValue() > agressionCap ||
			aggression.GetValue() < -agressionCap)
		{
			aggression.ClearModifiers();
		}
	}
}
