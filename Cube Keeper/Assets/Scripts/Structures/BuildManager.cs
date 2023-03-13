using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
	public enum Build
	{
		BuildSite,
		Storage,
		ClanHall
	}
	public StructureData[] Structures;
    //maybe have some effects like when built or destroied?

    private static BuildManager instance;
	public static BuildManager Instance { get {return instance; } private set{} }
	private void Awake()
	{
		if(instance != null && instance != this)
			Destroy(this);
		else
			instance = this;
	}

	public void RequestBuild(Build build, Vector3 position)
	{
		StructureData structure = GetStructure(build);
	}

	public StructureData GetStructure(Build build)
	{
		return Structures[(int)build];
	}
}
