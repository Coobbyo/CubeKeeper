using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour, IInventory
{
	public ItemData storageType;

	[SerializeField] private List<Transform> storageSpots;

	private int maxStorage = 16;
	private Inventory inventory;

	private void Start()
	{
		inventory = new Inventory();

		for (int i = 0; i < maxStorage; i++)
		{
			Instantiate(storageType.prefab, storageSpots[i]);
			storageSpots[i].transform.localScale = 0.25f * Vector3.one;
			storageSpots[i].gameObject.SetActive(false);
		}
	}

	public void Add(ItemData data)
	{
		if(IsFull())
			return;

		inventory.Add(data);
		UpdateStorageSpots();
	}

	public void Remove(ItemData data)
	{
		inventory.Remove(data);
		UpdateStorageSpots();
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
		return inventorySize == maxStorage ? true : false;
	}

	private void UpdateStorageSpots()
	{
		foreach (Transform spot in storageSpots)
		{
			spot.gameObject.SetActive(false);
		}

		foreach (Item item in inventory.items)
		{
			for (int i = 0; i < item.stackSize; i++)
			{
				storageSpots[i].gameObject.SetActive(true);
			}
		}
	}

	public ItemData GetItem()
	{
		return storageType;
	}
}
