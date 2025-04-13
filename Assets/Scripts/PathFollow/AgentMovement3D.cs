using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentMovement3D : MonoBehaviour
{
    [Header("NavMesh Settings")]
    public float moveSpeed = 3.5f;
    public float angularSpeed = 120f;
    public float acceleration = 8f;
    public float stoppingDistance = 0.1f;

    private NavMeshAgent navAgent;
    private DataAgent dataAgent;
    private bool isMoving = false;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        dataAgent = GetComponent<DataAgent>();

        ConfigureNavAgent();
    }

    private void ConfigureNavAgent()
    {
        navAgent.speed = moveSpeed;
        navAgent.angularSpeed = angularSpeed;
        navAgent.acceleration = acceleration;
        navAgent.stoppingDistance = stoppingDistance;
        navAgent.autoBraking = true;
        navAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
    }

    public void MoveToLocation(Vector3 destination)
    {
        if (!navAgent.enabled) return;

        isMoving = true;
        dataAgent.IsMoving = true;
        navAgent.isStopped = false;
        navAgent.SetDestination(destination);
    }

    private void Update()
    {
        // Verificar si ha llegado al destino
        if (isMoving && !navAgent.pathPending)
        {
            if (navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f)
                {
                    Stop();
                }
            }
        }
    }

    public void Stop()
    {
        if (!navAgent.enabled) return;

        isMoving = false;
        dataAgent.IsMoving = false;
        navAgent.isStopped = true;
        navAgent.ResetPath();
    }

    public bool IsDone() => !isMoving;

    // Método para actualizar parámetros dinámicamente
    public void UpdateMovementParameters(float speed, float angularSpeed, float accel)
    {
        navAgent.speed = speed;
        navAgent.angularSpeed = angularSpeed;
        navAgent.acceleration = accel;
    }
}