using ICKT.Services;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

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

	public void LaunchNormalProjectile()
	{
		LaunchProjectileToPlayer(false);
	}

	private void LaunchProjectileToPlayer(bool isGlowing = false)
	{
		var projectile = _Pool.Get();

		projectile.SetDamageAmount(_Data.GetAttack());
		projectile.transform.position = _ProjectileSpawnPoint.position; // TODO: inverse this if golem is facing left
		Vector3 playerPosition = ServiceLocator.Get<LevelManager>().PlayerTransform.position;
		Vector2 direction = ((Vector2)playerPosition - (Vector2)projectile.transform.position).normalized;
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
