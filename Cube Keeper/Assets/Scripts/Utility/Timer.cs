using System;
using UnityEngine;

public class Timer
{
	private event Action OnComplete;

	private float delay;
	private float time;
	private bool loop; 

	private bool isDestroyed;

	public Timer(Action action, float timer = 1f, bool doLoop = true)
	{
		OnComplete = action;
		this.delay = timer;
		loop = doLoop;
		isDestroyed = false;
		Restart();
	}

	public void Decrement()
	{
		if(delay == 0f || isDestroyed)
			return;
		
		if(time > 0f)
			time -= Time.deltaTime;
 
		if(time <= 0f)
		{
			OnComplete?.Invoke();
			if(loop)
				Restart();
			else
				DestroySlef();
		}
	}

	public void Restart(float delay)
	{
		this.delay = delay;
		time += delay;
	}

	public void Restart()
	{
		time += delay;
	}

	private void DestroySlef()
	{
		isDestroyed = true;
	}
}
