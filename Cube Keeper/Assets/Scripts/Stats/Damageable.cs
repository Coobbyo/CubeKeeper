using UnityEngine;

/* From Brackeys */

public class Damageable : MonoBehaviour
{
	public Transform HitPoint;
	public NPCClan Clan;

	public Stat MaxHealth;
	public int CurrentHealth { get; protected set; }

	public Stat Damage;
	public Stat Armor;

	public event System.Action OnHealthReachedZero;
	public event System.Action OnHealthChanged;

	public virtual void Awake()
    {
		CurrentHealth = MaxHealth.GetValue();
		if(HitPoint == null)
			HitPoint = transform;
	}

	public void TakeDamage(int damage)
	{
		damage -= Armor.GetValue();
		//damage = Mathf.Clamp(damage, 0, int.MaxValue);

		CurrentHealth -= damage;
		OnHealthChanged?.Invoke();

		if(CurrentHealth <= 0)
			OnHealthReachedZero?.Invoke();
	}

	public void Heal(int amount)
	{
		CurrentHealth += amount;
		CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth.GetValue());
		OnHealthChanged?.Invoke();
	}
}