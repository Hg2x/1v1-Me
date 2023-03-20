using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BlockPlayerOnlyCollider : MonoBehaviour
{
    protected Collider2D _Collider;

	protected virtual void Awake()
	{
		_Collider = GetComponent<Collider2D>();
	}

	protected virtual void OnCollisionEnter2D(Collision2D collision)
	{
		if (!collision.gameObject.TryGetComponent(out FireWarriorUnit playerUnit)) // TODO: change to PlayerUnit when the class exists
		{
			Physics2D.IgnoreCollision(_Collider, collision.collider, true);
		}
	}

	protected virtual void OnCollisionStay2D(Collision2D collision) { }

	protected virtual void OnCollisionExit2D(Collision2D collision) { }
}
