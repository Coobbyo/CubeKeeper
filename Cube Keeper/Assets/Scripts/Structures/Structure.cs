using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    private NPCClan clan;
    public NPCClan Clan
    {
        get { return clan; }
        set
        {
            clan = value;
            foreach (MeshRenderer mesh in displayMeshes)
            {
                if(clan == null)
                    mesh.material.color = NPC.Clanless.color;
                else
                    mesh.material.color = clan.Color;
            }
        }
    }

    [SerializeField] private MeshRenderer[] displayMeshes;
    [SerializeField] private StructureData data;

    public StructureData GetData()
    {
        return data;
    }
}
