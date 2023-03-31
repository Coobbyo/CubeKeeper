using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Structure : MonoBehaviour
{
	private NPCClan clan;
	public NPCClan Clan
	{
		get { return clan; }
		set
		{
			clan = value;
			stats.Clan = Clan;
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
	protected Damageable stats;
	protected bool IsCrumbling;

	virtual public void Awake()
	{
		stats = GetComponent<Damageable>();
		stats.OnHealthReachedZero += Crumble;
	}

	virtual public void OnDestroy()
	{
		//if(Clan != null) Debug.Log(name + " Destroied from: " + Clan.ToString());
		//else Debug.Log(name + " Destroied");

		if(Clan != null)
			Clan.builder.RemoveStructure(this);
	}

	public StructureData GetData()
	{
		return data;
	}

	virtual public void Crumble()
	{
		IsCrumbling = true;
		if(gameObject != null)
			Destroy(gameObject);
	}
}
