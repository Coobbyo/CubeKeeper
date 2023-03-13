using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWorker : MonoBehaviour
{
	[SerializeField] private float workingRange = 5f;
	[SerializeField] private Transform carryPoint;

	private NPC npc;

	private Transform targetFrom;
	private Transform targetTo;
	
	private Transform currentTarget
	{
		get { return npc.target; }
		set { npc.SetTarget(value); }
	}

	private int loadCapacity = 1;
	private Inventory inventory { get { return npc.inventory; } }

	private Timer findDelay = new Timer();
	private Timer workDelay = new Timer();

	private void Awake()
	{
		npc = GetComponent<NPC>();
	}

	private void Start()
	{
		findDelay.Set(FindWork, Random.Range(0f, 5f));
		workDelay.Set(DoWork, Random.Range(0f, 1f));
	}

	private void Update()
	{
		switch(npc.state)
		{
			case NPC.State.Roam:
				findDelay.Decrement();
				break;
			case NPC.State.Work:
				workDelay.Decrement();
				break;
		}
	}

	private void FindWork()
	{
		if(targetFrom == null)
			FindResource();
		if(targetTo == null)
			FindStorage();
	
		if(targetFrom != null && targetTo != null)
		{
			VerifyTargets();
			currentTarget = targetFrom;
			npc.state = NPC.State.Work;
			//Debug.Log("State Changed to work");
		}

		findDelay.Restart(Random.Range(0f, 5f));
	}

	public void FindResource()
	{
		//Debug.Log("Looking for resource");
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
		{
			//Debug.Log("No Resources found");
			//targetFrom = null;
		}
		else
		{
			//Debug.Log("Resource Found!");
			RawResource foundResource = nearbyResources[Random.Range(0, nearbyResources.Count)];
			targetFrom = foundResource.transform;
		}
			
	}

	public void FindStorage()
	{
		//Debug.Log("Looking for Storage");
		var nearbyStorage = new List<SingleStorage>();
		Collider[] colliderArray = Physics.OverlapSphere(transform.position, workingRange);
		foreach(Collider collider in colliderArray)
		{
			if(collider.TryGetComponent(out SingleStorage storage))
			{
				nearbyStorage.Add(storage);
			}
		}

		if(nearbyStorage.Count <= 0)
		{
			//Debug.Log("No storages found");
			//targetTo = null;
		}
		else
		{
			//Debug.Log("Storage Found!");
			SingleStorage foundStorage = nearbyStorage[Random.Range(0, nearbyStorage.Count)];
			targetTo = foundStorage.transform;
		}
	}

	private void DoWork()
	{
		if(targetFrom == null)
		{
			if(carryPoint.childCount > 0)
				currentTarget = targetTo;
			else
			{
				npc.state = NPC.State.Roam;
				return;
			}
		}
		
		if(targetTo == null)
		{
			npc.state = NPC.State.Roam;
			return;
		}

		//Debug.Log("Working!");
		if(Vector3.Distance(transform.position, currentTarget.position) <= npc.interactRange)
		{
			//Debug.Log("close enough");
			if(currentTarget == targetFrom)
				Withdraw();
			else if(currentTarget == targetTo)
				Deposit();
		}

		workDelay.Restart(Random.Range(0.5f, 1f));
	}

	public void Withdraw()
	{
		if(IsFull())
		{
			currentTarget = targetTo;
			return;
		}
		
		IInventory inv = targetFrom.GetComponent<IInventory>();
		inv.Remove(inv.GetItem());
		inventory.Add(inv.GetItem());

		Instantiate(inv.GetItem().prefab, carryPoint);
	}

	public void Deposit()
	{
		if(inventory.items.Count == 0)
		{
			currentTarget = targetFrom;
			return;
		}

		IInventory inv = targetTo.GetComponent<IInventory>();

		if(inv.IsFull())
		{
			targetTo = null;
			return;
		}

		if(inv.GetItem() == null)
		{
			inv.Add(targetFrom.GetComponent<IInventory>().GetItem());
		}
		else
		{
			inv.Add(inv.GetItem());
		}

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
		return inventorySize >= loadCapacity ? true : false;
	}

	private void VerifyTargets()
	{
		if(targetFrom == null || targetTo == null)
			return;

		IInventory invFrom = targetFrom.GetComponent<IInventory>();
		IInventory invTo = targetTo.GetComponent<IInventory>();

		if(invTo.GetItem() == null)
			return;

		if(carryPoint.childCount > 0) //This is checking the item we are carying
		{
			if(inventory.items.Count == 0)
				Debug.LogError("Are we carrying something with an empty inventory?");
			
			//Is the item we have the same as where we are withdrawing?
			if(inventory.Get(invFrom.GetItem()) == null)
			{
				targetFrom = null;
			}

			//Is the item we have te smae as where we are depositing?
			if(inventory.Get(invTo.GetItem()) == null)
			{
				targetTo = null;
			}
		}

		if(invFrom.GetItem() != invTo.GetItem())
			targetFrom = null;

		if(invTo.IsFull())
			targetTo = null;

		
	}
}
