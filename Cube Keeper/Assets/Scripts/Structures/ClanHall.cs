using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClanHall : Structure
{
    public override void OnDestroy()
    {
        Clan.builder.hall = null;
        base.OnDestroy();
    }
}
