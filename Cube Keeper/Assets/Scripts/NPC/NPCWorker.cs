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

	private Timer findDelay;
	private Timer workDelay;

	private void Awake()
	{
		npc = GetComponent<NPC>();
	}

	private void Start()
	{
		findDelay = new Timer(FindWork, Random.Range(0f, 1f));
		//findDelay.ShowLogs = true;
		workDelay = new Timer(DoWork, Random.Range(0f, 1f));

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
		if(npc.clan == null)
			return;

		if(targetFrom == null)
			FindFromTarget();
		if(targetTo == null)
			FindToTarget();
	
		if(targetFrom != null && targetTo != null)
		{
			VerifyTargets();
			currentTarget = targetFrom;
			npc.state = NPC.State.Work;
			workDelay.Restart(Random.Range(0, 1f));
			//Debug.Log("State Changed to work");
		}

		findDelay.Restart(Random.Range(0f, 5f));
	}

	public void FindFromTarget()
	{
		//Debug.Log("Finding From");

		RawResource resource = FindResource();
		if(resource != null)
		{
			targetFrom = resource.transform;
			//Debug.Log("TargetFrom found (Resource)");
			return;
		}

		Storage storage = FindStorage();
		if(storage != null)
		{
			if(storage.Clan == npc.clan && storage.IsHalfFull())
			{
				targetFrom = storage.transform;
				//Debug.Log("TargetFrom found (Storage)");
				if(targetFrom != targetTo)
					return;
			}
		}

		//Debug.Log("No from target found");
	}

	public void FindToTarget()
	{
		//Debug.Log("Finding To");

		Storage storage = FindStorage();
		if(storage != null)
		{
			//Debug.Log("To Storage not null!");
			if(storage.Clan == npc.clan && !storage.IsFull())
			{
				targetTo = storage.transform;
				//Debug.Log("TargetTo Found");
				return;
			}
		}

		//Debug.Log("No To target found");
	}

	public RawResource FindResource()
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

		if(nearbyResources.Count > 0)
		{
			//Debug.Log("Resource Found!");
			RawResource foundResource = nearbyResources[Random.Range(0, nearbyResources.Count)];
			return foundResource;
		}
		else
		{
			return null;
		}	
	}

	public Storage FindStorage()
	{
		//Debug.Log("Looking for Storage");
		var nearbyStorage = new List<Storage>();
		Collider[] colliderArray = Physics.OverlapSphere(transform.position, workingRange);
		foreach(Collider collider in colliderArray)
		{
			if(collider.TryGetComponent(out Storage storage))
			{
				nearbyStorage.Add(storage);
			}
		}

		if(nearbyStorage.Count > 0)
		{
			//Debug.Log("Storage Found!");
			Storage foundStorage = nearbyStorage[Random.Range(0, nearbyStorage.Count)];
			return foundStorage;
		}
		else
		{
			List<Structure> clanStorage = npc.clan.builder.GetStructures(BuildManager.Build.Storage);
			if(/*clanStorage == null || */clanStorage.Count <= 0)
			{
				//Debug.Log("Clan storage is either null or empty");
				return null;
			}

			
				
			Storage foundStorage = clanStorage[Random.Range(0, clanStorage.Count)].GetComponent<Storage>();
			return foundStorage;
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
				//Debug.Log("Roaming From"); //This one keeps happening
				npc.state = NPC.State.Roam;
				return;
			}
		}
		
		if(targetTo == null)
		{
			//Debug.Log("Roaming To");
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

		workDelay.Restart(Random.Range(0, 1f));
	}

	public void Withdraw()
	{
		if(IsFull())
		{
			currentTarget = targetTo;
			return;
		}
		
		IInventory inv = targetFrom.GetComponent<IInventory>();
		ItemData resource = inv.GetResource();
		inv.Remove(resource);
		inventory.Add(resource);

		Instantiate(resource.prefab, carryPoint);
	}

	public void Deposit()
	{
		//Things to keep in mind
		//Are we even carying anything
		//Does where we want to put it match where we are going?

		//We don't actually have anything so we should go grab something
		if(inventory.items.Count == 0)
		{
			currentTarget = targetFrom;
			return;
		}

		IInventory inv = targetTo.GetComponent<IInventory>();

		//We don't want to keep putting things here
		if(inv.IsFull())
		{
			targetTo = null;
			return;
		}

		//If it doesn't have a specified item we shoud make it the same as where we are pulling from
		if(inv.GetResource() == null)
		{
			if(targetFrom == null)
			{
				//Debug.Log("Target From is null");
				return;
			}
			inv.Add(targetFrom.GetComponent<IInventory>().GetResource());
		}
		else
		{
			inv.Add(inv.GetResource());
		}

		inventory.Remove(inv.GetResource());

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
			for (int i = 0; i < item.StackSize; i++)
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

		if(targetFrom == targetTo)
		{
			//Debug.Log("Nulling From");
			targetFrom = null;
			return;
		}

		IInventory invFrom = targetFrom.GetComponent<IInventory>();
		IInventory invTo = targetTo.GetComponent<IInventory>();

		if(invTo.GetResource() == null)
			return;

		if(carryPoint.childCount > 0) //This is checking the item we are carying
		{
			if(inventory.items.Count == 0)
				//Debug.LogError("Are we carrying something with an empty inventory?");
			
			//Is the item we have the same as where we are withdrawing?
			if(inventory.Get(invFrom.GetResource()) == null)
			{
				//Debug.Log("Nulling From");
				targetFrom = null;
			}

			//Is the item we have the same as where we are depositing?
			if(inventory.Get(invTo.GetResource()) == null)
			{
				targetTo = null;
			}
		}

		if(invFrom.GetResource() != invTo.GetResource())
		{
			//Debug.Log("Nulling From"); //This one keeps happening
			targetFrom = null;
		}

		if(invTo.IsFull())
			targetTo = null;
	}
}
