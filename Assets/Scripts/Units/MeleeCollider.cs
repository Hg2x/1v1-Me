using System.Data;
using UnityEngine;

public class MeleeCollider : MonoBehaviour
{
	protected GameObject _Parent;
	protected float _DamageAmount;

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
		}

		//Debug.Log("collided " + go.name);
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
		}

		//Debug.Log("triggered " + go.name);
	}
}
