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

	public virtual void TakeDamage(float damageAmount)
	{
		_DataBaseForm.Health -= damageAmount;
		if (_DataBaseForm.Health > _DataBaseForm.MaxHealth)
		{
			_DataBaseForm.Health = _DataBaseForm.MaxHealth;
		}
		Debug.Log(gameObject.name +" " + damageAmount + " damage taken");
	}

	protected virtual void Awake()
	{
		_Rigidbody = GetComponent<Rigidbody2D>();
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
		_Animator.SetFloat("PlaybackSpeed", 1f);
		_CurrentAnimation = newAnimation;
	}

	protected void PlayAnimationInReverse(string newAnimation)
	{
		_Animator.Play(newAnimation, -1, 1f);
		_Animator.SetFloat("PlaybackSpeed", -1f);
		_CurrentAnimation = newAnimation;
	}

	protected void FlipSprite(bool doFlip)
	{
		_SpriteRenderer.flipX = doFlip;
	}
}
