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
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject toy = Instantiate(toyPrefab, spawnPoint.position, Quaternion.identity);
        toy.GetComponent<Health>().Entity = Entity.Toy;
    }
}