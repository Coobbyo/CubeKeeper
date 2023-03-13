using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStats : CharacterStats
{
	//Social Stats
    public Stat Friendliness;// { get; private set; }
	public Stat Loyalty;// { get; private set; }
	public Stat Aggression;// { get; private set; }

	//Work Stats
	//Idleness

    private void Start()
    {
        Friendliness.baseValue = Random.Range(0, 5);
		Loyalty.baseValue = Random.Range(-10, 10);
		Aggression.baseValue = Random.Range(-2, 2);

		Damage.baseValue = Aggression.baseValue;
    }

    public void ClampStats()
	{
		int friendlinessCap = 10;
		int loyaltyCap = 15;
		int agressionCap = 10;

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
	}
}
