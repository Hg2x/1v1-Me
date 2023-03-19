using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class StoneMechaGolemArmProjectile : DamageCollider
{
    private Animator _Animator;
	private Rigidbody2D _Rigidbody;
	private SpriteRenderer _SpriteRenderer;

	private void Awake()
	{
		_Animator = GetComponent<Animator>();
		_Rigidbody = GetComponent<Rigidbody2D>();
		_SpriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void Launch(float projectileSpeed, Vector2 direction, bool isGlowing = false)
	{
		SetGlow(isGlowing);
		FlipSpriteY(direction.x < 0);
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // get angle between x-axis and direction
		transform.rotation = Quaternion.Euler(0, 0, angle);
		_Rigidbody.velocity = direction.normalized * projectileSpeed;
	}

	private void SetGlow(bool isGlowing)
	{
		_Animator.SetBool("IsGlowing", isGlowing);
	}

	private void FlipSpriteY(bool doFlip)
	{
		_SpriteRenderer.flipY = doFlip;
	}
}
