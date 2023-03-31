using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
	public enum Resource
	{
		Stone,
		Wood,
		Food
	}
	public ItemData[] Resources;

	private static ResourceManager instance;
	public static ResourceManager Instance { get {return instance; } private set{} }
	private void Awake()
	{
		if(instance != null && instance != this)
			Destroy(this);
		else
			instance = this;
	}

	public ItemData GetResource(Resource resource)
	{
		return Resources[(int)resource];
	}
}
