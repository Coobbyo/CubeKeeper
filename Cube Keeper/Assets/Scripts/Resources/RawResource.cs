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
	}

	public void Add(ItemData data) {}

	public void Remove(ItemData data)
	{
		Item item = inventory.Get(data);
		inventory.Remove(data);

		if(inventory.items.Count == 0)
			Destroy(gameObject);

		if(fullIndicator.activeSelf && item.stackSize < maxResources)
			fullIndicator.SetActive(false);
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
		
	}

	public ItemData GetItem()
	{
		return resourceType;
	}

	public List<ItemData> GetItems()
	{
		var items = new List<ItemData>();
		items.Add(resourceType);
		return items;
	}
}
