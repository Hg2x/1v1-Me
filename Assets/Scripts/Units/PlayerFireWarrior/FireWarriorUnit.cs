using FMOD.Studio;
using FMODUnity;
using ICKT.Audio;
using ICKT.Services;
using System.Collections.Generic;
using UnityEngine;

public class FireWarriorUnit : UnitBase
{
	private FireWarriorUnitData _Data;
	[SerializeField] private RuntimeAnimatorController _AnimatorNormal;
	[SerializeField] private RuntimeAnimatorController _AnimatorFire;
	[SerializeField] private DamageCollider _LeftSwordCollider;
	[SerializeField] private DamageCollider _RightSwordCollider;
	private InputHandler _Input; // playerInput

	// TODO: clean up and streamline all the SFX code and assets
	private AudioManager _AudioManager;
	private const string DODGE_AIR_SFX_PATH = "event:/Sfx/FireWarrior/DodgeAir";
	private const string DODGE_GROUND_SFX_PATH = "event:/Sfx/FireWarrior/DodgeGround";
	private const string JUMP_SFX_PATH = "event:/Sfx/FireWarrior/Jump";
	private const string SWORD_HIT_NORMAL_SFX_PATH = "event:/Sfx/FireWarrior/SwordHit_Normal";
	private const string SWORD_HIT_FIRE_SFX_PATH = "event:/Sfx/FireWarrior/SwordHit_Fire";
	private const string TAKE_DAMAGE_SFX_PATH = "event:/Sfx/FireWarrior/TakeDamage";
	private const string TRANSFORMATION_TO_FIRE_SFX_PATH = "event:/Sfx/FireWarrior/TransformationToFire";

	private const string IDLE = "Idle";
	private const string RUN = "Run";
	private const string DODGE_AIR = "DodgeAir";
	private const string DODGE_GROUND = "DodgeGround";
	private const string JUMP = "Jump";
	private const string FALL = "Fall";
	private const string ATTACK = "Attack";
	private const string TRANSFORMATION = "Transformation";

	protected override void Awake()
	{
		base.Awake();
		_Data = _DataBaseForm as FireWarriorUnitData;
		if (_Data == null)
		{
			Debug.LogError("FireWarriorUnitData initialize cast failed");
			return;
		}
		_Data.ResetData();
		_Input = ServiceLocator.Get<InputHandler>();
		_LeftSwordCollider.SetParent(gameObject);
		_RightSwordCollider.SetParent(gameObject);
		SetAttackDamage(_Data.GetAttack());
		EnableSwordCollider(false);

		_AudioManager = ServiceLocator.Get<AudioManager>();
		_LeftSwordCollider.SetHitSfx(SWORD_HIT_NORMAL_SFX_PATH);
		_RightSwordCollider.SetHitSfx(SWORD_HIT_NORMAL_SFX_PATH);
	}

	private void Start()
	{
		ChangeAnimationState(IDLE);
	}

	private void OnEnable()
	{
		_LeftSwordCollider.OnDamageDealt += OnSwordSwingDamageDealt;
		_RightSwordCollider.OnDamageDealt += OnSwordSwingDamageDealt;
	}

	private void OnDisable()
	{
		_LeftSwordCollider.OnDamageDealt -= OnSwordSwingDamageDealt;
		_RightSwordCollider.OnDamageDealt -= OnSwordSwingDamageDealt;
	}

	private void OnSwordSwingDamageDealt()
	{
		if (_Data.IsFireMode)
		{
			TakeDamage(-_Data.LifeDrainAmount);
		}
	}

	private void FixedUpdate()
	{
		if (IsGrounded())
		{
			_Data.JumpAmountLeft = _Data.IsFireMode ? _Data.JumpAmount + _Data.ExtraJumpAmount : _Data.JumpAmount;
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
		if (_Data.IsFireMode) 
		{
			_Data.LifeDrainTickLeft -= Time.deltaTime; // life drain during fire mode
			if (_Data.LifeDrainTickLeft <= 0) 
			{
				TakeDamage(_Data.LifeDrainAmount);
				_Data.LifeDrainTickLeft += _Data.LifeDrainTick;
			}
		}

		if (_Input.DoJump && _Input.InputMoveVector.y < 0) // if space and down button is pressed together
		{
			CheckForElevatedGroundCollider();
			_Input.DoJump = false;
		}

		if (_Input.DoJump && _Data.CanJump())
		{
			_Rigidbody.velocity = new(_Rigidbody.velocity.x, 0);
			_Rigidbody.AddForce(Vector2.up * _Data.JumpForce, ForceMode2D.Impulse);
			_Data.JumpAmountLeft--;
			_AudioManager.PlayOneShot(JUMP_SFX_PATH, transform.position);
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
			if (IsGrounded())
			{
				_AudioManager.PlayOneShot(DODGE_GROUND_SFX_PATH, transform.position);
			}
			else
			{
				_AudioManager.PlayOneShot(DODGE_AIR_SFX_PATH, transform.position);
			}
			_Rigidbody.velocity = new(_Rigidbody.velocity.x, 0);
		}

		UpdateAnimation();
		_Input.StopButtonsInput();
	}

	public void OnAttackAnimationEnd()
	{
		_Data.IsAttacking = false;
		EnableSwordCollider(false);
	}

	public void OnSwordSwing()
	{
		EnableSwordCollider(true);
	}

	public void OnTransformationAnimationEnd()
	{
		EndTransformation(true);
	}

	private void SetAttackDamage(float attack)
	{
		_LeftSwordCollider.SetDamageAmount(attack);
		_RightSwordCollider.SetDamageAmount(attack);
	}

	private void EnableSwordCollider(bool enable)
	{
		if (enable)
		{
			if (IsFacingRight())
			{
				_RightSwordCollider.gameObject.SetActive(true);
			}
			else
			{
				_LeftSwordCollider.gameObject.SetActive(true);
			}
		}
		else
		{
			_RightSwordCollider.gameObject.SetActive(false);
			_LeftSwordCollider.gameObject.SetActive(false);
		}
	}

	private void EndTransformation(bool transformationSuccess)
	{
		_Data.IsTransforming = false;

		if (transformationSuccess)
		{
			ChangeMode();
		}
	}
	
	private void ChangeMode()
	{
		_Data.IsFireMode = !_Data.IsFireMode;
		if (_Data.IsFireMode)
		{
			_Animator.runtimeAnimatorController = _AnimatorFire;
			_LeftSwordCollider.SetHitSfx(SWORD_HIT_FIRE_SFX_PATH);
			_RightSwordCollider.SetHitSfx(SWORD_HIT_FIRE_SFX_PATH);
		}
		else
		{
			_Animator.runtimeAnimatorController = _AnimatorNormal;
			_LeftSwordCollider.SetHitSfx(SWORD_HIT_NORMAL_SFX_PATH);
			_RightSwordCollider.SetHitSfx(SWORD_HIT_NORMAL_SFX_PATH);
		}
		SetAttackDamage(_Data.GetAttack());
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
		if (_Data.IsAttacking || _Data.IsTransforming)
		{
			return;
		}

		float xInput = _Input.InputMoveVector.x;
		bool isMoving = Mathf.Abs(xInput) > _Data.XTolerance;
		if (isMoving)
		{
			FlipSprite(!IsFacingRight());
		}

		bool isGrounded = IsGrounded();
		bool isDodging = _Data.IsDodging();
		bool doAttack = _Input.DoAttack;
		bool doSkill1 = _Input.DoSkill1;

		if (isGrounded)
		{
			if (doSkill1)
			{
				ChangeAnimationState(TRANSFORMATION);
				_Data.IsTransforming = true;
			}
			else if (isDodging)
			{
				ChangeAnimationState(DODGE_GROUND);
			}
			else if (doAttack)
			{
				ChangeAnimationState(ATTACK);
			}
			else if (isMoving)
			{
				ChangeAnimationState(RUN);
			}
			else
			{
				ChangeAnimationState(IDLE);
			}
		}
		else
		{
			if (isDodging)
			{
				ChangeAnimationState(DODGE_AIR);
			}
			else if (doAttack)
			{
				ChangeAnimationState(ATTACK);
			}
			else if (Mathf.Sign(_Rigidbody.velocity.y) == 1f)
			{
				ChangeAnimationState(JUMP);
			}
			else
			{
				ChangeAnimationState(FALL);
			}
		}
	}

	protected override void ChangeAnimationState(string newAnimation)
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

	private void CheckForElevatedGroundCollider()
	{
		float checkRadius = 0.1f; // Adjust the radius as needed
		Vector2 checkPosition = transform.position;
		LayerMask layerMask = LayerMask.GetMask("Default"); // Replace "Boundary" with the layer name for PassThroughBoundary objects

		Collider2D[] colliders = Physics2D.OverlapCircleAll(checkPosition, checkRadius, layerMask);

		foreach (Collider2D collider in colliders)
		{
			ElevatedGroundCollider boundary = collider.GetComponent<ElevatedGroundCollider>(); // TODO: change collider class name later
			if (boundary != null)
			{
				boundary.LetPlayerPassThrough();
			}
		}
	}
}
