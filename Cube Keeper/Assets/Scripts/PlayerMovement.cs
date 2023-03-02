using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private Rigidbody rb;

	[SerializeField] private InputReader input;

	[SerializeField] private float moveForce = 1f;
	[SerializeField] private float jumpForce = 5f;
	[SerializeField] private float maxSpeed = 5f;

	private Vector3 forceDirection = Vector3.zero;

	private bool isJumping;

	[SerializeField] private Camera playerCam;


	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		input.MoveEvent += HandleMove;

		input.JumpEvent += HandleJump;
		input.JumpCancelledEvent += HandleCancelledJump;
	}

	private void FixedUpdate()
	{
		Move();
		//Jump(); I'm just gonna take this out for now
		LookAt();
	}

	private Vector3 GetCameraRight(Camera playerCam)
    {
        Vector3 right = playerCam.transform.right;
		right.y = 0;
		return right.normalized;
    }

	private Vector3 GetCameraForward(Camera playerCam)
    {
        Vector3 forward = playerCam.transform.forward;
		forward.y = 0;
		return forward.normalized;
    }

	private void HandleMove(Vector2 dir)
	{
		forceDirection = Vector3.zero;
		forceDirection += dir.x * GetCameraRight(playerCam);
		forceDirection += dir.y * GetCameraForward(playerCam);
	}

    private void HandleJump()
	{
		isJumping = true;
	}

	private void HandleCancelledJump()
	{
		isJumping = false;
	}

	private void Move()
	{
		rb.AddForce(forceDirection * moveForce, ForceMode.Impulse); //Right now I think jump is tied to move speed?

		//This is supposed to increase downward velocity when moving down
		// Right now it seems to increase to 5.4 and stop?
		if(rb.velocity.y < 0f || !IsGrounded())
		{
			//rb.velocity -= Vector3.up * Physics.gravity.y * Time.fixedDeltaTime;
		}

		//Capping max velocity
		Vector3 horizontalVelocity = rb.velocity;
		horizontalVelocity.y = 0f;
		if(horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
			rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
	}

	private void Jump()
	{
		//Jumping is all sorts of jank right now...
		if(IsGrounded())
			forceDirection.y = jumpForce;

		if(!isJumping)
			forceDirection.y = 0f;
	}

	private void LookAt()
	{
		Vector3 direction = rb.velocity;
		direction.y = 0f;

		if(forceDirection.sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
			this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
		else
			rb.angularVelocity = Vector3.zero;
	}

	private bool IsGrounded()
	{
		Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
		if(Physics.Raycast(ray, out RaycastHit hit, 0.3f))
			return true;
		else
			return false;
	}
}
