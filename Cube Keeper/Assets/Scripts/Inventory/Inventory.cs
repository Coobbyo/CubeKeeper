using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
	private Dictionary<ItemData, Item> dataToItem;
	public List<Item> items { get; private set; }

	public event System.Action OnItemChanged;

	public Inventory() {}
	public Inventory(ItemData referenceData, int amount = 0)
	{
		Add(referenceData, amount);
	}
	public Inventory(Inventory inventory)
	{
		dataToItem = inventory.dataToItem;
		items = inventory.items;
	}

	private void Awake()
	{
		items = new List<Item>();
		dataToItem = new Dictionary<ItemData, Item>();
	}

	public void Add(ItemData referenceData, int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			Add(referenceData);
		}
	}

	public void Add(ItemData referenceData)
	{
		if(dataToItem.TryGetValue(referenceData, out Item value))
		{
			value.AddToStack();
		}
		else
		{
			Item newItem = new Item(referenceData);
			items.Add(newItem);
			dataToItem.Add(referenceData, newItem);
		}

		OnItemChanged?.Invoke();
	}

	public void Remove(ItemData referenceData, int amount)
	{
		if(dataToItem.TryGetValue(referenceData, out Item value))
		{
			if(value.stackSize < amount)
				amount = value.stackSize;
		} else
		{
			return;
		}
		
		for (int i = 0; i < amount; i++)
		{
			Add(referenceData);
		}
	}

	public void Remove(ItemData referenceData)
	{
		if(dataToItem.TryGetValue(referenceData, out Item value))
		{
			value.RemoveFromStack();

			if(value.stackSize == 0) //This could result in an error where the stack is less than 0
			{
				items.Remove(value);
				dataToItem.Remove(referenceData);
			}

			OnItemChanged?.Invoke();
		}
	}

	public Item Get(ItemData referenceData)
	{
		if(dataToItem .TryGetValue(referenceData, out Item value))
			return value;

		return null;
	}
}
