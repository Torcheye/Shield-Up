using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int Id { get; set; }
    public int Level { get; set; }
    
    public void Initialize(int id)
    {
        Id = id;
        Level = 1;
    }
}