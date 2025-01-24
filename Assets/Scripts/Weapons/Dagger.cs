using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Dagger : Weapon
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float swingDuration;
    [SerializeField] private float swingAngle;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Damageable"))
        {
            var damageable = other.GetComponentInParent<Damageable>();
            if (damageable != null)
            {
                var result = damageable.TakeDamage(DataManager.Instance.weaponsConfig.GetDaggerDamage(Level), IsHostile);
                if (result)
                    StartCoroutine(DoSwingAnimation(transform.position.x > other.transform.position.x));
            }
        }
    }
    
    private IEnumerator DoSwingAnimation(bool left)
    {
        spriteRenderer.transform.DOPunchRotation(new Vector3(0, 0, left ? swingAngle : -swingAngle), swingDuration, 3, 0);
        yield return new WaitForSeconds(swingDuration);
        spriteRenderer.transform.localEulerAngles = Vector3.zero;
    }
}