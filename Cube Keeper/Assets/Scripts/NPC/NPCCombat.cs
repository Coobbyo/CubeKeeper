using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCombat : CharacterCombat
{
	private NPC npc;
	private NPC target
	{
		get { return target; }
		set { target = value; npc.SetTarget(value.transform); }
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

		if(target == null)
			FindTarget();

		if(target == null)
			return;

		Attack(target.stats);
	}

	private void FindTarget()
	{
		List<NPC> otherNPCs = npc.FindNearbyNPCs();

		foreach (NPC otherNPC in otherNPCs)
		{
			if(npc.stats.Aggression.GetValue() > 0f && npc.clan.IsEnemy(otherNPC.clan) || //If aggressive then attack
				(npc.stats.Aggression.GetValue() < 0f && //If non aggessive heal
				(npc.clan == otherNPC.clan || npc.clan.IsFriend(otherNPC.clan)) && //Check if friend or clan mate
				otherNPC.stats.CurrentHealth <= otherNPC.stats.MaxHealth.GetValue())) //Check if less than max health
			{
				target = otherNPC;
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
		bullet.SetDamage(npc.stats.Damage.GetValue());

		if(bullet != null)
			bullet.Seek(enemyStats.transform);
	}
}
