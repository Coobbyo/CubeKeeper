using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* From Brackeys */

[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour
{
	public float attackRate = 1f;
	private float attackCountdown = 0f;

	private CharacterStats myStats;
	protected CharacterStats enemyStats;

	public event System.Action DoAttack;

	private void Awake()
	{
		myStats = GetComponent<CharacterStats>();
	}

	private void Update()
	{
		attackCountdown -= Time.deltaTime;
	}

	public void Attack(CharacterStats enemyStats)
	{
		if(attackCountdown <= 0f)
		{
			this.enemyStats = enemyStats;

			DoAttack();

			attackCountdown = 1f / attackRate;

			//StartCoroutine(DoDamage(enemyStats, 0.6f));
		}
	}

	//This will be the meelee damage if that gets implemented
	IEnumerator DoDamage(CharacterStats stats, float delay)
    {
		print ("Start");
		yield return new WaitForSeconds (delay);

		Debug.Log (transform.name + " swings for " + myStats.Damage.GetValue () + " damage");
		enemyStats.TakeDamage (myStats.Damage.GetValue ());
	}
}