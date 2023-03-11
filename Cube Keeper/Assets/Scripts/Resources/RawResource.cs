using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RawResource : MonoBehaviour, IInventory
{
	public ItemData resourceType;

	[SerializeField] private GameObject fullIndicator;
	[SerializeField] private int maxResources = 5;

	private Inventory inventory;

	private void Start()
	{
		inventory = new Inventory(resourceType, maxResources);
		inventory.OnItemChanged += OnResourceDepleted;
	}

	public void Add(ItemData data) {}

	public void Remove(ItemData data)
	{
		Item item = inventory.Get(data);
		if(fullIndicator.activeSelf && item != null && item.stackSize < maxResources)
			fullIndicator.SetActive(false);
		
		inventory.Remove(data);
	}

	public bool IsFull()
	{
		Item item = inventory.Get(resourceType);
		if(item != null && item.stackSize == maxResources)
			return true;

		return false;
	}

	private void OnResourceDepleted()
	{
		if(inventory.items.Count == 0)
			Destroy(gameObject);
	}

	public ItemData GetItem()
	{
		return resourceType;
	}
}
