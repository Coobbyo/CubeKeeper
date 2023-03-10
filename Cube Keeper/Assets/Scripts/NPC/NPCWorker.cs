using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWorker : MonoBehaviour
{
    [SerializeField] private float workingRange = 5f;

    private NPC npc;
    private RawResource source;
    private Storage storageTo;
    private Storage storageFrom;
    private Inventory inventory { get { return npc.inventory; } }


    public void FindResource()
    {
        var nearbyResources = new List<RawResource>();
		Collider[] colliderArray = Physics.OverlapSphere(transform.position, workingRange);
		foreach(Collider collider in colliderArray)
		{
			if(collider.TryGetComponent(out RawResource resource))
			{
				nearbyResources.Add(resource);
			}
		}

		if(nearbyResources.Count <= 0)
			source = null;
		else
			source = nearbyResources[Random.Range(0, nearbyResources.Count)];
    }

    public void Withdraw()
    {}

    public void Deposit()
    {}
}
