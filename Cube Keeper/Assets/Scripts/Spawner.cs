using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public List<Transform> list = new List<Transform>();
	[SerializeField] private GameObject prefab;
	[SerializeField] private float radius;
	[SerializeField] private float proximity; //How close can things spawn to eachother
	[SerializeField] private float spawnRate; //in seconds
	[SerializeField] private int maxSpawns;

	private float oddsToSpawn = 1f;

	private Timer spawnTimer;

	private void Start()
	{
		spawnTimer = new Timer(Spawn, spawnRate);
	}

	private void Update()
	{
		if(list.Count < maxSpawns)
			spawnTimer.Decrement();
	}

	private void Spawn()
	{	
		//TODO: Eventually this needs to be coverted into object pooling

		if(Random.value > 1 / oddsToSpawn)
			return;
		
		//Add sphere overlap colider detection so that objects aren't placed too close together
		Vector3 point = new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y);
		Vector3 spawnPoint = transform.position + point * radius;

		//This code doesn't work right now, cause I probably need a layer mask or something?
		//Could also make an ISpawnable interface? maybe subscribe to an OnDestroy event?
		LayerMask mask = new LayerMask();
		mask |= (1 << LayerMask.NameToLayer("Spawnable"));
		Collider[] colliders = Physics.OverlapSphere(spawnPoint, proximity, mask);
		if(colliders.Length > 0)
		{
			//Debug.Log("No spawning");
			spawnTimer.Restart();
			return;
		}

		Quaternion spawnRotation = Quaternion.identity;

		GameObject spawnGO = Instantiate(prefab, spawnPoint, spawnRotation, transform);
		list.Add(spawnGO.transform);

		spawnTimer.Restart();
	}
}
