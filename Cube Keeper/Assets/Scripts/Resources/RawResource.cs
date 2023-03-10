using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RawResource : MonoBehaviour
{
	public ItemData resourceType;

	private Inventory inventory;

	private void Start()
	{
		inventory = new Inventory(resourceType, 5);
		inventory.OnItemChanged += OnResourceDepleted;
	}

	private void OnResourceDepleted()
	{
		if(inventory.items.Count == 0)
			Destroy(gameObject);
	}
}
