using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public ItemData data { get; private set; }
    public int stackSize { get; private set; }

    public Item(ItemData source)
    {
        data = source;
        AddToStack();
    }

    public void AddToStack()
    {
        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }
}
