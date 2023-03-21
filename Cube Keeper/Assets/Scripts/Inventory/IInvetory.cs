using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventory
{
	public void Add(ItemData data);
	public void Remove(ItemData data);
	public List<Item> GetItems();
	public ItemData GetResource();
	public List<ItemData> GetResources();
	public bool IsFull();
}
