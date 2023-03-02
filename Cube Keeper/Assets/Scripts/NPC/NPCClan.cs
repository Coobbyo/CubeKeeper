using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCClan
{
	private Color color;
	private List<NPCSocialBehaviour> members = new List<NPCSocialBehaviour>();
	private Dictionary<NPCClan, int> socialIndex = new Dictionary<NPCClan, int>();
	private List<NPCClan> friends = new List<NPCClan>();
	private List<NPCClan> enemies = new List<NPCClan>();

	public NPCClan(NPCSocialBehaviour founder)
	{
		AddMember(founder);
		color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
	}

	public NPCClan(NPCSocialBehaviour founder, Color color)
	{
	   AddMember(founder);
		this.color = color;
	}

	public void AddMember(NPCSocialBehaviour npc)
	{
		members.Add(npc);
	}

	public void RemoveMember(NPCSocialBehaviour npc)
	{
		members.Remove(npc);
	}

	public Color GetColor()
	{
		return color;
	}

	public Vector3 GetCenterPoint()
	{
		Vector3 Totalposition = Vector3.zero;
		foreach (NPCSocialBehaviour npc in members)
		{
			Totalposition += npc.transform.position;
		}

		return Totalposition / GetClanSize();
	}

	public int GetClanSize()
	{
		return members.Count;
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
}
