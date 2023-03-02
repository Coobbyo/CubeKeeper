using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStats : CharacterStats
{
    public Stat Friendliness;// { get; private set; }
	public Stat Loyalty;// { get; private set; }
	public Stat Aggression;// { get; private set; }

    private void Start()
    {
        Friendliness.baseValue = Random.Range(1, 5);
		Loyalty.baseValue = Random.Range(-10, 10);
		Aggression.baseValue = Random.Range(-2, 2);
    }
}
