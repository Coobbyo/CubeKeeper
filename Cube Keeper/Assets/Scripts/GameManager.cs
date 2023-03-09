using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool doPause;
    [SerializeField] private InputReader input;
    [SerializeField] private GameObject pauseMenu;

    private NPCManager npcManager;

    private void Awake()
    {
        npcManager =  GetComponent<NPCManager>();
    }

    private void Start()
    {
        input.SpeedUpEvent += HandleSpeedUp;
        input.SlowDownEvent += HandleSlowDown;
        input.PauseEvent += HandlePause;
        input.ResumeEvent += HandleResume;
    }

    private void HandleSpeedUp()
    {
        Time.timeScale = 2f;
    }

    private void HandleSlowDown()
    {
        Time.timeScale = 0.5f;
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
}
