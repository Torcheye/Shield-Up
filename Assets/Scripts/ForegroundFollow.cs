using UnityEngine;

public class ForegroundFollow : MonoBehaviour
{
    public Transform cameraTransform; 
    public float followSpeed = 2f;  
    public float offsetStrength = 0.25f; 

    private Vector3 initialOffset; 

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;  
        initialOffset = transform.position - cameraTransform.position;
    }

    void Update()
    {
        Vector3 targetPosition = cameraTransform.position + initialOffset;

        targetPosition.x += Mathf.Sin(Time.time * 2f) * offsetStrength * 0.1f;

        transform.position = new Vector3(
            Mathf.Lerp(transform.position.x, targetPosition.x, followSpeed * Time.deltaTime),
            transform.position.y,
            transform.position.z
            );

    }
}
