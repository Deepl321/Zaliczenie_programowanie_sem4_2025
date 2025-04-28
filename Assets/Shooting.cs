using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float spawnDistance = 1f; // Odległość od środka gracza, z której będzie wystrzeliwany pocisk

    private float nextTimeToFire = 0f;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // Pobierz pozycję kursora myszy w świecie
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 pointToLook = hit.point;
            Vector3 direction = (pointToLook - transform.position).normalized;

            // Oblicz pozycję początkową pocisku
            Vector3 spawnPosition = transform.position + direction * spawnDistance;

            // Stwórz kopię prefabrykatu pocisku w pozycji startowej
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

            // Ustaw kierunek pocisku
            projectile.transform.forward = direction;

            // Nadaj pociskowi prędkość
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = direction * projectileSpeed;
            }

            // Przekaż wartość obrażeń do skryptu pocisku
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.damage = damage;
            }
        }
        else
        {
            // Jeśli raycast nie trafił żadnego obiektu, wyceluj w maksymalny zasięg
            Vector3 pointToLook = ray.GetPoint(1000f); // Zakres strzału
            Vector3 direction = (pointToLook - transform.position).normalized;

            // Oblicz pozycję początkową pocisku
            Vector3 spawnPosition = transform.position + direction * spawnDistance;

            // Stwórz kopię prefabrykatu pocisku w pozycji startowej
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

            // Ustaw kierunek pocisku
            projectile.transform.forward = direction;

            // Nadaj pociskowi prędkość
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = direction * projectileSpeed;
            }

            // Przekaż wartość obrażeń do skryptu pocisku
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.damage = damage;
            }
        }
    }
}
