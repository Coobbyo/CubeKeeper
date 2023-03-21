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

	private void Awake()
	{
		inventory = new Inventory();
	}

	private void Start()
	{
		//Debug.Log("BuildSite Start() " + structureToBuild.displayName);
		if(structureToBuild == null)
		{
			Debug.Log("There is nothing to build here");
			Destroy(gameObject);
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

			storages[i] = newStorage;
		}

		CheckResources();
	}

	public void OnDestroy()
	{
		//Debug.Log("Destroy BuildSite");
		for(int i = 0; i < storages.Length; i++)
		{
			Destroy(storages[i].gameObject);
			storages[i] = null;
		}
		Clan.builder.RemoveStructure(this);
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

	public ItemData GetItem()
	{
		var items = GetItems();
		var item = items[Random.Range(0, items.Count)];
		return null;
	}

	public List<ItemData> GetItems()
	{
		var items = new List<ItemData>();
		foreach (Storage storage in storages)
		{
			items.Add(storage.StorageType);
		}
		return items;
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
			Destroy(gameObject);
		}
	}
}
