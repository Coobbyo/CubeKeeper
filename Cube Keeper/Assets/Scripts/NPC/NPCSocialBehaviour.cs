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

	private Timer socialDelay;

	private void Awake()
	{
		npc = GetComponent<NPC>();
	}

	private void Start()
	{
		friendliness = npc.stats.Friendliness;
		loyalty = npc.stats.Loyalty;
		aggression = npc.stats.Aggression;
		socialDelay = new Timer(Socialize, Random.Range(0f, 10f - friendliness.GetValue()));
	}

	private void Update()
	{
		socialDelay.Decrement();
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
					//I should probably build a seperate function for this
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

				if(clan.IsEnemy(otherClan))
				{
					npc.state = NPC.State.Combat;
					return;
				}
			}
		}

		friendliness.AddModifier(Random.Range(-1, 2));
		if(clan != null)
			loyalty.AddModifier(Random.Range(-1, 2));
		aggression.AddModifier(Random.Range(-1, 2));

		npc.stats.ClampStats();
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
}
