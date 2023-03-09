using UnityEngine;

/* From Brackeys */

public class Bullet : MonoBehaviour
{
	private Transform target;

	public float speed = 1f;
	public int damage = 1;

	public float explosionRadius = 0;
	//public GameObject impactEffect;

	public void Seek(Transform _target)
	{
		target = _target;
	}

	public void SetDamage(int damage)
	{
		this.damage = damage;
	}

	private void Update()
	{
		if(target == null)
		{
			Destroy(gameObject);
			return;
		}

		Vector3 dir = target.position - transform.position;
		//dir.y = 0f;
		float distanceThisFrame = speed * Time.deltaTime;

		if(dir.magnitude <= distanceThisFrame)
		{
			HitTarget();
			return;
		}

		transform.Translate(dir.normalized * distanceThisFrame, Space.World);
		transform.LookAt(target);
	}

	private void HitTarget()
	{
		//GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform. rotation);
		//Destroy(effectIns, 5f);

		if(explosionRadius > 0f)
			Explode();
		else
			Damage(target);

		Destroy(gameObject);
	}

	private void Explode()
	{
		Collider[] hitStuff = Physics.OverlapSphere(transform.position, explosionRadius);
		foreach(Collider hitThing in hitStuff)
		{
			if(hitThing.tag == "Enemy")
			{
				Damage(hitThing.transform);
			}
		}
	}

	private void Damage(Transform hit)
	{
		NPC npc = hit.GetComponent<NPC>();

		if(npc != null)
		{
		   npc.stats.TakeDamage(damage);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, explosionRadius);
	}
}
