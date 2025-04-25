using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class EnemyPatrol : MonoBehaviour
{
    NavMeshAgent Agent;
    [SerializeField] LayerMask groundLayer, playerLayer;
    [SerializeField] float Range;
    Vector3 Destination;
    bool WalkPointSet;

    GameObject player;

    [SerializeField] private float health = 50f; // Dodano zmienn¹ zdrowia

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
    }

    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        if (!WalkPointSet)
        {
            SearchForDestination();
        }

        if (WalkPointSet)
        {
            Agent.SetDestination(Destination);
        }

        if (Vector3.Distance(transform.position, Destination) < 10)
        {
            WalkPointSet = false;
        }
    }

    void SearchForDestination()
    {
        float z = Random.Range(-Range, Range);
        float x = Random.Range(-Range, Range);

        Destination = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        if (Physics.Raycast(Destination, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            WalkPointSet = true;
        }
    }

    // Dodano metodê przyjmuj¹c¹ obra¿enia
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    // Metoda obs³uguj¹ca œmieræ przeciwnika
    void Die()
    {
        Destroy(gameObject);
    }
}
