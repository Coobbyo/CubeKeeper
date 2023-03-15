using System;
using UnityEngine;

public class Timer
{
	public bool ShowLogs;
	private event Action OnComplete;

	private float delay;
	private float time;

	private bool isDestroyed;

	public Timer(Action action, float timer = 1f)
	{
		OnComplete = action;
		this.delay = timer;
		isDestroyed = false;
		Restart();
	}

	public void Decrement()
	{
		if(isDestroyed)
			return;
		
		time -= Time.deltaTime;
 
		if(time <= 0f)
		{
			DestroySlef();
			OnComplete?.Invoke();
		}
	}

	public void Restart(float delay)
	{
		this.delay = delay;
		Restart();
	}

	public void Restart()
	{
		if(ShowLogs)
			Debug.Log("Restarting Timer");
		isDestroyed = false;
		time += delay;
	}

	private void DestroySlef()
	{
		if(ShowLogs)
			Debug.Log("Timer Destroied");
		isDestroyed = true;
	}
}
