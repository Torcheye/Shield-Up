using System.Collections;
using UnityEngine;

public class AcidPool : MonoBehaviour
{
    [SerializeField] private float destroyTime;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(destroyTime);
        AcidPoolFactory.DestroyItem(gameObject);
    }
}