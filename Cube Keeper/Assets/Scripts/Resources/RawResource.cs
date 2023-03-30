using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RawResource : MonoBehaviour, IInventory
{
	public ItemData resourceType;

	[SerializeField] private GameObject fullIndicator;
	[SerializeField] private int maxResources = 5;

	private Inventory inventory;

	private void Awake()
	{
		var items = new List<Item>();
		var item = new Item(resourceType, maxResources);
		items.Add(item);
		inventory = new Inventory(items);
	}

	public void Add(ItemData data) {}

	public void Remove(ItemData data)
	{
		Item item = inventory.Get(data);
		inventory.Remove(data);

		if(inventory.items.Count == 0)
			OnResourcesDepleted();

		if(fullIndicator.activeSelf && item.StackSize < maxResources)
			fullIndicator.SetActive(false);
	}

	public bool IsFull()
	{
		Item item = inventory.Get(resourceType);
		if(item != null && item.StackSize == maxResources)
			return true;

		return false;
	}

	private void OnResourcesDepleted()
	{
		//Debug.Log("Destroying");
		Destroy(gameObject);
	}

	public List<Item> GetItems()
	{
		return inventory.items;
	}

	public ItemData GetResource()
	{
		return resourceType;
	}

	public List<ItemData> GetResources()
	{
		var items = new List<ItemData>();
		items.Add(resourceType);
		return items;
	}
}
