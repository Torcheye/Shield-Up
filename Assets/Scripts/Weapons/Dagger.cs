using UnityEngine;

public class Dagger : Weapon
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Damageable"))
        {
            var damageable = other.GetComponentInParent<Damageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(DataManager.Instance.weaponsConfig.GetDaggerDamage(Level), false);
            }
        }
    }
}