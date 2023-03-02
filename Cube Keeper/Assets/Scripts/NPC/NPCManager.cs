using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [SerializeField] private InputReader input;
    [SerializeField] private Transform player;
    [SerializeField] private Transform npcPrefab;
    private List<NPCMovement> npcs = new List<NPCMovement>();

    private static NPCManager instance;
    public static NPCManager Instance { get {return instance; } private set{} }
    private void Awake()
    {
        if(instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    private void Start()
    {
        input.CreateEvent += HandleCreate;
    }

    private void HandleCreate()
    {
        CreateNPC();
    }

    private void CreateNPC()
    {
        float spawnRadius = 0.5f;
        Vector3 spawnPoint = player.position +
            new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius));
        Transform npcGO = Instantiate(npcPrefab, spawnPoint, Quaternion.identity, this.transform);
        NPCMovement newNPC = npcGO.GetComponent<NPCMovement>();
        npcs.Add(newNPC);
    }
}
