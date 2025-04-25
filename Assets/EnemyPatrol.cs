using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

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

    [SerializeField] private float health = 50f; // Dodano zmienn� zdrowia

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
    }

    void Update()
    {
        //sprawdzanie booli
        playerInSight = Physics.CheckSphere(transform.position, Sight, playerLayer);       
        playerInAttack = Physics.CheckSphere(transform.position, AttackRange, playerLayer);


        //zmiana na odpowiednie akcje w zaleznosci od pozycji gracza
        if(!playerInSight && !playerInAttack)Patrol();
        if(playerInSight && !playerInAttack)Chase();
        if(playerInSight && playerInAttack)Attack();
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

    //metoda gonienia gracza 
    void Chase() {
        Agent.SetDestination(player.transform.position);
    }
    //atakowanie(idk jakos to sie podepnie pod zdrowie twoje)
    void Attack() {
        
    }

    // Dodano metod� przyjmuj�c� obra�enia
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    // Metoda obs�uguj�ca �mier� przeciwnika
    void Die()
    {
        Destroy(gameObject);
    }
}
