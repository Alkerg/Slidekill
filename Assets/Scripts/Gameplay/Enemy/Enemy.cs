using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Pool fireballPool;

    private float health = 100f;
    private float chasingRange = 15f;
    private float attackingRange = 2f;
    private float enemyDamage = 5f;
    private Player player;
    private Transform playerTransform;
    private bool isAttacking;


    void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        playerTransform = player.GetComponent<Transform>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        CheckPlayerInChasingRange();
    }

    public void CheckPlayerInChasingRange()
    {
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);

        if (distanceToPlayer <= chasingRange && distanceToPlayer > attackingRange)
        {
            ChasePlayer();
            StopAllCoroutines();
            isAttacking = false;
        }
        else if(distanceToPlayer < attackingRange)
        {
            if (!isAttacking)
            {
                StartCoroutine(AttackPlayer());
            }
        }
    }

    public void ChasePlayer()
    {
        Vector3 targetRotation = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
        navMeshAgent.SetDestination(playerTransform.position);
        transform.LookAt(targetRotation);
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        while (isAttacking)
        {
            player.TakeDamage(enemyDamage);
            yield return new WaitForSeconds(2);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log(health);
    }
}
