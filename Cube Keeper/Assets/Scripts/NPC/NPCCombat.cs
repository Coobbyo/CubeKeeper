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

	override public void Awake()
	{
		base.Awake();
		npc = GetComponent<NPC>();
	}

	private void Start()
	{
		DoAttack += Shoot;

		attackRate += Mathf.Abs(npc.stats.Aggression.GetValue());
	}

	private void Shoot()
	{
		GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		bulletGO.GetComponentInChildren<MeshRenderer>().material.color = npc.colorDisplay.material.color;
		Bullet bullet = bulletGO.GetComponent<Bullet>();

		if(bullet == null) Debug.Log("Bullet null");
		if(myStats == null) Debug.Log("stats null");
		if(myStats.Damage ==  null) Debug.Log("stat (single) null");

		bullet.SetDamage(myStats.Damage.GetValue());

		if(bullet != null)
			bullet.Seek(target.HitPoint);
	}
}
