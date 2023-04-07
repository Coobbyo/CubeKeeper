using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCClan
{
	public ClanBehaviour behaviour;
	public string ClanName {get; private set; }
	public Color Color { get; private set; }
	public Stat PopulationCap { get; private set; }
	public int Size
	{
		get
		{
			return Members.Count;
		}
	}
	public Vector3 CenterPoint
	{
		get
		{
			if(builder.hall != null)
				return builder.hall.transform.position;
			
			Vector3 Totalposition = Vector3.zero;
			foreach (NPC npc in Members)
			{
				Totalposition += npc.transform.position;
			}

			return Totalposition / Size;
		}
	}
	
	public List<NPC> Members { get; private set; }
	public List<NPCClan> Friends { get; private set; }
	public List<NPCClan> Enemies { get; private set; }
	public ClanBuilder builder { get; private set; }

	private Dictionary<NPCClan, int> socialIndex;

	private string id;

	public NPCClan()
	{
		Assignments();
	}
	public NPCClan(Color color)
	{
		Assignments();
		Color = color;
	}
	public NPCClan(string name)
	{
		Assignments();
		ClanName = name;

	}
	public NPCClan(string name, Color color)
	{
		Assignments();
		ClanName = name;
		Color = color;
	}
	public NPCClan(ClanData referenceData)
	{
		Assignments();
		id = referenceData.id;
		ClanName = referenceData.clanName;
		Color = referenceData.color;
	}

	private void Assignments()
	{
		id = Random.Range(0, 10000).ToString();
		ClanName = "Clan " + id;
		Color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

		Members = new List<NPC>();
		Friends = new List<NPCClan>();
		Enemies = new List<NPCClan>();
		socialIndex = new Dictionary<NPCClan, int>();
		builder = new ClanBuilder(this);

		PopulationCap = new Stat();
		PopulationCap.baseValue = 10;
	}

	public void VerifyMembers()
	{
		foreach (NPC npc in Members)
		{
			if(npc.clan != this)
			{
				//Eventually I need to send them to he proper clan...
				Members.Remove(npc);
				//npc.social.LeaveClan();
			}
		}
	}

	public bool AddMember(NPC npc)
	{
		if(IsFull())
			return false;
		//Debug.Log(ToString() + " Adding Memeber");
		Members.Add(npc);
		return true;
	}

	public void RemoveMember(NPC npc)
	{
		//Debug.Log(ToString() + " Removeing Memeber");
		Members.Remove(npc);
		if(Members.Count <= 0)
			NPCManager.Instance.RemoveClan(this);
	}

	public bool IsFriend(NPCClan otherClan)
	{
		if(otherClan == this)
			return true;
		
		foreach(NPCClan clan in Friends)
		{
			if(clan == otherClan)
				return true;
		}

		return false;
	}

	public bool IsEnemy(NPCClan otherClan)
	{
		foreach(NPCClan clan in Enemies)
		{
			if(clan == otherClan)
				return true;
		}

		return false;
	}

	public bool IsFull()
	{
		return Size >= PopulationCap.GetValue();
	}

	public NPC GetRandomMemeber()
	{
		if(Members.Count <= 0)
		{
			Debug.Log(ToString() + " should not exist");
			return null;
		}
		return Members[Random.Range(0, Members.Count)];
	}

	public void Search()
	{
		foreach (NPC member in Members)
		{
			member.Search();
		}
	}

	public void Notify(NPCClan clan, int socialPoints)
	{
		int friendThreshold = 1;
		int enemyThreshold = 0;
		if(clan == null)
		{
			Debug.Log("clan should not be null here");
			return;
		}

		if(socialIndex.ContainsKey(clan))	
		{
			socialIndex.TryGetValue(clan, out int socialValue);
			socialValue += socialPoints;

			if(socialValue >= friendThreshold && !IsFriend(clan))
			{
				//Debug.Log(id + " is now friends with " + clan.ToString());
				Friends.Add(clan);
			}
			else if(socialValue < friendThreshold && IsFriend(clan))
			{
				//Debug.Log(id + " had a falling out with " + clan.ToString());
				Friends.Remove(clan);
			}

			if(socialValue <= enemyThreshold && !IsEnemy(clan))
			{
				//Debug.Log(id + " went to war with " + clan.ToString());
				Enemies.Add(clan);
			}
			else if(socialValue > enemyThreshold && IsEnemy(clan))
			{
				//Debug.Log(id + " came to peace with " + clan.ToString());
				Enemies.Remove(clan);
			}
		}
		else
		{
			//Debug.Log(id + " is now tracking " + clan.ToString());
			socialIndex.TryAdd(clan, socialPoints);
		}
	}

	override public string ToString()
	{
		return ClanName;
	}
}
