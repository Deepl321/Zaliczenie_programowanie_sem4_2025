using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10f;
    public float lifeTime = 2f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Sprawdü, czy trafiono w przeciwnika
        EnemyPatrol enemy = collision.collider.GetComponent<EnemyPatrol>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Zniszcz pocisk po trafieniu
        Destroy(gameObject);
    }
}
