using UnityEngine;

public class ShieldController : MonoBehaviour
{
    [SerializeField] private Transform shieldPivot;
    [SerializeField] private float rotateSpeed = 5f;

    private void Update()
    {
        transform.position = shieldPivot.position;
        transform.Rotate(Vector3.back, rotateSpeed * Time.deltaTime);
    }
}