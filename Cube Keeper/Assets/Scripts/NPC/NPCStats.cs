using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStats : Damageable
{
	//Social Stats
	public Stat Friendliness;// { get; private set; }
	public Stat Loyalty;// { get; private set; }
	public Stat Aggression;// { get; private set; }

	//Work Stats
	public Stat Idleness;// { get; private set; }

	private void Start()
	{
		Friendliness.baseValue = Random.Range(0, 5);
		Loyalty.baseValue = Random.Range(-10, 10);
		Aggression.baseValue = Random.Range(0, 2);
		Idleness.baseValue = Random.Range(-5, 5);

		MaxHealth.baseValue = 10;
		CurrentHealth = MaxHealth.GetValue();
		Damage.baseValue = Aggression.baseValue;
		Armor.baseValue = 0;
	}

	public void ClampStats()
	{
		int friendlinessCap = 10;
		int loyaltyCap = 15;
		int agressionCap = 10;
		int idlenessCap = 10;

		if(Friendliness.GetValue() > friendlinessCap ||
			Friendliness.GetValue() < -friendlinessCap)
		{
			Friendliness.ClearModifiers();
		}

		if(Loyalty.GetValue() > loyaltyCap ||
			Loyalty.GetValue() < -loyaltyCap)
		{
			Loyalty.ClearModifiers();
		}

		if(Aggression.GetValue() > agressionCap ||
			Aggression.GetValue() < -agressionCap)
		{
			Aggression.ClearModifiers();
		}

		if(Idleness.GetValue() > idlenessCap ||
			Idleness.GetValue() < -idlenessCap)
		{
			Idleness.ClearModifiers();
		}
	}
}
