using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public List<Transform> list = new List<Transform>();

	[SerializeField] private GameObject prefab;
	public GameObject Prefab
	{
		get { return prefab; }
		set
		{
			prefab = value;
		}
	}

	[SerializeField] private float radius;
	public float Radius
	{
		get { return radius; }
		set
		{
			radius = value;
		}
	}

	[SerializeField] private float proximity; //How close can things spawn to eachother
	public float Proximity
	{
		get { return proximity; }
		set
		{
			proximity = value;
		}
	}

	[SerializeField] private float spawnRate; //in seconds
	public float SpawnRate
	{
		get { return spawnRate; }
		set
		{
			spawnRate = value;
		}
	}

	[SerializeField] private int maxSpawns;
	public int MaxSpawns
	{
		get { return maxSpawns; }
		set
		{
			maxSpawns = value;
		}
	}

	private float oddsToSpawn = 1f;

	private Timer spawnTimer;

	private void Start()
	{
		spawnTimer = new Timer(Spawn, SpawnRate);
	}

	private void Update()
	{
		if(list.Count < MaxSpawns)
			spawnTimer.Decrement();
	}

	private void Spawn()
	{	
		//TODO: Eventually this needs to be coverted into object pooling

		if(Random.value > 1 / oddsToSpawn)
			return;
		
		//Add sphere overlap colider detection so that objects aren't placed too close together
		Vector3 point = new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y);
		Vector3 spawnPoint = transform.position + point * Radius;


		//Could also make an ISpawnable interface? maybe subscribe to an OnDestroy event?
		LayerMask mask = new LayerMask();
		mask |= (1 << LayerMask.NameToLayer("Spawnable"));
		Collider[] colliders = Physics.OverlapSphere(spawnPoint, Proximity, mask);
		if(colliders.Length > 0)
		{
			//Debug.Log("No spawning");
			spawnTimer.Restart();
			return;
		}

		Quaternion spawnRotation = Quaternion.identity;

		GameObject spawnGO = Instantiate(Prefab, spawnPoint, spawnRotation, transform);
		list.Add(spawnGO.transform);

		spawnTimer.Restart();
	}
}
