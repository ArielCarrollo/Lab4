using UnityEngine;
using UnityEngine.AI;

public class NavMeshMasterSystem : MonoBehaviour
{
    [Header("Referencias")]
    public NavMeshAgent agent;
    public Transform target; // Para Raycast y TargetReachable

    [Header("Configuración")]
    public float sampleRadius = 1f;
    public float randomRange = 10f;
    public float edgeCheckFrequency = 0.5f;

    private NavMeshPath currentPath;
    private float elapsedEdgeTime;
    private bool showVisuals = true;

    void Update()
    {
        HandleInput();
        UpdateEdgeChecker();
    }

    void HandleInput()
    {
        // Tecla 1: RandomPoint (Movimiento aleatorio)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (RandomPoint(transform.position, randomRange, out Vector3 randomPos))
            {
                agent.SetDestination(randomPos);
                Debug.Log("Movimiento aleatorio a: " + randomPos);
            }
        }

        // Tecla 2: MoveToTargetPosition (Mover a posición específica)
        if (Input.GetKeyDown(KeyCode.Alpha2) && target != null)
        {
            if (MoveToTargetPosition(target.position))
            {
                Debug.Log("Movimiento a objetivo: " + target.position);
            }
        }

        // Tecla 3: CalculatePath (Calcular y mostrar path)
        if (Input.GetKeyDown(KeyCode.Alpha3) && target != null)
        {
            if (CalculatePath(target.position))
            {
                Debug.Log("Path calculado hacia: " + target.position);
            }
        }

        // Tecla 4: Raycast (Verificar línea de visión)
        if (Input.GetKeyDown(KeyCode.Alpha4) && target != null)
        {
            Vector3 hitPos;
            bool blocked = NavMeshRaycast(transform.position, target.position, out hitPos);
            Debug.Log(blocked ? $"Camino bloqueado en {hitPos}" : "Camino despejado");
        }

        // Tecla 5: FindClosestEdge (Borde más cercano)
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Vector3 edgePos;
            float distance;
            if (FindClosestEdge(transform.position, out edgePos, out distance))
            {
                Debug.Log($"Borde más cercano a {distance} metros en {edgePos}");
                agent.SetDestination(edgePos);
            }
        }

        // Tecla V: Toggle visualizaciones
        if (Input.GetKeyDown(KeyCode.V))
        {
            showVisuals = !showVisuals;
            Debug.Log("Visualizaciones: " + (showVisuals ? "ON" : "OFF"));
        }
    }

    // 1. RandomPoint (Tecla 1)
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, sampleRadius, NavMesh.AllAreas))
            {
                result = hit.position;
                if (showVisuals) Debug.DrawLine(center, result, Color.green, 2f);
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    // 2. MoveToTargetPosition (Tecla 2)
    bool MoveToTargetPosition(Vector3 targetPos)
    {
        if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, sampleRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            if (showVisuals) Debug.DrawLine(transform.position, hit.position, Color.blue, 2f);
            return true;
        }
        return false;
    }

    // 3. CalculatePath (Tecla 3)
    bool CalculatePath(Vector3 targetPos)
    {
        currentPath = new NavMeshPath();

        // Aseguramos que la posición del agente esté sobre el NavMesh
        if (!NavMesh.SamplePosition(transform.position, out NavMeshHit startHit, sampleRadius, NavMesh.AllAreas))
        {
            Debug.LogError("La posición del agente NO está sobre el NavMesh.");
            return false;
        }

        // Aseguramos que la posición del target esté sobre el NavMesh
        if (!NavMesh.SamplePosition(targetPos, out NavMeshHit targetHit, sampleRadius, NavMesh.AllAreas))
        {
            Debug.LogError("La posición del target NO está sobre el NavMesh.");
            return false;
        }

        // Intentamos calcular el path
        if (NavMesh.CalculatePath(startHit.position, targetHit.position, NavMesh.AllAreas, currentPath))
        {
            if (currentPath.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetPath(currentPath);
                if (showVisuals) DrawPath(currentPath);
                Debug.Log("✅ Path hacia target calculado exitosamente.");
                return true;
            }
            else
            {
                Debug.LogWarning("⚠️ Path hacia target es incompleto. Status: " + currentPath.status);
            }
        }
        else
        {
            Debug.LogError("❌ No se pudo calcular el path hacia el target.");
        }

        return false;
    }


    // 4. NavMeshRaycast (Tecla 4)
    bool NavMeshRaycast(Vector3 start, Vector3 end, out Vector3 hitPos)
    {
        hitPos = Vector3.zero;

        if (!NavMesh.SamplePosition(start, out NavMeshHit startHit, sampleRadius, NavMesh.AllAreas) ||
            !NavMesh.SamplePosition(end, out NavMeshHit endHit, sampleRadius, NavMesh.AllAreas))
        {
            Debug.LogWarning("Start o End están fuera del NavMesh.");
            return true;
        }

        bool blocked = NavMesh.Raycast(startHit.position, endHit.position, out NavMeshHit hit, NavMesh.AllAreas);
        hitPos = hit.position;

        if (showVisuals)
        {
            Debug.DrawLine(startHit.position, endHit.position, blocked ? Color.red : Color.green, 2f);
            if (blocked) Debug.DrawRay(hitPos, Vector3.up * 2f, Color.yellow, 2f);
        }

        return blocked;
    }


    // 5. FindClosestEdge (Tecla 5)
    bool FindClosestEdge(Vector3 position, out Vector3 edgePos, out float distance)
    {
        edgePos = Vector3.zero;
        distance = 0f;

        if (NavMesh.SamplePosition(position, out NavMeshHit sampledHit, sampleRadius, NavMesh.AllAreas))
        {
            if (NavMesh.FindClosestEdge(sampledHit.position, out NavMeshHit edgeHit, NavMesh.AllAreas))
            {
                edgePos = edgeHit.position;
                distance = edgeHit.distance;

                if (showVisuals)
                {
                    DrawCircle(sampledHit.position, distance, Color.magenta);
                    Debug.DrawRay(edgePos, Vector3.up * 3f, Color.magenta, edgeCheckFrequency);
                }
                return true;
            }
            else
            {
                Debug.LogWarning("No se encontró borde cercano.");
            }
        }
        else
        {
            Debug.LogWarning("Posición no está en el NavMesh.");
        }

        return false;
    }


    void UpdateEdgeChecker()
    {
        if (!showVisuals) return;

        elapsedEdgeTime += Time.deltaTime;
        if (elapsedEdgeTime >= edgeCheckFrequency)
        {
            elapsedEdgeTime = 0f;
            FindClosestEdge(transform.position, out _, out _);
        }
    }

    void DrawPath(NavMeshPath path)
    {
        if (path == null || path.corners.Length < 2) return;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.cyan, 3f);
            Debug.DrawRay(path.corners[i], Vector3.up * 0.5f, Color.white, 3f);
        }
    }

    void DrawCircle(Vector3 center, float radius, Color color)
    {
        Vector3 prevPos = center + new Vector3(radius, 0.1f, 0);
        for (int i = 1; i <= 30; i++)
        {
            float angle = i * Mathf.PI * 2f / 30;
            Vector3 newPos = center + new Vector3(Mathf.Cos(angle) * radius, 0.1f, Mathf.Sin(angle) * radius);
            Debug.DrawLine(prevPos, newPos, color, edgeCheckFrequency);
            prevPos = newPos;
        }
    }
}