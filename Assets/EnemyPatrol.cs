using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using System;

public class EnemyPatrol : MonoBehaviour
{
    NavMeshAgent Agent;
    [SerializeField] LayerMask groundLayer, playerLayer;
    [SerializeField] float Range;
    [SerializeField] float Sight;
    [SerializeField] float AttackRange;

    Vector3 Destination;

    bool playerInSight;
    bool playerInAttack;
    bool WalkPointSet;

    GameObject player;

    [SerializeField] private float health = 50f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private int damage = 10;
    private float lastAttackTime;

    public event Action<float> OnTakeDamage;

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
    }

    void Update()
    {
        playerInSight = Physics.CheckSphere(transform.position, Sight, playerLayer);
        playerInAttack = Physics.CheckSphere(transform.position, AttackRange, playerLayer);

        if (!playerInSight && !playerInAttack) Patrol();
        if (playerInSight && !playerInAttack) Chase();
        if (playerInSight && playerInAttack) Attack();
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
        float z = UnityEngine.Random.Range(-Range, Range);
        float x = UnityEngine.Random.Range(-Range, Range);

        Destination = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        if (Physics.Raycast(Destination, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            WalkPointSet = true;
        }
    }

    void Chase()
    {
        Agent.SetDestination(player.transform.position);
    }

    void Attack()
    {
        Agent.SetDestination(transform.position); 

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= AttackRange)
            {
                //do skryptu od zdrowia
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.PlayerTakeDamage(damage);
                }

                lastAttackTime = Time.time;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        OnTakeDamage?.Invoke(health);
        if (health <= 0f)
        {
            Die();
        }
    }

    public float GetHealth()
    {
        return health;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
