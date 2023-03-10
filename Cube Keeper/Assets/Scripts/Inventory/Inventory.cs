using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Dictionary<ItemData, Item> m_itemDictionary;
    public List<Item> inventory { get; private set; }

    private void Awake()
    {
        inventory = new List<Item>();
        m_itemDictionary = new Dictionary<ItemData, Item>();
    }

    public void Add(ItemData referenceData)
    {
        if(m_itemDictionary.TryGetValue(referenceData, out Item value))
        {
            value.AddToStack();
        }
        else
        {
            Item newItem = new Item(referenceData);
            inventory.Add(newItem);
            m_itemDictionary.Add(referenceData, newItem);
        }
    }

    public void Remove(ItemData referenceData)
    {
        if(m_itemDictionary.TryGetValue(referenceData, out Item value))
        {
            value.RemoveFromStack();

            if(value.stackSize == 0) //This could result in an error where the stakc is less than 0
            {
                inventory.Remove(value);
                m_itemDictionary.Remove(referenceData);
            }
        }
    }

    public Item Get(ItemData referenceData)
    {
        if(m_itemDictionary.TryGetValue(referenceData, out Item value))
        {
            return value;
        }
        return null;
    }
}
