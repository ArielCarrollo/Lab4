using System.Linq;
using UnityEngine;
public class ToySpawner : MonoBehaviour
{
    public GameObject toyPrefab;
    public float spawnInterval = 10f;
    public Transform[] spawnPoints;

    void Start()
    {
        InvokeRepeating("SpawnToy", 0f, spawnInterval);
    }

    void SpawnToy()
    {
        // Verifica si ya hay un juguete
        Health existingToy = FindObjectsOfType<Health>().FirstOrDefault(h => h.Entity == Entity.Toy);
        if (existingToy != null)
        {
            // Ya hay un juguete, no spawnear otro
            return;
        }

        // Si no hay juguete, crea uno nuevo
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject toy = Instantiate(toyPrefab, spawnPoint.position, Quaternion.identity);
        toy.GetComponent<Health>().Entity = Entity.Toy;
    }
}
