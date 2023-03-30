using System;
using UnityEngine;

/************** From Code Monkey **************\
 * https://www.youtube.com/watch?v=1hsppNzx7_0
 * https://www.youtube.com/watch?v=NFvmfoRnarY
\**********************************************/

public static class TimeTickSystem
{
	public static event Action<int> OnTick; 	
	public static event Action<int> OnTick_Big; //Once per second

	private const int TICKS_PER_SECOND = 5;
	private const float TICK_TIMER_MAX = 0.2f; //Inverse ticks per second

	private static GameObject timeTickSystemGameObject;
	private static int tick;

	public static void Create()
	{
		if(timeTickSystemGameObject == null)
		{
			timeTickSystemGameObject = new GameObject("TimeTickSystem");
			timeTickSystemGameObject.AddComponent<TimeTickSystemObject>();
		}
	}

	public static int GetTick()
	{
		return tick;
	}

	private class TimeTickSystemObject : MonoBehaviour
	{
		private float tickTimer;

		private void Awake()
		{
			tick = 0;
		}

		private void Update()
		{
			tickTimer += Time.deltaTime;
			if(tickTimer >= TICK_TIMER_MAX)
			{
				tickTimer -= TICK_TIMER_MAX;
				tick++;
				OnTick?.Invoke(tick);
				if(tick % TICKS_PER_SECOND == 0)
					OnTick_Big?.Invoke(tick);
			}
		}
	}
}
