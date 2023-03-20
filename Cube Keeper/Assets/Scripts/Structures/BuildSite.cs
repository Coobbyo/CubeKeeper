using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is horribly desined right now, but I will come back to this in the future

public class BuildSite : Structure, IInventory
{
	public StructureData structureToBuild;
	[SerializeField] private Storage stoneStorage;
	[SerializeField] private Storage woodStorage;

	private Inventory inventory;

	private void Awake()
	{
		inventory = new Inventory();
	}

	private void Start()
	{
		//Debug.Log("BuildSite Start() " + structureToBuild.displayName);
		if(structureToBuild == null)
		{
			Debug.Log("There is nothing to build here");
			Destroy(gameObject);
		}

		stoneStorage.SetMaxStorage(structureToBuild.resourceAmounts[0]);
		stoneStorage.OnFull += CheckResources;
		stoneStorage.Clan = Clan;

		woodStorage.SetMaxStorage(structureToBuild.resourceAmounts[1]);
		woodStorage.OnFull += CheckResources;
		woodStorage.Clan = Clan;

		CheckResources();
	}

	public void Add(ItemData data)
	{
		if(data == stoneStorage.StorageType)
			stoneStorage.Add(data);

		if(data == woodStorage.StorageType)
			woodStorage.Add(data);

		CheckResources();
	}

	public void Remove(ItemData data)
	{
		if(data == stoneStorage.StorageType)
			stoneStorage.Remove(data);

		if(data == woodStorage.StorageType)
			woodStorage.Remove(data);
	}

	public ItemData GetItem()
	{
		Debug.Log("what do I do here?");
		CheckResources();
		return null;
	}

	public List<ItemData> GetItems()
	{
		Debug.Log("what do I do here?");
		CheckResources();
		return null;
	}

	public bool IsFull()
	{
		CheckResources();
		if(stoneStorage.IsFull() && woodStorage.IsFull())
			return true;

		return false;
	}

	private void CheckResources()
	{
		if(stoneStorage.Total() >= structureToBuild.resourceAmounts[0] &&
		woodStorage.Total() >= structureToBuild.resourceAmounts[1])
		{
			BuildManager.Instance.BuildStructure(structureToBuild, transform.position, Clan);
			Destroy(gameObject);
		}
	}

	private void OnDestroy()
	{
		Clan.builder.RemoveStructure(this);
	}
}
