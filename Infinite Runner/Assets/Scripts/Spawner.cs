using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] List<GameObject> blockObjs = new List<GameObject>();
    [SerializeField] float startingSpawnTime = 2f;
    [SerializeField] float ySpawnPos = 6f;
    [SerializeField] float xSpawnRange = 8.5f;

    private void Start()
    {
        StartCoroutine(SpawnBlock());
    }

    IEnumerator SpawnBlock()
    {
        while (true)
        {
            Vector2 spawnPos = new Vector2(Random.Range(-xSpawnRange, xSpawnRange), ySpawnPos);
            GameObject whichBlock = blockObjs[Random.Range(0, blockObjs.Count)];
            Instantiate(whichBlock, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(startingSpawnTime);
        }
    }
}
