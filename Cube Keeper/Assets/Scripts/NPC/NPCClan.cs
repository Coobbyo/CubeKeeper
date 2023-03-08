using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCClan
{
	private string id;
	private Color color;
	
	private List<NPC> members = new List<NPC>();
	private Dictionary<NPCClan, int> socialIndex = new Dictionary<NPCClan, int>();
	private List<NPCClan> friends = new List<NPCClan>();
	private List<NPCClan> enemies = new List<NPCClan>();

	public NPCClan()
	{
		id = "Clan " + Random.Range(0, 10000);
		color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

		NPCManager.Instance.AddClan(this);
	}
	public NPCClan(Color color)
	{
		id = "Clan " + Random.Range(0, 10000);
		this.color = color;

		NPCManager.Instance.AddClan(this);
	}
	public NPCClan(string id)
	{
		this.id = id;
		color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

		NPCManager.Instance.AddClan(this);
	}
	public NPCClan(string id, Color color)
	{
		this.id = id;
		this.color = color;

		NPCManager.Instance.AddClan(this);
	}

	public void VerifyMembers()
	{
		foreach (NPC npc in members)
		{
			if(npc.GetClan() != this)
			{
				//Eventually I need to send them to he proper clan...
				members.Remove(npc);
				//npc.social.LeaveClan();
			}
		}
	}

	public void AddMember(NPC npc)
	{
		members.Add(npc);
	}

	public void RemoveMember(NPC npc)
	{
		members.Remove(npc);
		if(members.Count <= 0)
			NPCManager.Instance.RemoveCLan(this);
	}

	public Color GetColor()
	{
		return color;
	}

	public Vector3 GetCenterPoint()
	{
		Vector3 Totalposition = Vector3.zero;
		foreach (NPC npc in members)
		{
			Totalposition += npc.transform.position;
		}

		return Totalposition / GetClanSize();
	}

	public int GetClanSize()
	{
		return members.Count;
	}

	public int GetNumFriends()
	{
		return friends.Count;
	}

	public int GetNumEnemies()
	{
		return enemies.Count;
	}

	public List<NPC> GetMembers()
	{
		return members;
	}

	public List<NPCClan> GetFriends()
	{
		return friends;
	}

	public List<NPCClan> GetEnemies()
	{
		return enemies;
	}

	public bool IsFriend(NPCClan otherClan)
	{
		foreach(NPCClan clan in friends)
		{
			if(clan == otherClan)
				return true;
		}

		return false;
	}

	public bool IsEnemy(NPCClan otherClan)
	{
		foreach(NPCClan clan in friends)
		{
			if(clan == otherClan)
				return true;
		}

		return false;
	}

	public void Notify(NPCClan clan, int socialPoints)
	{
		if(clan == null)
		{
			Debug.Log("clan should not be null here");
			return;
		}
		if(socialIndex.ContainsKey(clan))	
		{
			socialIndex.TryGetValue(clan, out int socialValue);
			socialValue += socialPoints;

			if(socialValue >= 5)
			{
				if(IsFriend(clan))
					return;

				friends.Add(clan);
			}
			else if(socialValue < 5)
			{
				if(!IsFriend(clan))
					return;

				friends.Remove(clan);
			}

			if(socialValue <= -5)
			{
				if(IsEnemy(clan))
					return;

				friends.Add(clan);
			}
			else if(socialValue > -5)
			{
				if(!IsEnemy(clan))
					return;

				friends.Remove(clan);
			}
		}
		else
		{
			socialIndex.TryAdd(clan, socialPoints);
		}
	}

	override public string ToString()
	{
		return id;
	}
}
