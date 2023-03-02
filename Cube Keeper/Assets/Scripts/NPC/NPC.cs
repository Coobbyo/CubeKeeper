using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public enum State
    {
        Move,
        Combat
    }

    public State state;

    public MeshRenderer colorDisplay;
    [SerializeField] private float interactRange = 1f;

    [HideInInspector] public NPCMovement movement;
    [HideInInspector] public NPCSocialBehaviour social;
    [HideInInspector] public NPCCombat combat;
    [HideInInspector] public NPCStats stats;

    private void Awake()
    {
        movement = GetComponent<NPCMovement>();
        social = GetComponent<NPCSocialBehaviour>();
        combat = GetComponent<NPCCombat>();
        stats = GetComponent<NPCStats>();
    }

    private void Start()
    {
        stats.OnHealthReachedZero += Die;
    }

    public NPC FindNearbyNPC()
    {
        var nearbyNPCs = FindNearbyNPCs();

		if(nearbyNPCs.Count <= 0)
			return null;
        else
            return nearbyNPCs[Random.Range(0, nearbyNPCs.Count)];
    }

    public List<NPC> FindNearbyNPCs()
    {
        var nearbyNPCs = new List<NPC>();
		Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
		foreach(Collider collider in colliderArray)
		{
			if(collider.TryGetComponent(out NPC npc))
			{
				nearbyNPCs.Add(npc);
			}
		}

        return nearbyNPCs;
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}
