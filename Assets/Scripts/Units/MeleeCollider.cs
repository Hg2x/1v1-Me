using UnityEngine;

public class MeleeCollider : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log("collided " + collision.gameObject.name);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("triggered " + collision.gameObject.name);
	}
}
