using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class UnitBase : MonoBehaviour
{
	protected Rigidbody2D _Rigidbody;
	private SpriteRenderer _SpriteRenderer;
	private Animator _Animator;
	protected string _CurrentAnimation;

	protected virtual void Awake()
	{
		_Rigidbody = GetComponent<Rigidbody2D>();
		_SpriteRenderer = GetComponent<SpriteRenderer>();
		_Animator = GetComponent<Animator>();
	}

	protected void PlayAnimation(string newAnimation)
	{
		_Animator.Play(newAnimation);
		_CurrentAnimation = newAnimation;
	}

	protected void FlipSprite(bool doFlip)
	{
		_SpriteRenderer.flipX = doFlip;
	}
}
