using System.Collections.Generic;
using UnityEngine;

public class Storage : Structure, IInventory
{
	[SerializeField] private ItemData storageType;
	public ItemData StorageType
	{
		get { return storageType; }
		set
		{
			storageType = value;
			foreach(Transform spot in storageSpots)
			{
				if(spot.childCount > 0)
					Destroy(spot.GetChild(0).gameObject);
			}

			if(storageType == null)
				return;

			for(int i = 0; i < maxStorage; i++)
			{
				Instantiate(storageType.prefab, storageSpots[i]);
				storageSpots[i].gameObject.SetActive(false);
			}
		}
	}

	public event System.Action OnFull;
	[SerializeField] private Transform storageSpotParent;
	private List<Transform> storageSpots = new List<Transform>();

	private int maxStorage = 16;
	private Inventory inventory;

	private void Awake()
	{
		inventory = new Inventory();
		SetMaxStorage(maxStorage);
	}

	public void SetMaxStorage()
	{
		maxStorage = FindLargestSquare(maxStorage);
		CreateStorageSpots();
	}

	public void SetMaxStorage(int max)
	{
		maxStorage = max;
		CreateStorageSpots();
	}

	public void Add(ItemData data)
	{
		if(StorageType == null)
			StorageType = data;
		
		if(IsFull() || data != StorageType)
			return;

		inventory.Add(data);
		UpdateStorageSpots();
		IsFull();
	}

	public void Remove(ItemData data)
	{
		if(data != StorageType)
			return;

		inventory.Remove(data);
		UpdateStorageSpots();

		//if(inventory.items.Count == 0)
			//Destroy(gameObject);
	}

	public int Total()
	{
		int inventorySize = 0;
		foreach(Item item in inventory.items)
		{
			for(int i = 0; i < item.stackSize; i++)
			{
				inventorySize++;
			}
		}
		return inventorySize;
	}

	public bool IsFull()
	{
		if(Total() == maxStorage)
		{
			OnFull?.Invoke();
			return true;
		} else
		{
			return false;
		}
	}

	public bool IsHalfFull()
	{
		if(Total() >= maxStorage * 0.5f)
		{
			return true;
		} else
		{
			return false;
		}
	}

	private void CreateStorageSpots()
	{
		foreach(Transform spot in storageSpots)
		{
			Destroy(spot.gameObject);
		}
		storageSpots.Clear();


		float size = 1f;
		//Vector2 startPointOffset = new Vector2(-size, -size);
		int pointsPerSide = (int)Mathf.Sqrt(FindNextSquare(maxStorage)) + 1;
		float spacing = size * 2 / pointsPerSide;
		for (int i = 1; i < pointsPerSide; i++)
		{
			for (int j = 1; j < pointsPerSide; j++)
			{
				GameObject newGameObject = new GameObject("StorageSpot (" + i + "," + j + ")");
				newGameObject.transform.parent = storageSpotParent;
				newGameObject.transform.localPosition = new Vector3(spacing * i, 0f , spacing * j);
				newGameObject.transform.localScale = 4f / (float)maxStorage * Vector3.one;
				storageSpots.Add(newGameObject.transform);
			}
		}

		StorageType = storageType;
	}

	private int FindLargestSquare(int num)
	{
		int root = Mathf.FloorToInt(Mathf.Sqrt(num));
		return root * root;
	}

	private int FindNextSquare(int num)
	{
		int root = Mathf.CeilToInt(Mathf.Sqrt(num));
		return root * root;
	}

	private void UpdateStorageSpots()
	{
		foreach(Transform spot in storageSpots)
		{
			spot.gameObject.SetActive(false);
		}

		foreach(Item item in inventory.items)
		{
			for (int i = 0; i < item.stackSize; i++)
			{
				if(item.stackSize > maxStorage) { Debug.Log("there are more items than storage!"); }
				storageSpots[i].gameObject.SetActive(true);
			}
		}
	}

	public ItemData GetItem()
	{
		return storageType;
	}

	public List<ItemData> GetItems()
	{
		if(storageType == null)
			return null;
		
		var items = new List<ItemData>();
		items.Add(storageType);
		return items;
	}
}
