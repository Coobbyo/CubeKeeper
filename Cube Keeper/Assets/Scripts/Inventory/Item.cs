using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
	[SerializeField] private ItemData data;
	public ItemData Data
	{
		get { return data; }
		private set { data = value; }
	}

	[SerializeField] private int stackSize;
	public int StackSize
	{
		get { return stackSize; }
		private set { stackSize = value; }
	}

	public Item(ItemData source)
	{
		Data = source;
		AddToStack();
	}

	public void AddToStack()
	{
		StackSize++;
	}

	public void RemoveFromStack()
	{
		StackSize--;
	}
}
