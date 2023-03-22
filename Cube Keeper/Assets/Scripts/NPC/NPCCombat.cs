using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCombat : Combat
{
	private NPC npc;

	public Transform Target
	{
		get { return target == null ? null : target.transform; }
		set { target = value.GetComponent<Damageable>(); npc.SetTarget(value.transform); }
	}

	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private Transform firePoint;

	private void Awake()
	{
		npc = GetComponent<NPC>();
	}

	private void Start()
	{
		DoAttack += Shoot;

		attackRate += Mathf.Abs(npc.stats.Aggression.GetValue());
	}

	private void Update()
	{
		if(npc.state != NPC.State.Combat)
			return;

		if(Target == null)
			FindTarget();
		else
			Attack(target);
	}

	private void FindTarget()
	{
		List<NPC> otherNPCs = npc.FindNearbyNPCs();
		var targets = new List<Damageable>();

		foreach(NPC otherNPC in otherNPCs)
		{
			if(IsTarget(otherNPC)) //Check if less than max health
			{
				targets.Add(otherNPC.stats);
				return;
			}
			else
			{
				npc.state = NPC.State.Roam;
				return;
			}
		}

		npc.state = NPC.State.Roam;
	}

	private void Shoot()
	{
		GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		bulletGO.GetComponentInChildren<MeshRenderer>().material.color = npc.colorDisplay.material.color;
		Bullet bullet = bulletGO.GetComponent<Bullet>();
		bullet.SetDamage(myStats.Damage.GetValue());

		if(bullet != null)
			bullet.Seek(target.HitPoint);
	}

	private bool IsTarget(NPC otherNPC)
	{
		if(npc.stats.Aggression.GetValue() > 0f && npc.clan.IsEnemy(otherNPC.clan))
			return true;
		if(npc.stats.Aggression.GetValue() > 5f && otherNPC.clan == null)
			return true;

		if(npc.stats.Damage.GetValue() > 0f)
			return false;

		if(npc.IsFriend(otherNPC) &&
			otherNPC.stats.CurrentHealth <= otherNPC.stats.MaxHealth.GetValue()) //Has less than full health
			return true;

		return false;
	}
}
