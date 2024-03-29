using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private bool doPause;
	[SerializeField] private InputReader input;
	[SerializeField] private GameObject pauseMenu;

	private void Start()
	{
		TimeTickSystem.Create();
		//TimeTickSystem.OnTick += ShowTick;
		//TimeTickSystem.OnTick_5 += ShowMegaTick;

		input.SpeedUpEvent += HandleSpeedUp;
		input.SlowDownEvent += HandleSlowDown;
		input.PauseEvent += HandlePause;
		input.ResumeEvent += HandleResume;
	}

	private void HandleSpeedUp()
	{
		Time.timeScale *= 2f;
		if(Time.timeScale > 8f)
			Time.timeScale = 8f;
	}

	private void HandleSlowDown()
	{
		Time.timeScale *= 0.5f;
		if(Time.timeScale < 0.25f)
            Time.timeScale = 0.25f;
	}

	private void HandlePause()
	{
		if(doPause)
			Time.timeScale = 0f;
		pauseMenu.SetActive(true);
	}

	private void HandleResume()
	{
		if(doPause)
			Time.timeScale = 1f;
		pauseMenu.SetActive(false);
	}

	private void ShowTick(int tick)
	{
		//tick = TimeTickSystem.GetTick();
		Debug.Log("tick: " + tick);
	}

	private void ShowMegaTick(int tick)
	{
		Debug.Log("MEGA TICK");
	}
}
