using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Structure
{
	public int Population { get; private set; }

	override public void Awake()
	{
		base.Awake();
		stats.OnHealthReachedZero += Crumble;
	}

	private void Start()
	{
		Population = 5;
		Clan.PopulationCap.AddModifier(Population);
	}

    public override void Crumble()
    {
		Clan.PopulationCap.RemoveModifier(Population);
        base.Crumble();
    }
}
