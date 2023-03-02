using System;
using UnityEngine;

public class Timer
{
	public float delay = 0f;
	public float time;

	private event Action OnCompleteEvent;

	public void Decrement()
	{
		if(delay == 0f)
			return;
		
		if(time > 0f)
			time -= Time.deltaTime;
 
		if(time <= 0f)
			OnComplete();
	}

	public void Set(Action onComplete, float delay = 1f)
	{
		OnCompleteEvent += onComplete;
		this.delay = delay;
		Restart();
	}

	public void Stop()
	{
		delay = 0;
	}

	public void Reset()
	{
		OnCompleteEvent = null;
		Stop();
	}

	public void Restart()
	{
		time += delay;
	}

	public void Restart(float delay)
	{
		this.delay = delay;
		time += delay;
	}

	private void OnComplete()
	{
		OnCompleteEvent?.Invoke();
		Restart();
	}
}
