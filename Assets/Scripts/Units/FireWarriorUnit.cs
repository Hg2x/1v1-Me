using ICKT.Services;
using UnityEditor.Animations;
using UnityEngine;

public class FireWarriorUnit : UnitBase
{
	[SerializeField] private FireWarriorUnitData _Data;
	[SerializeField] private AnimatorController _AnimatorNormal;
	[SerializeField] private AnimatorController _AnimatorFire;
	private InputHandler _Input; // playerInput

	private const string IDLE = "Idle";
	private const string RUN = "Run";
	private const string DODGE_AIR = "DodgeAir";
	private const string DODGE_GROUND = "DodgeGround";
	private const string JUMP = "Jump";
	private const string FALL = "Fall";
	private const string ATTACK = "Attack";
	private const string TRANSFORMATION = "Transformation";

	// TODO: Proper transformation. Fire Mode. Also change sprite + animator controller based on fire mode or not. Colliders depends on sprite.
	// maybe do DodgeAmount as well and move some functionalities to a separate PlayerUnitBase

	protected override void Awake()
	{
		base.Awake();
		_Data.ResetData();
		_Input = ServiceLocator.Get<InputHandler>();
	}

	private void Start()
	{
		ChangeAnimationState(IDLE);
	}

	private void FixedUpdate()
	{
		if (IsGrounded())
		{
			_Data.JumpAmountLeft = _Data.JumpAmount;
		}

		if (_Data.IsDodging())
		{
			int dodgeDirection = IsFacingRight() ? 1 : -1;
			_Rigidbody.velocity = new(dodgeDirection * _Data.DodgeSpeed, _Rigidbody.velocity.y);
		}
		else
		{
			_Rigidbody.velocity = new(_Input.InputMoveVector.x * _Data.BaseMoveSpeed, _Rigidbody.velocity.y);
		}
	}

	private void Update()
	{
		if (_Input.DoJump && _Data.CanJump())// && isGrounded)
		{
			_Rigidbody.velocity = new(_Rigidbody.velocity.x, 0);
			_Rigidbody.AddForce(Vector2.up * _Data.JumpForce, ForceMode2D.Impulse);
			_Data.JumpAmountLeft--;
		}

		if (_Data.IsDodging())
		{
			_Data.DodgeDurationLeft -= Time.deltaTime;
		}

		if (!_Data.CanDodge())
		{
			_Data.DodgeCooldownLeft -= Time.deltaTime;
		}

		if (_Input.DoDodge && _Data.CanDodge())
		{
			_Data.DodgeDurationLeft = _Data.DodgeDuration;
			_Data.DodgeCooldownLeft = _Data.DodgeCooldown;
			// TODO: maybe change Y velocity if dodging in air? 
		}

		UpdateAnimation();
		_Input.StopButtonsInput();
	}

	public void OnAttackAnimationEnd()
	{
		_Data.IsAttacking = false;
	}

	public void OnTransformationAnimationEnd()
	{
		EndTransformation(true);
	}

	private void EndTransformation(bool transformationSuccess)
	{
		_Data._IsTransforming = false;

		if (transformationSuccess)
		{
			_Data._IsFireMode = !_Data._IsFireMode;
			ChangeMode(_Data._IsFireMode);
		}
	}
	
	private void ChangeMode(bool isFireMode)
	{
		if (isFireMode)
		{
			// change to fire mode
			_Animator.runtimeAnimatorController = _AnimatorFire;
		}
		else
		{
			// change to normal mode
			_Animator.runtimeAnimatorController = _AnimatorNormal;
		}
	}

	private bool IsGrounded()
	{
		return Mathf.Abs(_Rigidbody.velocity.y) < _Data.GroundedTolerance;
	}

	private bool IsMovingHorizontally()
	{
		return Mathf.Abs(_Rigidbody.velocity.x) > _Data.XTolerance;
	}

	private bool IsFacingRight()
	{
		if (IsMovingHorizontally())
		{
			_Data.IsFacingRight = _Rigidbody.velocity.x > 0;
		}
		return _Data.IsFacingRight;
	}

	private void UpdateAnimation()
	{
		if (_Data.IsAttacking || _Data._IsTransforming)
		{
			return;
		}

		// Transformation to change mode
		if (_Input.DoSkill1 && IsGrounded())
		{
			ChangeAnimationState(TRANSFORMATION);
			_Data._IsTransforming = true;
			return;
		}

		float xInput = _Input.InputMoveVector.x;
		bool isMoving = Mathf.Abs(xInput) > _Data.XTolerance;
		if (isMoving)
		{
			FlipSprite(!IsFacingRight());
		}

		if (IsGrounded())
		{
			if (_Data.IsDodging())
			{
				ChangeAnimationState(DODGE_GROUND);
				return;
			}

			if (_Input.DoAttack)
			{
				ChangeAnimationState(ATTACK);
				return;
			}

			if (isMoving)
			{
				ChangeAnimationState(RUN);
				return;
			}

			ChangeAnimationState(IDLE);
		}
		else
		{
			if (_Data.IsDodging())
			{
				ChangeAnimationState(DODGE_AIR);
				return;
			}

			if (_Input.DoAttack)
			{
				ChangeAnimationState(ATTACK);
				return;
			}

			if (Mathf.Sign(_Rigidbody.velocity.y) == 1f)
			{
				ChangeAnimationState(JUMP);
				return;
			}
			else
			{
				ChangeAnimationState(FALL);
			}
		}
	}

	private void ChangeAnimationState(string newAnimation)
	{
		if (_CurrentAnimation == newAnimation)
		{
			return;
		}

		if (newAnimation == ATTACK)
		{
			_Data.IsAttacking = true;
		}

		if (newAnimation != TRANSFORMATION)
		{
			// TODO: change animation if hurt
			EndTransformation(false);
		}

		PlayAnimation(newAnimation);
	}
}
