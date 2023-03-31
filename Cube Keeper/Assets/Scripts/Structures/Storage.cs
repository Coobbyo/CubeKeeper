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

			for(int i = 0; i < MaxStorage; i++)
			{
				Instantiate(storageType.prefab, storageSpots[i]);
				storageSpots[i].gameObject.SetActive(false);
			}
		}
	}

	public BuildSite parentSite;
	public event System.Action OnFull;
	[SerializeField] private Transform storageSpotParent;
	private List<Transform> storageSpots = new List<Transform>();

	private int maxStorage = 16;
	public int MaxStorage
	{
		get { return maxStorage; }
		set
		{
			maxStorage = value;
			CreateStorageSpots();
		}
	}
	private Inventory inventory;

	override public void Awake()
	{
		base.Awake();
		stats.OnHealthReachedZero += Crumble;
		inventory = new Inventory();
		MaxStorage = maxStorage;
		//SetMaxStorage(); in order to square? the storage		
	}

	public void SetMaxStorage()
	{
		MaxStorage = FindLargestSquare(MaxStorage);
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
			//Crumble();
	}

	public int Total()
	{
		int inventorySize = 0;
		foreach(Item item in inventory.items)
		{
			for(int i = 0; i < item.StackSize; i++)
			{
				inventorySize++;
			}
		}
		return inventorySize;
	}

	public bool IsFull()
	{
		if(Total() == MaxStorage)
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
		if(Total() >= MaxStorage * 0.5f)
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
		int pointsPerSide = (int)Mathf.Sqrt(FindNextSquare(MaxStorage)) + 1;
		float spacing = size * 2 / pointsPerSide;
		for (int i = 1; i < pointsPerSide; i++)
		{
			for (int j = 1; j < pointsPerSide; j++)
			{
				GameObject newGameObject = new GameObject("StorageSpot (" + i + "," + j + ")");
				newGameObject.transform.parent = storageSpotParent;
				newGameObject.transform.localPosition = new Vector3(spacing * i, 0f , spacing * j);
				//newGameObject.transform.localScale = 4f / (float)maxStorage * Vector3.one;
				newGameObject.transform.localScale = 0.25f * Vector3.one;
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
			for (int i = 0; i < item.StackSize; i++)
			{
				if(item.StackSize > MaxStorage) { Debug.Log("there are more items than storage!"); }
				storageSpots[i].gameObject.SetActive(true);
			}
		}
	}

	public ItemData GetResource()
	{
		return storageType;
	}

	public List<ItemData> GetResources()
	{
		if(storageType == null)
			return null;
		
		var items = new List<ItemData>();
		items.Add(storageType);
		return items;
	}

	public List<Item> GetItems()
	{
		return inventory.items;
	}

    public override void Crumble()
    {
		if(parentSite != null)
			parentSite.Crumble();
        base.Crumble();
    }
}
