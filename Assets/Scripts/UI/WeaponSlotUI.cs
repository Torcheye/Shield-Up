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
    [SerializeField] private GameObject l2Icon;
    [SerializeField] private GameObject l3Icon;

    private void Awake()
    {
        selectionOutline.SetActive(false);
        transform.rotation = quaternion.identity;
    }
    
    public void OnClick()
    {
        UIManager.Instance.SelectWeaponSlot(this);
    }
    
    public void OnDeselect()
    {
        selectionOutline.SetActive(false);
        l2Icon.SetActive(false);
        l3Icon.SetActive(false);
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
            weaponIcon.sprite = DataManager.Instance.weaponsConfig.GetWeaponSprite(weaponType, level < 3);
            if (level == 2)
            {
                l2Icon.SetActive(true);
                l3Icon.SetActive(false);
            }
            else if (level == 3)
            {
                l2Icon.SetActive(false);
                l3Icon.SetActive(true);
            }
            else
            {
                l2Icon.SetActive(false);
                l3Icon.SetActive(false);
            }
        }
    }
}