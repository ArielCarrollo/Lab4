using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager Instance;

    // Solo necesitamos los waypoints principales
    public Transform SalaDeJuegos;
    public Transform Dormitorio;
    public Transform Comedor;
    public Transform Banno;

    private void Awake()
    {
        Instance = this;
    }

    // M�todo simplificado para obtener posici�n
    public Vector3 GetWaypointPosition(string locationName)
    {
        switch (locationName)
        {
            case "SalaDeJuegos": return SalaDeJuegos.position;
            case "Dormitorio": return Dormitorio.position;
            case "Comedor": return Comedor.position;
            case "Banno": return Banno.position;
            default: return Vector3.zero;
        }
    }
}

