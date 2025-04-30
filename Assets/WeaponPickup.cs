using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private string weaponName; // Nazwa broni do podniesienia

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup();
        }
    }

    void Pickup()
    {
        WeaponManager weaponManager = WeaponManager.Instance;
        if (weaponManager != null && !string.IsNullOrEmpty(weaponName))
        {
            weaponManager.SwitchWeapon(weaponName);
            Debug.Log("Podniesiono broñ: " + weaponName);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("WeaponManager jest null lub weaponName nie jest ustawiona.");
        }
    }
}
