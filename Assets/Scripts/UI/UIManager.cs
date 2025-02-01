using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD")]
    [SerializeField] private Image playerHpBar;
    [SerializeField] private Image playerXpBar;
    [SerializeField] private PlayerStatusEffectUI[] playerStatusEffectUIs;

    [Header("Effects")] 
    [SerializeField] private Image blindEffect;
    [SerializeField] private Vector2 blindEffectOuterRadius;
    
    [Header("Upgrade Screen")]
    [SerializeField] private GameObject upgradeScreen;
    [SerializeField] private WeaponSlotUI[] weaponSlotUIRing0;
    [SerializeField] private WeaponSlotUI[] weaponSlotUIRing1;
    [SerializeField] private WeaponSlotUI[] weaponSlotUIRing2; 
    [SerializeField] private RingController ringController;
    [SerializeField] private GameObject[] optionsScreens;
        
    private static readonly int PlayerScreenPos = Shader.PropertyToID("_PlayerScreenPos");
    private static readonly int OuterRadius = Shader.PropertyToID("_OuterRadius");
    private static readonly int Value = Shader.PropertyToID("_Value");
    private static readonly int MaxValue = Shader.PropertyToID("_MaxValue");
    
    private float _shuffleTimer;
    private List<WeaponSlotUI> _weaponSlotUIs;
    private WeaponSlotUI _selectedWeaponSlot;
    private bool _upgradeOptionChosen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        blindEffect.gameObject.SetActive(false);
        upgradeScreen.SetActive(false);
        
        _weaponSlotUIs = new List<WeaponSlotUI>();
        for (var i = 0; i < weaponSlotUIRing0.Length; i++)
        {
            _weaponSlotUIs.Add(weaponSlotUIRing0[i]);
            weaponSlotUIRing0[i].SlotIndex = new Vector2Int(0, i);
        }
        for (var i = 0; i < weaponSlotUIRing1.Length; i++)
        {
            _weaponSlotUIs.Add(weaponSlotUIRing1[i]);
            weaponSlotUIRing1[i].SlotIndex = new Vector2Int(1, i);
        }
        for (var i = 0; i < weaponSlotUIRing2.Length; i++)
        {
            _weaponSlotUIs.Add(weaponSlotUIRing2[i]);
            weaponSlotUIRing2[i].SlotIndex = new Vector2Int(2, i);
        }
        _selectedWeaponSlot = null;
    }

    public void UpgradeWeaponOption()
    {
        if (_selectedWeaponSlot == null)
            return;

        Debug.Log("Upgrading weapon option");
        ringController.UpgradeWeapon(_selectedWeaponSlot.SlotIndex);
        UpdateWeaponSlotsUI();
        SetOptionsScreen(5);
    }
    
    public void SelectNewWeaponOption(int type)
    {
        if (_selectedWeaponSlot == null)
            return;

        var weaponType = (WeaponType) type;
        Debug.Log("Selecting new weapon option: " + weaponType);
        ringController.AddNewWeapon(_selectedWeaponSlot.SlotIndex, weaponType);
        UpdateWeaponSlotsUI();
        SetOptionsScreen(5);
    }

    public void SelectWeaponSlot(WeaponSlotUI weaponSlot)
    {
        foreach (var slot in _weaponSlotUIs.Where(slot => slot != weaponSlot))
        {
            slot.OnDeselect();
        }
        weaponSlot.OnSelect();
        _selectedWeaponSlot = weaponSlot;
        Debug.Log("Selected weapon slot: " + weaponSlot.SlotIndex);
        if (_upgradeOptionChosen)
        {
            SetOptionsScreen(5);
        }
        else
        {
            SetOptionsScreen(weaponSlot.Level + 1);
        }
    }
    
    // 0 is default, nothing selected
    private void SetOptionsScreen(int selectedWeaponSlotLevel)
    {
        foreach (var optionsScreen in optionsScreens)
        {
            optionsScreen.SetActive(false);
        }
        optionsScreens[selectedWeaponSlotLevel].SetActive(true);
    }

    public void OpenUpgradeScreen()
    {
        UpdateWeaponSlotsUI();
        upgradeScreen.SetActive(true);
        SetOptionsScreen(0);
        _upgradeOptionChosen = false;
        _selectedWeaponSlot = null;
        foreach (var slot in _weaponSlotUIs)
        {
            slot.OnDeselect();
        }
    }
    
    private void UpdateWeaponSlotsUI()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                ringController.GetWeapon(new Vector2Int(i, j), out var weaponType, out var level);
                _weaponSlotUIs[i * 3 + j].SetWeapon(weaponType, level);
            }
        }
    }
    
    public void CloseUpgradeScreen()
    {
        upgradeScreen.SetActive(false);
        DataManager.Instance.IsGamePaused = false;
    }
    
    public void UpdateBlindEffect(Vector4 screenPos, float progress)
    {
        if (progress <= 0)
        {
            blindEffect.gameObject.SetActive(false);
            return;
        }
        
        blindEffect.gameObject.SetActive(true);
        var mat = blindEffect.material;
        mat.SetVector(PlayerScreenPos, screenPos);
        var outerRadius = Mathf.Lerp(blindEffectOuterRadius.x, blindEffectOuterRadius.y, progress);
        mat.SetFloat(OuterRadius, outerRadius);
    }

    public void UpdatePlayerHp(int currentHp, int maxHp)
    {
        playerHpBar.material.SetFloat(Value, currentHp);
        playerHpBar.material.SetFloat(MaxValue, maxHp);
        playerHpBar.enabled = false;
        playerHpBar.enabled = true;
    }
    
    public void UpdatePlayerXp(int currentXp, int xpToNextLevel)
    {
        playerXpBar.material.SetFloat(Value, currentXp);
        playerXpBar.material.SetFloat(MaxValue, xpToNextLevel);
        playerXpBar.enabled = false;
        playerXpBar.enabled = true;
    }
    
    public void UpdatePlayerStatusEffects(int effectIndex, float progress)
    {
        playerStatusEffectUIs[effectIndex].UpdateStatusEffect(progress);
    }
}