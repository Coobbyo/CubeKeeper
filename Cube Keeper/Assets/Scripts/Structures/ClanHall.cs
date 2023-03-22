using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClanHall : Structure
{
	[SerializeField] private List<Tower> towers;

	private void Start()
	{
		foreach (Tower tower in towers)
		{
			tower.Clan = Clan;
		}
	}

	public override void OnDestroy()
	{
		Clan.builder.hall = null;
		base.OnDestroy();
	}

    public override void Crumble()
    {
        base.Crumble();
    }
}
