using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    public GameObject[] trashPrefabs;  // Array of trash object prefabs
    public int numberOfTrashObjects = 10;  // Number of trash objects to generate
    public Vector3 spawnArea;  // Size of the area where trash will be spawned (X, Z define the area on the water surface)

    public float waterHeight = 0f;  // Y position of the water surface

    void Start()
    {
        GenerateTrash();
    }
    private void Update()
    {
        
    }

    void GenerateTrash()
    {
        for (int i = 0; i < numberOfTrashObjects; i++)
        {
            // Generate random X and Z position within the spawn area
            Vector3 randomPos = new Vector3(
                Random.Range(-spawnArea.x / 2, spawnArea.x / 2),
                waterHeight,  // Y position matches the water surface
                Random.Range(-spawnArea.z / 2, spawnArea.z / 2)
            );
            Debug.Log("Instantiating trash at: " + randomPos);

            // Randomly select a trash object to spawn
            GameObject trashPrefab = trashPrefabs[Random.Range(0, trashPrefabs.Length)];

            // Instantiate the trash object at the random position
            GameObject trashInstance = Instantiate(trashPrefab, randomPos, Quaternion.identity);

            // Optionally add some random rotation
            trashInstance.transform.rotation = Random.rotation;
        }
    }
     
}

