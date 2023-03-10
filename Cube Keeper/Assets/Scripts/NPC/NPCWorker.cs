using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWorker : MonoBehaviour
{
	[SerializeField] private float workingRange = 5f;
	[SerializeField] private Transform carryPoint;

	private NPC npc;

	private Transform targetTo;
	private Transform targetFrom;
	private Transform currentTarget
	{
		get { return currentTarget; }
		set { currentTarget = value; npc.SetTarget(value.transform); }
	}

	private int loadCapacity = 1;
	private Inventory inventory { get { return npc.inventory; } }

	private void Awake()
	{
		npc = GetComponent<NPC>();
	}

	private void Update()
	{
		switch(npc.state)
		{
			case NPC.State.Roam:
				if(targetTo == null)
					FindResource();
				if(targetFrom == null)
					FindStorage();

				if(targetTo != null && targetFrom != null)
				{
					currentTarget = targetFrom;
					npc.state = NPC.State.Work;
				}
				break;
			case NPC.State.Work:
				if(targetTo == null || targetFrom == null)
					npc.state = NPC.State.Roam;

				if(Vector3.Distance(transform.position, currentTarget.position) <= npc.interactRange)
				{
					if(currentTarget == targetTo)
						Deposit();
					else if(currentTarget == targetFrom)
						Withdraw();
				}
				break;
		}
	}

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
			targetFrom = null;
		else
		{
			RawResource foundResource = nearbyResources[Random.Range(0, nearbyResources.Count)];
			targetFrom = foundResource.transform;
		}
			
	}

	public void FindStorage()
	{
		var nearbyStorage = new List<Storage>();
		Collider[] colliderArray = Physics.OverlapSphere(transform.position, workingRange);
		foreach(Collider collider in colliderArray)
		{
			if(collider.TryGetComponent(out Storage storage))
			{
				nearbyStorage.Add(storage);
			}
		}

		if(nearbyStorage.Count <= 0)
			targetTo = null;
		else
		{
			Storage foundStorage = nearbyStorage[Random.Range(0, nearbyStorage.Count)];
			targetFrom = foundStorage.transform;
		}
	}

	public void Withdraw()
	{
		if(IsFull())
			currentTarget = targetFrom;
		
		IInventory inv = currentTarget.GetComponent<IInventory>();
		inv.Remove(inv.GetItem());
		inventory.Add(inv.GetItem());

		Instantiate(inv.GetItem().prefab, carryPoint);
	}

	public void Deposit()
	{
		if(inventory.items.Count == 0)
			currentTarget = targetTo;

		IInventory inv = currentTarget.GetComponent<IInventory>();

		if(inv.IsFull())
		{
			targetFrom = null;
			return;
		}	

		inv.Add(inv.GetItem());
		inventory.Remove(inv.GetItem());

		foreach (Transform child in carryPoint)
		{
			Destroy(child.gameObject);
		}
	}

	public bool IsFull()
	{
		int inventorySize = 0;
		foreach (Item item in inventory.items)
		{
			for (int i = 0; i < item.stackSize; i++)
			{
				inventorySize++;
			}
		}
		return inventorySize == loadCapacity ? true : false;
	}
}
