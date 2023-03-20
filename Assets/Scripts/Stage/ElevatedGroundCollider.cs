using System.Collections;
using UnityEngine;

public class ElevatedGroundCollider : BlockPlayerOnlyCollider
{
	[SerializeField] private float _PassThroughDuration = 1f;
	private bool _LetPlayerPass;

	public void LetPlayerPassThrough()
	{
		_LetPlayerPass = true;	
	}

	protected override void OnCollisionStay2D(Collision2D collision)
	{
		base.OnCollisionStay2D(collision);
		if (collision.gameObject.TryGetComponent(out FireWarriorUnit playerUnit))
		{
			if (_LetPlayerPass)
			{
				StartCoroutine(TemporarilyDisableCollision(collision.collider));
			}
		}
	}

	private IEnumerator TemporarilyDisableCollision(Collider2D otherCollider)
	{
		Physics2D.IgnoreCollision(_Collider, otherCollider, true);
		yield return new WaitForSeconds(_PassThroughDuration);
		Physics2D.IgnoreCollision(_Collider, otherCollider, false);
		_LetPlayerPass = false;
	}
}
