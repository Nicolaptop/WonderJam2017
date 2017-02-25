﻿using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PatrolState : IEnemyState

{
    private readonly StatePatternEnemy enemy;
    private int nextWayPoint;

    public PatrolState(StatePatternEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Look();
        Patrol();
    }

    /*public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            ToAlertState();
    }*/

    public void ToPatrolState()
    {
        Debug.Log("Can't transition to same state");
    }

    public void ToAlertState()
    {
        enemy.currentState = enemy.alertState;
    }
    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }

    private void Look()
    {
        RaycastHit hit;
        if (Physics.Raycast(enemy.eyes.transform.position, enemy.eyes.transform.forward, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
        {
            enemy.chaseTarget = hit.transform;
            ToChaseState();
        }
    }

    void Patrol()
    {
        enemy.GetComponent<MeshRenderer>().material.color = Color.green;
        if (enemy.wayPoints.Length > 1)
        {
            enemy.GetComponent<NavMeshAgent>().destination = enemy.wayPoints[nextWayPoint].position;
            enemy.GetComponent<NavMeshAgent>().Resume();

            if (enemy.GetComponent<NavMeshAgent>().remainingDistance <= enemy.GetComponent<NavMeshAgent>().stoppingDistance && !enemy.GetComponent<NavMeshAgent>().pathPending)
            {
                nextWayPoint = (nextWayPoint + 1) % enemy.wayPoints.Length;
            }
        }
        else
        {
            enemy.GetComponent<NavMeshAgent>().destination = enemy.wayPoints[0].position;
            enemy.GetComponent<NavMeshAgent>().Resume();

            if (enemy.GetComponent<NavMeshAgent>().remainingDistance <= enemy.GetComponent<NavMeshAgent>().stoppingDistance && !enemy.GetComponent<NavMeshAgent>().pathPending)
            {
                Vector3 to = new Vector3(0, enemy.direction , 0);
                if (Vector3.Distance(enemy.transform.eulerAngles, to) > 0.01f)
                {
                    enemy.transform.eulerAngles = Vector3.Lerp(enemy.transform.rotation.eulerAngles, to, Time.deltaTime);
                }
                else
                {
                    enemy.transform.eulerAngles = to;
                }
            }
        }


    }
}