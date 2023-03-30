using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* From Brackeys */

[RequireComponent(typeof(Damageable))]
public class Combat : MonoBehaviour
{
	public float attackRate = 1f;
	private float attackCountdown = 0f;

	protected Damageable myStats;
	public Damageable target;

	public event System.Action DoAttack;

	virtual public void Awake()
	{
		myStats = GetComponent<Damageable>();
		if(myStats == null)
			Debug.Log("NULL STATS!");
	}

	private void Update()
	{
		if(attackCountdown > 0)
			attackCountdown -= Time.deltaTime;
	}

	public void Attack(Damageable enemyStats)
	{
		//Debug.Log("Attacking");
		if(attackCountdown <= 0f)
		{
			this.target = enemyStats;

			DoAttack?.Invoke();

			attackCountdown = 1f / attackRate;

			//StartCoroutine(DoDamage(enemyStats, 0.6f));
		}
	}

	//This will be the meelee damage if that gets implemented
	IEnumerator DoDamage(Damageable stats, float delay)
    {
		print ("Start");
		yield return new WaitForSeconds (delay);

		Debug.Log (transform.name + " swings for " + myStats.Damage.GetValue () + " damage");
		target.TakeDamage (myStats.Damage.GetValue ());
	}
}