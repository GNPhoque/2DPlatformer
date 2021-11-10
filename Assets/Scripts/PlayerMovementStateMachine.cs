using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementStateMachine : MonoBehaviour
{
    private PlayerState currentState;

    private Animator animator;
    private PlayerMovement player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        OnStateEnter(PlayerState.GROUNDED);
    }

    private void Update()
    {
        OnStateUpdate(currentState); 
    }

    private void OnStateEnter(PlayerState state)
    {
		switch (state)
		{
			case PlayerState.GROUNDED:
				OnEnterGrounded();
				break;

			case PlayerState.JUMPING:
				OnEnterJump();
				break;

			case PlayerState.FALLING:
				OnEnterFall();
				break;
			case PlayerState.WALLSLIDING:
                OnEnterWallSlide();
				break;
			default:
				Debug.LogError("OnStateEnter: Invalid state " + state.ToString());
				break;
		}
	}

	private void OnStateExit(PlayerState state)
    {
		switch (state)
		{
			case PlayerState.GROUNDED:
				OnExitGrounded();
				break;

			case PlayerState.JUMPING:
				OnExitJump();
				break;

			case PlayerState.FALLING:
				OnExitFall();
				break;
			case PlayerState.WALLSLIDING:
                OnExitWallSlide();
				break;
			default:
				Debug.LogError("OnStateExit: Invalid state " + state.ToString());
				break;
		}
	}

	private void OnStateUpdate(PlayerState state)
    {
		switch (state)
		{
			case PlayerState.GROUNDED:
				OnUpdateGrounded();
				break;

			case PlayerState.JUMPING:
				OnUpdateJump();
				break;

			case PlayerState.FALLING:
				OnUpdateFall();
				break;
			case PlayerState.WALLSLIDING:
                OnUpdateWallSlide();
				break;
			default:
				Debug.LogError("OnStateUpdate: Invalid state " + state.ToString());
				break;
		}
	}

	public void TransitionToState(PlayerState toState)
    {
        OnStateExit(currentState);
        currentState = toState;
        OnStateEnter(toState);
    }


    private void OnEnterGrounded()
    {
        player.ResetJumpCounter();
        animator.SetBool("IsGrounded", true);
    }

    private void OnUpdateGrounded()
    {
        if (Input.GetButtonDown("Jump") && player.CanJump())
        {
            TransitionToState(PlayerState.JUMPING);
        }
        else if (!player.IsGrounded())
        {
            TransitionToState(PlayerState.FALLING);
        }
    }

    private void OnExitGrounded()
    {
        animator.SetBool("IsGrounded", false);
    }


    private void OnEnterJump()
    {
        player.Jump();
        animator.SetBool("IsJumping", true);
    }

    private void OnUpdateJump()
    {
        if (Input.GetButtonDown("Jump") && player.CanJump())
        {
            player.Jump();
        }
        else if (player.GetVerticalVelocity() < 0)
        {
            TransitionToState(PlayerState.FALLING);
        }
        else if (player.IsWallSliding())
        {
            TransitionToState(PlayerState.WALLSLIDING);
        }
    }

    private void OnExitJump()
    {
        animator.SetBool("IsJumping", false);
    }


    private void OnEnterFall()
    {
        animator.SetBool("IsFalling", true);
    }

    private void OnUpdateFall()
    {
        if (Input.GetButtonDown("Jump") && player.CanJump())
        {
            TransitionToState(PlayerState.JUMPING);
        }
        else if (player.IsGrounded())
        {
            TransitionToState(PlayerState.GROUNDED);
        }
        else if (player.IsWallSliding())
		{
            TransitionToState(PlayerState.WALLSLIDING);
		}
    }

    private void OnExitFall()
    {
        animator.SetBool("IsFalling", false);
    }


    private void OnEnterWallSlide()
    {
        animator.SetBool("IsWallSliding", true);
    }

    private void OnUpdateWallSlide()
    {
        if (Input.GetButtonDown("Jump") && player.CanJump())
        {
            TransitionToState(PlayerState.JUMPING);
        }
        else if (player.IsGrounded())
        {
            TransitionToState(PlayerState.GROUNDED);
        }
		else if (!player.IsWallSliding())
		{
            TransitionToState(PlayerState.FALLING);
		}
    }

    private void OnExitWallSlide()
    {
        animator.SetBool("IsWallSliding", false);
    }
}
