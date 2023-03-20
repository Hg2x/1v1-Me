using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class UnitBase : MonoBehaviour, IDamagable
{
	[SerializeField] protected UnitDataBase _DataBaseForm; // TODO: use another way to fetch this if there's time
	protected Rigidbody2D _Rigidbody;
	private SpriteRenderer _SpriteRenderer;
	protected Animator _Animator;
	protected string _CurrentAnimation;

	public const string EMPTY_ANIMATION = "EmptyAnimation";
	public const string PLAYBACK_SPEED = "PlaybackSpeed"; // TODO: migrate all the const strings somewhere else

	public virtual void TakeDamage(float damageAmount)
	{
		_DataBaseForm.ModifyHealth(-damageAmount);
		Debug.Log(gameObject.name +" " + damageAmount + " damage taken");
	}

	public void SetAnimatorPlaybackSpeed(float speed)
	{
		if (speed >= 0)
		{
			speed = 1f;
		}
		_Animator.SetFloat(PLAYBACK_SPEED, speed);
	}

	protected virtual void Awake()
	{
		_Rigidbody = GetComponent<Rigidbody2D>();
		_Rigidbody.gravityScale = _DataBaseForm.GravityScale;
		_SpriteRenderer = GetComponent<SpriteRenderer>();
		_Animator = GetComponent<Animator>();
	}

	protected virtual void ChangeAnimationState(string newAnimation)
	{
		if (_CurrentAnimation == newAnimation)
		{
			return;
		}

		PlayAnimation(newAnimation);
	}

	protected void PlayAnimation(string newAnimation)
	{
		_Animator.Play(newAnimation);
		_Animator.SetFloat(PLAYBACK_SPEED, 1f);
		_CurrentAnimation = newAnimation;
	}

	protected void PlayAnimationInReverse(string newAnimation)
	{
		_Animator.Play(newAnimation, -1, 1f);
		_Animator.SetFloat(PLAYBACK_SPEED, -1f);
		_CurrentAnimation = newAnimation;
	}

	public float GetCurrentAnimationDuration()
	{
		float playbackSpeed = _Animator.GetFloat(PLAYBACK_SPEED);
		if (playbackSpeed <= 0)
		{
			playbackSpeed = 1;
		}
		AnimatorClipInfo[] clipInfo = _Animator.GetCurrentAnimatorClipInfo(0);
		if (clipInfo.Length > 0)
		{
			return clipInfo[0].clip.length / playbackSpeed;
		}
		return 0;
	}

	protected void FlipSprite(bool doFlip)
	{
		_SpriteRenderer.flipX = doFlip;
	}
}
