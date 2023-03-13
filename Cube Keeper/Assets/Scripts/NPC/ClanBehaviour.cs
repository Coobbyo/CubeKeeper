using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClanBehaviour : MonoBehaviour
{
    private NPCClan clan;
    public NPCClan Clan
    {
        get { return clan; }
        set
        {
            clan = value;
        }
    }

    private void Start()
    {
        clan = new NPCClan();
    }
}
