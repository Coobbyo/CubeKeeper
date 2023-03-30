using System;
using UnityEngine;

public class TickTimer
{
	public bool ShowLogs;
	private event Action OnComplete;

	private int delay;
	private int timeTick;

	private bool isDestroyed;

	public TickTimer(Action action, int timer = 1)
	{
		OnComplete = action;
		this.delay = timer;
		isDestroyed = false;
		Restart();

		TimeTickSystem.OnTick += TimeTickSystem_OnTick;
	}

	public void Stop()
	{
		DestroySlef();
	}

	public void Restart(int delay)
	{
		this.delay = delay;
		Restart();
	}

	public void Restart()
	{
		if(ShowLogs)
			Debug.Log("Restarting Timer");
		isDestroyed = false;
		timeTick += delay;
	}

	private void DestroySlef()
	{
		if(ShowLogs)
			Debug.Log("Timer Destroied");
		isDestroyed = true;
	}

	private void TimeTickSystem_OnTick(int tick)
	{
		if(isDestroyed)
			return;
		
		timeTick--;
 
		if(timeTick <= 0f)
		{
			DestroySlef();
			OnComplete?.Invoke();
		}
	}
}
