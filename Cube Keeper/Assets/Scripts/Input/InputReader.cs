using System;
using UnityEngine;
using UnityEngine.InputSystem;

//Code from https://www.youtube.com/watch?v=ZHOWqF-b51k

[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, PlayerInput.IPlayerActions, PlayerInput.IUIActions
{
	PlayerInput playerInput;

	private void OnEnable()
	{
		if(playerInput != null)
			return;
		
		playerInput = new PlayerInput();

		playerInput.Player.SetCallbacks(this);
		playerInput.UI.SetCallbacks(this);

		SetPlayer();
	}

	public void SetPlayer()
	{
		playerInput.Player.Enable();
		playerInput.UI.Disable();
	}

	public void SetUI()
	{
		playerInput.Player.Disable();
		playerInput.UI.Enable();
	}

	//Instead of requireing an instance of the InputReader, couldn't I make these events static?
	public event Action<Vector2> MoveEvent;

	public event Action JumpEvent;
	public event Action JumpCancelledEvent;

	public event Action LookEvent;

	public event Action CreateEvent;
	public event Action SuperCreateEvent;

	public event Action SpeedUpEvent;
	public event Action SlowDownEvent;

	public event Action PauseEvent;
	public event Action ResumeEvent;


	public void OnMove(InputAction.CallbackContext context)
	{
		//Debug.Log($"Phase: {context.phase}, Value: {context.ReadValue<Vector2>()}");
		MoveEvent?.Invoke(context.ReadValue<Vector2>());
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		if(context.performed)
			JumpEvent?.Invoke();
		
		if(context.canceled)
			JumpCancelledEvent?.Invoke();
	}

	public void OnLook(InputAction.CallbackContext context)
	{
		LookEvent?.Invoke();
	}

	public void OnCreate(InputAction.CallbackContext context)
	{
		if(context.performed)
		{
			CreateEvent?.Invoke();
		}
	}

	public void OnSuperCreate(InputAction.CallbackContext context)
	{
		SuperCreateEvent?.Invoke();
	}

	public void OnSpeedUpTime(InputAction.CallbackContext context)
	{
		if(context.performed)
		{
			SpeedUpEvent?.Invoke();
		}
	}

	public void OnSlowDownTime(InputAction.CallbackContext context)
	{
		if(context.performed)
		{
			SlowDownEvent?.Invoke();
		}
	}

	 public void OnPause(InputAction.CallbackContext context)
	{
		if(context.performed)
		{
			PauseEvent?.Invoke();
			SetUI();
		}
	}

	public void OnResume(InputAction.CallbackContext context)
	{
		if(context.performed)
		{
			ResumeEvent?.Invoke();
			SetPlayer();
		}
	}

}
