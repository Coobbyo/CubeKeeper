using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
	/*private enum MoveState
	{
		Short,
		Medium,
		Long
	}

	private MoveState state;*/

	private NPC npc;

	//[SerializeField] private Transform targetPoint;
	private NavMeshAgent agent;
	private float stopDistance;
	//private float followDistance;

	private TickTimer moveDelay;

	private void Awake()
	{
		npc = GetComponent<NPC>();
		agent = GetComponent<NavMeshAgent>();
	}

	private void Start()
	{
		moveDelay = new TickTimer(MoveTowardsTarget);
		stopDistance = npc.interactRange * 0.9f;
	}

	private void MoveTowardsTarget()
	{
		Vector3 target = transform.position;
		if(npc.target == null)
			target = npc.stateManager.GetTarget();
		else
			target = npc.target.position;

		//Debug.Log("From movement" + target);

		if(Vector3.Distance(transform.position, target) >= stopDistance)
			agent.SetDestination(target);

		moveDelay.Restart(Random.Range(2, 7));
	}

	public Vector3 FindNewDestination(float moveRange)
	{
		Vector3 newTarget = transform.position;
		NPCClan clan = npc.clan;

		if(clan != null)
		{
			newTarget = clan.CenterPoint;
			moveRange = (clan.Size / moveRange) + (moveRange * 0.5f);
		}
		
		newTarget += new Vector3(Random.Range(-moveRange, moveRange), 0f, Random.Range(-moveRange, moveRange));

		//Debug.Log("being set" + target);
		return newTarget;
	}

	private void OnDestroy()
	{
		moveDelay.Stop();
	}
}
