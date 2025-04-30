using UnityEngine;

public class Shooting : MonoBehaviour
{
    private float nextTimeToFire = 0f;
    private Camera mainCamera;
    private Weapon currentWeapon;

    void Start()
    {
        mainCamera = Camera.main;
        UpdateWeapon();
    }

    void Update()
    {
        // Aktualizuj broń, jeśli się zmieniła
        if (WeaponManager.Instance != null)
        {
            Weapon newWeapon = WeaponManager.Instance.GetCurrentWeapon();
            if (newWeapon != currentWeapon)
            {
                currentWeapon = newWeapon;
                Debug.Log("Aktualna broń ustawiona na: " + currentWeapon.weaponName);
            }
        }

        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            if (currentWeapon != null)
            {
                nextTimeToFire = Time.time + 1f / currentWeapon.fireRate;
                Shoot();
            }
            else
            {
                Debug.LogWarning("Brak ustawionej broni.");
            }
        }
    }

    void Shoot()
    {
        if (currentWeapon == null || currentWeapon.projectilePrefab == null)
        {
            Debug.LogWarning("Brak broni lub prefabrykatu pocisku.");
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 pointToLook;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            pointToLook = hit.point;
        }
        else
        {
            pointToLook = ray.GetPoint(1000f); // Zakres strzału
        }

        Vector3 direction = (pointToLook - transform.position).normalized;

        Vector3 spawnPosition = transform.position + direction * currentWeapon.spawnDistance;

        GameObject projectile = Instantiate(currentWeapon.projectilePrefab, spawnPosition, Quaternion.identity);

        projectile.transform.forward = direction;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * currentWeapon.projectileSpeed;
        }

        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.damage = currentWeapon.damage;
        }
    }

    void UpdateWeapon()
    {
        if (WeaponManager.Instance != null)
        {
            currentWeapon = WeaponManager.Instance.GetCurrentWeapon();
            Debug.Log("Broń zaktualizowana: " + currentWeapon.weaponName);
        }
        else
        {
            Debug.LogError("WeaponManager nie jest zainicjalizowany.");
        }
    }
}
