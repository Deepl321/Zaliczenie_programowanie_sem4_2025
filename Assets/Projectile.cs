using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10f;
    public float lifeTime = 2f;

    [SerializeField] private GameObject hitEffect; // Dodany efekt trafienia
    [SerializeField] private float hitEffectLifeTime = 2f; // Czas �ycia efektu trafienia

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Sprawd�, czy trafiono w przeciwnika
        EnemyPatrol enemy = collision.collider.GetComponent<EnemyPatrol>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Stw�rz efekt trafienia, je�li jest przypisany
        if (hitEffect != null)
        {
            ContactPoint contact = collision.contacts[0];
            GameObject effect = Instantiate(hitEffect, contact.point, Quaternion.LookRotation(contact.normal));

            // Zniszcz efekt po okre�lonym czasie
            Destroy(effect, hitEffectLifeTime);
        }

        // Zniszcz pocisk po trafieniu
        Destroy(gameObject);
    }
}
