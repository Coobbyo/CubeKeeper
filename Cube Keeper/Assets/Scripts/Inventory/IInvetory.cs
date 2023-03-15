using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventory
{
	public void Add(ItemData data);
	public void Remove(ItemData data);
	public List<ItemData> GetItems();
	public ItemData GetItem();
	public bool IsFull();
}
