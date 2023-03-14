using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleStorage : Structure, IInventory
{
	[SerializeField] private ItemData storageType;
	public ItemData StorageType
	{
		get { return storageType; }
		set
		{
			storageType = value;
			foreach(Transform spot in storageSpots)
			{
				if(spot.childCount > 0)
					Destroy(spot.GetChild(0).gameObject);
			}

			if(storageType == null)
				return;

			for(int i = 0; i < maxStorage; i++)
			{
				Instantiate(storageType.prefab, storageSpots[i]);
				storageSpots[i].transform.localScale = 0.25f * Vector3.one;
				storageSpots[i].gameObject.SetActive(false);
			}
		}
	}

	//TODO: change this to be a parent object that dynamicaly creates spots at runtime
	[SerializeField] private List<Transform> storageSpots;

	private int maxStorage = 16;
	private Inventory inventory;

	private void Awake()
	{
		inventory = new Inventory();
	}

	private void Start()
	{
		StorageType = storageType;
	}

	public void SetMaxStorage(int max)
	{
		maxStorage = max;
	}

	public void Add(ItemData data)
	{
		if(StorageType == null)
		{
			StorageType = data;
		}
		
		if(IsFull() || data != StorageType)
			return;

		inventory.Add(data);
		UpdateStorageSpots();
	}

	public void Remove(ItemData data)
	{
		inventory.Remove(data);
		UpdateStorageSpots();

		if(inventory.items.Count == 0)
			Destroy(gameObject);
	}

	public int Total()
	{
		int inventorySize = 0;
		if(inventory == null)
		{
			Debug.Log("inventory is null");
			return 0;
		}
		foreach(Item item in inventory.items)
		{
			for(int i = 0; i < item.stackSize; i++)
			{
				inventorySize++;
			}
		}
		return inventorySize;
	}

	public bool IsFull()
	{
		return Total() == maxStorage ? true : false;
	}

	private void UpdateStorageSpots()
	{
		foreach(Transform spot in storageSpots)
		{
			spot.gameObject.SetActive(false);
		}

		foreach(Item item in inventory.items)
		{
			for (int i = 0; i < item.stackSize; i++)
			{
				storageSpots[i].gameObject.SetActive(true);
			}
		}
	}

	public ItemData GetItem()
	{
		return StorageType;
	}
}
