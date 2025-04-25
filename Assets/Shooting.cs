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
        Plane groundPlane = new Plane(Vector3.up, -0.7f); // Płaszczyzna na wysokości -0.7

        if (groundPlane.Raycast(ray, out float rayLength))
        {
            Vector3 pointToLook = ray.GetPoint(rayLength);
            Vector3 direction = (pointToLook - new Vector3(transform.position.x, -0.7f, transform.position.z)).normalized;

            // Ustawiamy kierunek w płaszczyźnie XZ
            direction.y = 0f;

            // Oblicz pozycję początkową pocisku na krawędzi gracza na wysokości -0.7
            Vector3 spawnPosition = new Vector3(transform.position.x, -0.7f, transform.position.z) + direction * spawnDistance;

            // Stwórz kopię (klon) prefabrykatu pocisku w pozycji startowej
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

            // Ustaw kierunek pocisku
            projectile.transform.forward = direction;

            // Nadaj pociskowi prędkość tylko w płaszczyźnie XZ
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = direction * projectileSpeed;
            }

            // Przekaż wartość obrażeń do skryptu pocisku, jeśli używasz skryptu Projectile
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.damage = damage;
            }
        }
    }
}
