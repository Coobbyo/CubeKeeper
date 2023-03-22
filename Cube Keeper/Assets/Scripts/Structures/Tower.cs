using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Structure
{
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private Transform firePoint;

	private Combat combat;

	private float range = 10f;

	override public void Awake()
	{
		base.Awake();
		stats.OnHealthReachedZero += Crumble;
		combat = GetComponent<Combat>();
	}

	private void Start()
	{
		combat.DoAttack += Shoot;
	}

	private void Update()
	{
		if(combat.target == null)
			FindTarget();
		else
			combat.Attack(combat.target);
	}

	private void FindTarget()
	{
		var targets = new List<Damageable>();
		Collider[] colliderArray = Physics.OverlapSphere(transform.position, range);
		foreach(Collider collider in colliderArray)
		{
			if(collider.TryGetComponent(out Damageable possibleTarget))
			{
				if(Clan.IsEnemy(possibleTarget.Clan))
					targets.Add(possibleTarget);
			}
		}

		if(targets.Count == 0)
			return;

		combat.target = targets[Random.Range(0, targets.Count - 1)];
	}

	private void Shoot()
	{
		GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		bulletGO.GetComponentInChildren<MeshRenderer>().material.color = Clan.Color;
		Bullet bullet = bulletGO.GetComponent<Bullet>();
		bullet.SetDamage(stats.Damage.GetValue());

		if(bullet != null)
			bullet.Seek(combat.target.HitPoint);
	}
}
