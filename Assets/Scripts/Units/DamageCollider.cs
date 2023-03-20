using FMOD.Studio;
using ICKT.Audio;
using ICKT.Services;
using System;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
	protected GameObject _Parent;
	protected float _DamageAmount;
	protected string _SfxPath;

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

	public void SetHitSfx(string sfxPath)
	{
		_SfxPath = sfxPath;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		var go = collision.gameObject;
		if (go == _Parent)
		{
			return;
		}

		TryToDamage(go);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var go = collision.gameObject;
		if (go == _Parent)
		{
			return;
		}

		TryToDamage(go);
	}

	private void TryToDamage(GameObject go)
	{
		if (go.TryGetComponent(out IDamagable damagable))
		{
			damagable.TakeDamage(_DamageAmount);
			OnDamageDealt?.Invoke();
			if (!string.IsNullOrEmpty(_SfxPath))
			{
				
				ServiceLocator.Get<AudioManager>().PlayOneShot(_SfxPath, transform.position);
			}
		}
	}
}
