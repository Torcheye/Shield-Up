using System.Collections;
using UnityEngine;

public class PlayerXp : MonoBehaviour
{
    [SerializeField] private float destroyTime;
    [SerializeField] private float cannotBeDestroyedTime;
    
    private bool _cannotBeDestroyed;
    private float _cannotBeDestroyedTimer;

    private void Update()
    {
        if (_cannotBeDestroyed)
        {
            _cannotBeDestroyedTimer += Time.deltaTime;
            if (_cannotBeDestroyedTimer >= cannotBeDestroyedTime)
            {
                _cannotBeDestroyed = false;
            }
        }
    }

    private IEnumerator Start()
    {
        _cannotBeDestroyed = true;
        yield return new WaitForSeconds(destroyTime);
        XpPickupFactory.DestroyItem(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MouthEnhancedAttack") && !_cannotBeDestroyed)
        {
            XpPickupFactory.DestroyItem(gameObject);
        }
    }
}