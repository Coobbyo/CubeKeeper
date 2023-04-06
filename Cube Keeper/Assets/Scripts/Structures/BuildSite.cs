using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is horribly desined right now, but I will come back to this in the future

public class BuildSite : Structure, IInventory
{
	public StructureData structureToBuild;
	[SerializeField] private GameObject storagePrefab;
	[SerializeField] private Transform[] storagePositions;

	private Inventory inventory;
	private Storage[] storages;

	override public void Awake()
	{
		base.Awake();
		stats.OnHealthReachedZero += Crumble;
		inventory = new Inventory();
	}

	private void Start()
	{
		//Debug.Log("BuildSite Start() " + structureToBuild.displayName);
		if(structureToBuild == null)
		{
			Debug.Log("There is nothing to build here");
			Crumble();
		}

		int length = structureToBuild.resourceList.Length;
		storages = new Storage[length];
		for(int i = 0; i < length; i++)
		{
			//Debug.Log("Creating new BuildStorage");
			Storage newStorage = Instantiate(
				storagePrefab,
				storagePositions[i].position,
				Quaternion.identity, storagePositions[i]
				).GetComponent<Storage>();
			newStorage.StorageType = structureToBuild.resourceList[i].Data;
			newStorage.MaxStorage = structureToBuild.resourceList[i].StackSize;
			newStorage.OnFull += CheckResources;
			newStorage.Clan = Clan;
			newStorage.parentSite = this;

			storages[i] = newStorage;
		}

		CheckResources();
	}

	public void Add(ItemData data)
	{
		foreach(Storage storage in storages)
		{
			if(data == storage.StorageType)
				storage.Add(data);
		}
	}

	public void Remove(ItemData data)
	{
		//Can't remove from a build site
		/*foreach(Storage storage in storages)
		{
			if(data == storage.StorageType)
				storage.Remove(data);
		}*/
	}

	public bool IsFull()
	{
		int fullStorages = 0;
		int numStorages = 0;
		foreach(Storage storage in storages)
		{
			if(storage.Total() >= storage.MaxStorage)
				fullStorages++;
			numStorages++;
		}

		//Debug.Log(numStorages);

		if(fullStorages >= numStorages)
			return true;
		return false;
	}

	private void CheckResources()
	{
		//if(stoneStorage.Total() >= structureToBuild.resourceAmounts[0] &&
			//woodStorage.Total() >= structureToBuild.resourceAmounts[1])
		if(IsFull())
		{
			BuildManager.Instance.BuildStructure(structureToBuild, transform.position, Clan);
			Crumble();
		}
	}

	public List<Item> GetNeededResources()
	{
		var needs = new List<Item>();
		//Compare what we have to what we need
		foreach(Storage storage in storages)
		{
			int amountNeeded = storage.MaxStorage - storage.Total();
			Item need = new Item(storage.GetResource(), amountNeeded); //TODO this code may need to change if sotrages ever have multiple items
			needs.Add(need);
		}
		return needs;
	}

    List<Item> IInventory.GetItems()
    {
        var items = new List<Item>();
		foreach(Storage storage in storages)
		{
			items.AddRange(storage.GetItems());
		}

		return items;
    }

    public ItemData GetResource()
    {
        var items = GetResources();
		var item = items[Random.Range(0, items.Count)];
		return null;
    }

    public List<ItemData> GetResources()
    {
        var items = new List<ItemData>();
		foreach (Storage storage in storages)
		{
			items.Add(storage.StorageType);
		}
		return items;
    }

	public Storage GetStorage()
	{
		return storages[Random.Range(0, storages.Length)];
	}

	public Storage GetEmptyStorage()
	{
		if(IsFull() || IsCrumbling) return null;

		Storage returnStorage = GetStorage();
		if(returnStorage.IsFull())
		{
			foreach (Storage storage in storages)
			{
				if(!storage.IsFull())
					returnStorage = storage;
			}
		}

		return returnStorage;
	}

    public override void Crumble()
    {
		if(IsCrumbling == true)
			return;

		IsCrumbling = true;
		for(int i = 0; i < storages.Length; i++)
		{
			if(storages[i] == null) continue;
			storages[i].Crumble();
			storages[i] = null;
		}

        base.Crumble();
    }
}
