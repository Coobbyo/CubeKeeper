using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCombat : CharacterCombat
{
    private NPC npc;
    private NPC target;

    [SerializeField] private Transform bulletPrefab;

    private void Awake()
    {
        npc = GetComponent<NPC>();
    }

    private void Update()
    {
        if(npc.state != NPC.State.Combat)
            return;

        if(target == null)
            FindTarget();

        if(target == null)
            return;

        Attack(target.stats);
    }

    private void FindTarget()
    {
        List<NPC> otherNPCs = npc.FindNearbyNPCs();

        foreach (NPC otherNPC in otherNPCs)
        {
            if(npc.social.GetClan().IsEnemy(otherNPC.social.GetClan()))
            {
                target = otherNPC;
                return;
            }
        }

        npc.state = NPC.State.Move;
    }
}
