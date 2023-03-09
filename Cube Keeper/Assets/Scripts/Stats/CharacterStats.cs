using UnityEngine;

/* From Brackeys */

public class CharacterStats : MonoBehaviour
{
	public Stat MaxHealth;
	public int CurrentHealth { get; protected set; }

	public Stat Damage;
	public Stat Armor;

	public event System.Action OnHealthReachedZero;
	public event System.Action OnHealthChanged;

	public virtual void Awake()
    {
		CurrentHealth = MaxHealth.GetValue();
	}

	public void TakeDamage(int damage)
	{
		damage -= Armor.GetValue();
		damage = Mathf.Clamp(damage, 0, int.MaxValue);

		CurrentHealth -= damage;
		OnHealthChanged?.Invoke();

		if(CurrentHealth <= 0)
		{
			if(OnHealthReachedZero != null)
			{
				OnHealthReachedZero?.Invoke();
			}
		}
	}

	public void Heal(int amount)
	{
		CurrentHealth += amount;
		CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth.GetValue());
		OnHealthChanged();
	}
}