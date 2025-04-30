using UnityEngine;
using System.Collections.Generic;
using System;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private List<Weapon> availableWeapons = new List<Weapon>();
    private Weapon currentWeapon;

    public static WeaponManager Instance { get; private set; }

    // Dodaj zdarzenie
    public event Action<Weapon> OnWeaponChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (availableWeapons.Count > 0)
        {
            currentWeapon = availableWeapons[0];
            Debug.Log("Aktualna broñ: " + currentWeapon.weaponName);
            OnWeaponChanged?.Invoke(currentWeapon);
        }
        else
        {
            Debug.LogWarning("Brak broni w WeaponManager.");
        }
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public void SwitchWeapon(string weaponName)
    {
        Weapon newWeapon = availableWeapons.Find(w => w.weaponName == weaponName);
        if (newWeapon != null)
        {
            currentWeapon = newWeapon;
            Debug.Log("Broñ zmieniona na: " + currentWeapon.weaponName);
            OnWeaponChanged?.Invoke(currentWeapon);
        }
        else
        {
            Debug.LogError("Broñ o nazwie '" + weaponName + "' nie zosta³a znaleziona w WeaponManager.");
        }
    }
}
