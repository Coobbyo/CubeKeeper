using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWorker : MonoBehaviour
{
	public float workingRange = 5f;
	public Transform carryPoint;
	public ItemData ItemCarried;

	private NPC npc;

	public Transform targetFrom;
	public Transform targetTo;

	public Transform currentTarget
	{
		get { return npc.target; }
		set { npc.SetTarget(value); }
	}

	private int loadCapacity = 1;
	private Inventory inventory { get { return npc.inventory; } }


	private void Awake()
	{
		npc = GetComponent<NPC>();
	}

	public bool FindWork()
	{
		//TODO: verify that npc's drop off excess resources they may be holding

		if(npc.stateManager.IsState(npc.stateManager.WorkState))
		{
			//Debug.Log("already working");
			return false;
		}
		if (npc.clan == null)
			return false;

		if (targetFrom == null)
			FindFromTarget();
		if (targetTo == null)
			FindToTarget();

		if(targetFrom != null && targetTo != null)
		{
			VerifyTargets();
			currentTarget = targetFrom;
			if(currentTarget != null)
			{
				npc.stateManager.SwitchState(npc.stateManager.WorkState);
				return true;
			}
		}

		return false;
	}

	public void FindFromTarget()
	{
		//Debug.Log("Finding From");

		RawResource resource = FindResource();
		if (resource != null)
		{
			targetFrom = resource.transform;
			//Debug.Log("TargetFrom found (Resource)");
			return;
		}

		Storage storage = FindStorage();
		if (storage != null)
		{
			if (storage.Clan == npc.clan && storage.IsHalfFull())
			{
				targetFrom = storage.transform;
				//Debug.Log("TargetFrom found (Storage)");
				if (targetFrom != targetTo)
					return;
			}
		}

		//Debug.Log("No from target found");
	}

	public void FindToTarget()
	{
		//Debug.Log("Finding To");

		Storage storage = FindStorage();
		if (storage != null)
		{
			//Debug.Log("To Storage not null!");
			if (storage.Clan == npc.clan && !storage.IsFull())
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
		foreach (Collider collider in colliderArray)
		{
			if (collider.TryGetComponent(out RawResource resource))
			{
				nearbyResources.Add(resource);
			}
		}

		if (nearbyResources.Count > 0)
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
		foreach (Collider collider in colliderArray)
		{
			if (collider.TryGetComponent(out Storage storage))
			{
				nearbyStorage.Add(storage);
			}
		}

		if (nearbyStorage.Count > 0)
		{
			//Debug.Log("Storage Found!");
			Storage foundStorage = nearbyStorage[Random.Range(0, nearbyStorage.Count)];
			return foundStorage;
		}
		else
		{
			List<Structure> clanStorage = npc.clan.builder.GetStructures(BuildManager.Build.Storage);
			List<Structure> clanBuildSites = npc.clan.builder.GetStructures(BuildManager.Build.BuildSite);

			if(clanBuildSites.Count > 0)
			{
				BuildSite foundBuildSite = clanBuildSites[Random.Range(0, clanBuildSites.Count)].GetComponent<BuildSite>();
				return foundBuildSite.GetEmptyStorage();
			}

			if(clanStorage.Count > 0)
			{
				Storage foundStorage = clanStorage[Random.Range(0, clanStorage.Count)].GetComponent<Storage>();
				return foundStorage;
			}

			//Debug.Log("Clan has no storage");
			return null;
		}
	}

	public void DoWork()
	{
		//Debug.Log("Working!");
		if (Vector3.Distance(transform.position, currentTarget.position) <= npc.interactRange)
		{
			//Debug.Log("close enough");
			if (currentTarget == targetFrom)
				Withdraw();
			else if (currentTarget == targetTo)
				Deposit();
		}
	}

	public void Withdraw()
	{
		if (IsFull())
		{
			currentTarget = targetTo;
			return;
		}

		IInventory inv = targetFrom.GetComponent<IInventory>();
		ItemData resource = inv.GetResource();
		inv.Remove(resource);
		inventory.Add(resource);

		ItemCarried = resource;
		Instantiate(resource.prefab, carryPoint);
	}

	public void Deposit()
	{
		//Things to keep in mind
		//Are we even carying anything
		//Does where we want to put it match where we are going?

		//We don't actually have anything so we should go grab something
		if (inventory.items.Count == 0)
		{
			currentTarget = targetFrom;
			return;
		}

		IInventory inv = targetTo.GetComponent<IInventory>();

		//We don't want to keep putting things here
		if (inv.IsFull())
		{
			targetTo = null;
			return;
		}

		//If it doesn't have a specified item we shoud make it the same as where we are pulling from
		if (inv.GetResource() == null)
		{
			if (targetFrom == null)
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
		ItemCarried = null;

		foreach (Transform child in carryPoint)
		{
			Destroy(child.gameObject);
		}
	}

	public void DropItem() //TODO if item dropping is added to the game, this will need to be looked at
	{
		if(IsItemCarried())
		{
			inventory.Remove(ItemCarried);
			ItemCarried = null;

			foreach(Transform child in carryPoint)
			{
				Destroy(child.gameObject);
			}

			//Add item to world
		}
	}

	public bool Consume()
	{
		if(IsItemCarried())
		{
			inventory.Remove(ItemCarried);
			ItemCarried = null;

			foreach(Transform child in carryPoint)
			{
				Destroy(child.gameObject);
			}

			return true;
		} else
		{
			return false;
		}
	}

	public bool IsFull()
	{
		int inventorySize = 0;
		foreach(Item item in inventory.items)
		{
			for(int i = 0; i < item.StackSize; i++)
			{
				inventorySize++;
			}
		}
		return inventorySize >= loadCapacity ? true : false;
	}

	public bool IsItemCarried()
	{
		return carryPoint.childCount > 0;
	}

	public void VerifyTargets()
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

		if(IsItemCarried()) //This is checking the item we are carying
		{
			if(inventory.items.Count == 0)
				Debug.LogError("Are we carrying something with an empty inventory?");

			//Do we have the item we are withdrawing?
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
