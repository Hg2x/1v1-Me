using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class GolemProjectilePool : MonoBehaviour
{
	private StoneMechaGolemUnitData _Data;
	private StoneMechaGolemArmProjectile _ProjectilePrefab;
	[SerializeField] private Transform _ProjectileSpawnPoint;

	private ObjectPool<StoneMechaGolemArmProjectile> _Pool;

	private void Awake()
	{
		_Pool = new ObjectPool<StoneMechaGolemArmProjectile>(CreateProjectile, OnGetProjectile, OnReleaseProjectile, OnDestroyProjectile, true);
	}

	public void Initialize(StoneMechaGolemUnitData data, StoneMechaGolemArmProjectile projectilePrefab)
	{
		_Data = data;
		_ProjectilePrefab = projectilePrefab;
	}

	public void LaunchNormalProjectile(Vector2? targetPosition = null)
	{
		LaunchProjectile(false, targetPosition);
	}

	private void LaunchProjectile(bool isGlowing = false, Vector2 ? targetPosition = null)
	{
		var projectile = _Pool.Get();

		projectile.SetDamageAmount(_Data.GetAttack());
		projectile.transform.position = _ProjectileSpawnPoint.position;
		Vector2 direction;
		if (targetPosition == null)
		{
			direction = _ProjectileSpawnPoint.right; // TODO: flip this if flipped
		}
		else
		{
			direction = ((Vector2)targetPosition - (Vector2)projectile.transform.position).normalized;
		}
		projectile.Launch(_Data.ProjectileSpeed, direction, isGlowing);

		StartCoroutine(ReturnProjectileToPool(projectile, _Data.ProjectileDuration));
	}

	private StoneMechaGolemArmProjectile CreateProjectile()
	{
		var projectile = Instantiate(_ProjectilePrefab);
		projectile.SetParent(gameObject);
		projectile.gameObject.SetActive(false);
		return projectile;
	}

	private void OnGetProjectile(StoneMechaGolemArmProjectile projectile)
	{
		projectile.gameObject.SetActive(true);
	}

	private void OnReleaseProjectile(StoneMechaGolemArmProjectile projectile)
	{
		projectile.gameObject.SetActive(false);
	}

	private void OnDestroyProjectile(StoneMechaGolemArmProjectile projectile)
	{
		Destroy(projectile.gameObject);
	}

	private IEnumerator ReturnProjectileToPool(StoneMechaGolemArmProjectile projectile, float delay)
	{
		yield return new WaitForSeconds(delay);
		_Pool.Release(projectile);
	}
}
