using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSite : MonoBehaviour, IInventory
{
	[SerializeField] private SingleStorage stoneStorage;
	[SerializeField] private SingleStorage woodStorage;

	private int maxStorage = 16;
	private Inventory inventory;

	private void Start()
	{
		inventory = new Inventory();
	}

	public void Add(ItemData data)
	{
		throw new System.NotImplementedException();
	}

	public void Remove(ItemData data)
	{
		throw new System.NotImplementedException();
	}

	public ItemData GetItem()
	{
		throw new System.NotImplementedException();
	}

	public bool IsFull()
	{
		int inventorySize = 0;
		foreach(Item item in inventory.items)
		{
			for(int i = 0; i < item.stackSize; i++)
			{
				inventorySize++;
			}
		}
		return inventorySize == maxStorage ? true : false;
	}

	private void UpdateStorageSpots()
	{}
}
