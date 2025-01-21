using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlotUI : MonoBehaviour
{
    public Vector2Int SlotIndex { get; set; }
    
    public WeaponType WeaponType { get; private set; }

    public int Level { get; private set; }

    [SerializeField] private Image weaponIcon;
    [SerializeField] private GameObject selectionOutline;

    private void Awake()
    {
        selectionOutline.SetActive(false);
    }

    private void LateUpdate()
    {
        transform.rotation = quaternion.identity;
    }
    
    public void OnClick()
    {
        UIManager.Instance.SelectWeaponSlot(this);
    }
    
    public void OnDeselect()
    {
        selectionOutline.SetActive(false);
    }
    
    public void OnSelect()
    {
        selectionOutline.SetActive(true);
    }
    
    public void SetWeapon(WeaponType weaponType, int level)
    {
        Level = level;
        WeaponType = weaponType;

        if (Level == 0)
        {
            weaponIcon.enabled = false;
        }
        else
        {
            weaponIcon.enabled = true;
            weaponIcon.sprite = DataManager.Instance.weaponsConfig.GetWeaponSprite(weaponType);
        }
    }
}