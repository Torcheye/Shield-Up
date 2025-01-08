using UnityEngine;

public class Shield : Weapon
{
    [SerializeField] private DeflectCollider deflectCollider;

    private void Update()
    {
        deflectCollider.normal = -transform.right;
    }
}