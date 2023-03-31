using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public List<GameObject> list = new List<GameObject>();

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

	[SerializeField] private float chanceToSpawn = 1f;

	private TickTimer spawnTimer;

	private void Start()
	{
		spawnTimer = new TickTimer(TrySpawn, (int)(SpawnRate * 5f));
	}

	private void TrySpawn()
	{
		if(list.Count < MaxSpawns)
			Spawn();
		else
			Purge();
		
		spawnTimer.Restart();
	}

	private void Spawn()
	{	
		if(Random.value <= 1 - chanceToSpawn)
			return;
		
		Vector3 point = new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y);
		Vector3 spawnPoint = transform.position + point * Radius;

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
		list.Add(spawnGO);
	}

	private void Purge()
	{
		//Debug.Log("Purge");
		for (int i = 0; i < list.Count; i++)
		{
			if(list[i] == null) list.Remove(list[i]);
		}
	}

	private void OnDestroy()
	{
		spawnTimer.Stop();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
