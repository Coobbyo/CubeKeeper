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
    [SerializeField] protected StructureData data;

    private void OnDestroy()
	{
		//Debug.Log("Structure Destroied from: " + Clan.ToString());

        Clan.builder.RemoveStructure(this);
	}

    public StructureData GetData()
    {
        return data;
    }
}
