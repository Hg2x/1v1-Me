using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageCollider : MonoBehaviour
{
	protected GameObject _Parent;
	protected float _DamageAmount;

	public delegate void DamageDealtDelegate();
	public event DamageDealtDelegate OnDamageDealt;

	public void SetParent(GameObject parent)
	{
		_Parent = parent;
	}

	public void SetDamageAmount(float damageAmount)
	{
		_DamageAmount = damageAmount;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		var go = collision.gameObject;
		if (go == _Parent)
		{
			return;
		}

		if (go.TryGetComponent(out IDamagable damagable))
		{
			damagable.TakeDamage(_DamageAmount);
			OnDamageDealt?.Invoke();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var go = collision.gameObject;
		if (collision.gameObject == _Parent)
		{
			return;
		}

		if (go.TryGetComponent(out IDamagable damagable))
		{
			damagable.TakeDamage(_DamageAmount);
			OnDamageDealt?.Invoke();
		}
	}
}
