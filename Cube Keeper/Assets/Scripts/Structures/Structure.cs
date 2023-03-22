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
					mesh.material.color = NPCManager.Instance.Clanless.color;
				else
					mesh.material.color = clan.Color;
			}
		}
	}

	[SerializeField] private MeshRenderer[] displayMeshes;
	[SerializeField] protected StructureData data;

	virtual public void OnDestroy()
	{
		//Debug.Log(name + " Destroied from: " + Clan.ToString());
		if(Clan != null)
			Clan.builder.RemoveStructure(this);
	}

	public StructureData GetData()
	{
		return data;
	}
}
