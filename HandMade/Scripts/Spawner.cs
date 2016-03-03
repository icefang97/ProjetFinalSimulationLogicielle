using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{

    public Wave[] waves;
    public Enemy enemy;
    public Transform spawnLocation;
    public float nextSpawnTime;
    Wave currentWave;

    int currentWaveNumber;
    int enemiesRemainingToSpawn;

    float TimeBetweenSpawn;
   

    void Start()
    {
        NextWave();
        TimeBetweenSpawn = nextSpawnTime;
    }

    void Update()
    {
        print("Ennemies remaining to spawn: " + enemiesRemainingToSpawn);
        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            StartCoroutine(Spawn());
        }
       
    }
    IEnumerator Spawn()
    {      
        while (enemiesRemainingToSpawn > 0)
        {
            StartCoroutine(BetweenSpawn());
        }
        if (enemiesRemainingToSpawn == 0)
        {
            nextSpawnTime = Time.time + TimeBetweenSpawn;
            enemiesRemainingToSpawn = currentWave.enemyCount;
        }
        yield return new WaitForSeconds(nextSpawnTime);
    }

    IEnumerator BetweenSpawn()
    {
        enemiesRemainingToSpawn--;
        nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

        Enemy spawnedEnemy = Instantiate(enemy, spawnLocation.position + Vector3.up, Quaternion.identity) as Enemy;

        yield return new WaitForSeconds(currentWave.timeBetweenSpawns);
    }
    void NextWave()
    {
        currentWaveNumber++;
        print("Wave: " + currentWaveNumber);
        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
        }
    }

    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;
    }

}