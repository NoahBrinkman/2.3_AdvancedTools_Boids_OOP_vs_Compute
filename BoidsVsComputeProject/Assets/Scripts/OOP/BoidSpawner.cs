using System.Collections.Generic;
using UnityEngine;

namespace OOP
{
    
public class BoidSpawner : MonoBehaviour
{
    public Boid boidPrefab;

    public int spawnBoids = 100;

    private List<Boid> _boids;

    private void Start()
    {
        _boids = new List<Boid>();

        for (int i = 0; i < spawnBoids; i++)
        {
            SpawnBoid(boidPrefab.gameObject, 0);
        }
    }

    private void Update()
    {
        foreach (Boid boid in _boids)
        {
            boid.SimulateMovement(_boids, Time.deltaTime);
        }
    }

    private void SpawnBoid(GameObject prefab, int swarmIndex)
    {
        var boidInstance = Instantiate(prefab);
        boidInstance.transform.localPosition += (transform.position + new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)));
        _boids.Add(boidInstance.GetComponent<Boid>());
    }
}
}
