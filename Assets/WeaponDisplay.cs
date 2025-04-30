using UnityEngine;
using TMPro;

public class WeaponDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI weaponNameText;

    void Start()
    {
        if (WeaponManager.Instance != null)
        {
            WeaponManager.Instance.OnWeaponChanged += UpdateWeaponName;

            UpdateWeaponName(WeaponManager.Instance.GetCurrentWeapon());
        }
        else
        {
            Debug.LogError("WeaponManager.Instance jest null!");
        }
    }

    void OnDestroy()
    {
        if (WeaponManager.Instance != null)
        {
            WeaponManager.Instance.OnWeaponChanged -= UpdateWeaponName;
        }
    }

    void UpdateWeaponName(Weapon currentWeapon)
    {
        if (currentWeapon != null)
        {
            weaponNameText.text = currentWeapon.weaponName;
        }
        else
        {
            weaponNameText.text = "Brak broni";
        }
    }
}
