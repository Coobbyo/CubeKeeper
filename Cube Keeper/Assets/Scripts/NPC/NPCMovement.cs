using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
	private enum MoveState
	{
		Short,
		Medium,
		Long
	}

	private MoveState state;

	private NPC npc;

	//[SerializeField] private Transform targetPoint;
	private NavMeshAgent agent;

	private Timer modeDelay;
	private Timer moveDelay;

	private void Awake()
	{
		npc = GetComponent<NPC>();
		agent = GetComponent<NavMeshAgent>();
	}

	private void Start()
	{
		modeDelay = new Timer(ChangeState, Random.Range(0f, 0.1f));
		moveDelay = new Timer(FindNewDestination, Random.Range(0f, 0.1f));
	}

	private void Update()
	{
		switch(npc.state)
		{
			case NPC.State.Roam:
				modeDelay.Decrement();
				moveDelay.Decrement();
				break;
			case NPC.State.Combat:
			case NPC.State.Work:
			MoveTowardsTarget();
				break;
		}
	}

	private void MoveTowardsTarget()
	{
		if(npc.target == null)
			return;
		
		Vector3 target = npc.target.transform.position;
		if(Vector3.Distance(transform.position, target) >= npc.interactRange * 0.9f)
			agent.SetDestination(target);
	}

	private void ChangeState()
	{
		state = (MoveState)Random.Range(0, (int)System.Enum.GetValues(typeof(MoveState)).Cast<MoveState>().Max());

		FindNewDestination();

		modeDelay.Restart(Random.Range(30f, 90f));
	}

	private void FindNewDestination()
	{
		Vector3 newDestination = transform.position;
		NPCClan clan = npc.clan;
		if(clan != null)
			newDestination = clan.CenterPoint;

		float moveRange = clan == null ? 10f : clan.Size / 10 + 2;

		newDestination += new Vector3(Random.Range(-moveRange, moveRange), 0f, Random.Range(-moveRange, moveRange));
		agent.SetDestination(newDestination);

		switch(state)
		{
			case MoveState.Short:
				moveDelay.Restart(Random.Range(1f, 5f));
				break;
			case MoveState.Medium:
				moveDelay.Restart(Random.Range(5f, 10f));
				break;
			case MoveState.Long:
				moveDelay.Restart(Random.Range(10f, 20f));
				break;
		}
	}
}
