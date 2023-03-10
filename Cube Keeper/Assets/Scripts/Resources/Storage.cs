using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
	public ItemData storageType;

	[SerializeField] private List<Transform> storageSpots;
	private Dictionary<Transform, bool> fullStorageSpots;

	private int maxStorage = 16;
	private Inventory inventory;

	private void Start()
    {
        inventory = new Inventory();
    }

	public void Add(ItemData data)
	{
		inventory.Add(data);
	}

	public void Remove(ItemData data)
	{
		inventory.Remove(data);
	}

	public bool IsFull()
	{
		return inventory.items.Count == maxStorage ? true : false;
	}

	private void UpdateStorageSpots()
	{
		foreach (Item item in inventory.items)
		{
			for (int i = 0; i < maxStorage; i++)
			{
				
			}
		}
	}
}
